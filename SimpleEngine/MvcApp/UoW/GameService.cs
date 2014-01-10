using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using MvcApp.Models;
//using SimpleEngine.Classes.Game.Game = GameEngine;
//using EfDataModels = GameDataBase;

namespace MvcApp.UoW
{
    // TODO: Separate BuisnessObjects parsed from EngineObject -> 
    //      EngineObject could be change independent of MVC Models
    // TODO: decide -> EngineObject in my case can be BO
    // TODO: add some service for getting only engineGames by id -> layer
    public class GameService : BaseService
    {
        public GameService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public GameModel Get(Int32 id)
        {
            var dbGame = GameRepository.Get(id);
            var gameEngine = CustomSpecificParser.DbGameToEngineGame(dbGame);
            return CustomSpecificParser.EngineGameToGameModel(gameEngine);
        }

        public OpenGameListModel GetOpenGameRequests()
        {
            return new OpenGameListModel
            {
                OpenGames = NewGameRequestRepository.GetAll().Select(r => new OpenGameRequestModel { RequestId = r.Id, PlayerId = r.PlayerOneId }).ToList()
            };
        }

        public Int32[] GetFinishedGameIds(Int32 playerId)
        {
            return GameRepository.GetAll().Where(g => g.PlayerOneId == playerId && g.IsFinished).Select(g => g.Id).ToArray();
        }

        public Int32[] GetActualGameIds(Int32 playerId)
        {
            return GameRepository.GetAll().Where(g => g.PlayerOneId == playerId && !g.IsFinished).Select(g => g.Id).ToArray();
        }

        public void Turn(TurnModel turnModel, Int32 playerId)
        {
            // maybe get throw exception if does not exist
            var dbGame = GameRepository.Get(turnModel.GameId);
            if (dbGame == null)
            {
                throw new ArgumentException("Game does not exists.");
            }

            SimpleEngine.Classes.Game.Game gameEngine = CustomSpecificParser.DbGameToEngineGame(dbGame);
            gameEngine.Turn(turnModel.RowIndex, turnModel.ColumnIndex, playerId);
            var changedGame = CustomSpecificParser.EngineGameToDbGame(gameEngine);
            changedGame.Id = dbGame.Id;

            GameRepository.Update(changedGame);
            _unitOfWork.Save();
        }

        public void SkipTurn(Int32 gameId, Int32 playerId)
        {
            // maybe get throw exception if does not exist
            var dbGame = GameRepository.Get(gameId);
            if (dbGame == null)
            {
                throw new ArgumentException("Game does not exists.");
            }

            SimpleEngine.Classes.Game.Game gameEngine = CustomSpecificParser.DbGameToEngineGame(dbGame);
            gameEngine.SkipTurn(playerId);
            var changedGame = CustomSpecificParser.EngineGameToDbGame(gameEngine);
            changedGame.Id = dbGame.Id;

            GameRepository.Update(changedGame);
            _unitOfWork.Save();
        }

        public void Surrender(int gameId, int playerId)
        {
            var dbGame = GameRepository.Get(gameId);
            if (dbGame == null)
            {
                throw new ArgumentException("Game does not exists.");
            }

            SimpleEngine.Classes.Game.Game gameEngine = CustomSpecificParser.DbGameToEngineGame(dbGame);
            gameEngine.Surrender(playerId);

            var changedGame = CustomSpecificParser.EngineGameToDbGame(gameEngine);
            changedGame.Id = dbGame.Id;

            GameRepository.Update(changedGame);
            _unitOfWork.Save();
        }

        //public void Test(int gameId)
        //{
        //    var dbGame = GameRepository.Get(gameId);
        //    if (dbGame == null)
        //    {
        //        throw new ArgumentException("Game does not exists.");
        //    }

        //    SimpleEngine.Classes.Game.Game gameEngine = CustomSpecificParser.DbGameToEngineGame(dbGame);
        //    var json = SimpleEngine.Classes.Game.GameStateSerializer.Serialize(gameEngine.CurrentGameState);
        //    var newGameState = SimpleEngine.Classes.Game.GameStateSerializer.Deserialize(json);
        //}
    }
}
