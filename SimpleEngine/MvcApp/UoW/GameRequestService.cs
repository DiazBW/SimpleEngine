using System;
using MvcApp.Controllers;
using MvcApp.EfDataModels;

namespace MvcApp.UoW
{
    public class GameRequestService : BaseService
    {
        public GameRequestService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public void NewGameRequest(Int32 playerId)
        {
            var newGameRequest = new NewGameRequest()
            {
                PlayerOneId = playerId
            };

            NewGameRequestRepository.SaveNew(newGameRequest);
            _unitOfWork.Save();
        }

        public Int32 GameRequestAccept(int requestId, int playerId)
        {
            // TODO: new game repository - rename
            var openGameRequest = NewGameRequestRepository.Get(requestId);
            if (openGameRequest == null)
            {
                throw new ArgumentException("New game request does not exists.");
            }

            if (openGameRequest.PlayerOneId == playerId)
            {
                throw new ArgumentException("Player can not play with himself.");
            }

            var newDbGame = new Game
            {
                PlayerOneId = openGameRequest.PlayerOneId,
                PlayerTwoId = playerId,
                IsFinished = false
            };

            var gameEngine = new SimpleEngine.Classes.Game.Game(newDbGame.PlayerOneId, newDbGame.PlayerTwoId.Value);
            newDbGame.JsonGameState = SimpleEngine.Classes.Game.GameStateSerializer.Serialize(gameEngine.CurrentGameState);

            // TODO: ?? single responsibility problem ??
            GameRepository.SaveNew(newDbGame);

            NewGameRequestRepository.Remove(requestId);
            _unitOfWork.Save();
            // TODO: required ??
            return newDbGame.Id;
        }
    }
}