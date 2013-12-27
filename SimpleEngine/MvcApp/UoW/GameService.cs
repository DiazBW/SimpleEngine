using System;
using System.Collections.Generic;
using System.Linq;
using MvcApp.EfDataModels;
using MvcApp.Models;
using Newtonsoft.Json;

namespace MvcApp.UoW
{
    public class GameService : BaseService
    {
        public GameService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Game Get(Int32 id)
        {
            return GameRepository.Get(id);
        }

        // add error and not found checks
        public Game GetActualGameForPlayer(String playerIdStr)
        {
            Int32 playerId = 0;
            Int32.TryParse(playerIdStr, out playerId);
            return GameRepository.GetAll().Where(g => g.PlayerOneId == playerId && !g.IsFinished).Single();
        }

        public void Turn(Game game, Int32 playerId)
        {
            SimpleEngine.Classes.Game.Game game = new SimpleEngine.Classes.Game.Game();

            //return GameRepository.GetAll().Where(g => g.PlayerOneId == playerId && !g.IsFinished).Single();
        }

        public void SaveGame(GameModel gameModel)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(gameModel);

            var gameDataObject = new Game
            {
                PlayerOneId = gameModel.PlayerOneId,
                PlayerTwoId = gameModel.PlayerTwoId,
                ActivePlayerId = gameModel.ActivePlayerId,
                IsFinished = gameModel.IsFinished,
                Json = json
            };

            _unitOfWork.GameRepository.SaveNew(gameDataObject);
            _unitOfWork.Save();
        }
    }
}
