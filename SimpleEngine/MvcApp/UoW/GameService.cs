using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using MvcApp.Controllers;
using MvcApp.EfDataModels;
using MvcApp.Models;
using Newtonsoft.Json;
using SimpleEngine.Classes;

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

        public Game GetActualGameForPlayer(Int32 playerId)
        {
            return GameRepository.GetAll().Where(g => g.PlayerOneId == playerId && !g.IsFinished).Single();
        }

        // add error and not found checks
        public Game GetActualGameForPlayer(String playerIdStr)
        {
            Int32 playerId = 0;
            Int32.TryParse(playerIdStr, out playerId);
            return GetActualGameForPlayer(playerId);
        }

        // todo: rewrite this ugly stuff after illness passed.
        //public void Turn(GameModel gameModel, Game game, Int32 playerId)
        //public void TurnV1(GameModel gameModel, Int32 playerId)
        //{
        //    var game = GetActualGameForPlayer(playerId);

        //    Int32 gameSize = gameModel.Rows.Count;
        //    SimpleEngine.Classes.Board gameBoard = new Board(gameSize);
        //    for (var rowIndex = 0; rowIndex < gameSize; rowIndex++)
        //    {
        //        for (var columnIndex = 0; columnIndex < gameSize; columnIndex++)
        //        {
        //            SimpleEngine.Classes.CellType newCellTypeValue = (SimpleEngine.Classes.CellType)Enum.Parse(typeof(SimpleEngine.Classes.CellType), gameModel.Rows[rowIndex].Cells[columnIndex].Value.ToString());
        //            gameBoard.Cells[rowIndex, columnIndex] = newCellTypeValue;
        //        }
        //    }

        //    SimpleEngine.Classes.Game.Game gameEngine = new SimpleEngine.Classes.Game.Game(game.PlayerOneId, game.PlayerTwoId);
        //    gameEngine.LoadState(playerId, gameBoard);

        //    var gameModelForSaving = FillGameModelByEngineGame(gameEngine);
        //    //SaveGame(gameModelForSaving);

        //    UpdateGame(gameModelForSaving, game.Id);
        //    //return GameRepository.GetAll().Where(g => g.PlayerOneId == playerId && !g.IsFinished).Single();
        //}

        public void TurnV2(TurnModel turnModel, Int32 playerId)
        {
            var game = GetActualGameForPlayer(playerId);
            var gameModel = JsonConvert.DeserializeObject<GameModel>(game.Json);

            Int32 gameSize = gameModel.Rows.Count;
            SimpleEngine.Classes.Board gameBoard = new Board(gameSize);
            for (var rowIndex = 0; rowIndex < gameSize; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < gameSize; columnIndex++)
                {
                    SimpleEngine.Classes.CellType newCellTypeValue = (SimpleEngine.Classes.CellType)Enum.Parse(typeof(SimpleEngine.Classes.CellType), gameModel.Rows[rowIndex].Cells[columnIndex].Value.ToString());
                    gameBoard.Cells[rowIndex, columnIndex] = newCellTypeValue;
                }
            }

            SimpleEngine.Classes.Game.Game gameEngine = new SimpleEngine.Classes.Game.Game(game.PlayerOneId, game.PlayerTwoId.Value);
            gameEngine.LoadState(playerId, gameBoard);

            gameEngine.Turn(turnModel.RowIndex, turnModel.ColumnIndex, playerId);

            var gameModelForSaving = FillGameModelByEngineGame(gameEngine);
            //SaveGame(gameModelForSaving);

            UpdateGame(gameModelForSaving, game.Id);
            //return GameRepository.GetAll().Where(g => g.PlayerOneId == playerId && !g.IsFinished).Single();
        }

        private GameModel FillGameModelByEngineGame(SimpleEngine.Classes.Game.Game gameEngine)
        {
            var boardSize = gameEngine.Board.Size;
            var rows = new List<CellRow>(boardSize);
            for (var rowIndex = 0; rowIndex < boardSize; rowIndex++)
            {
                var cells = new List<Cell>(boardSize);
                for (var columnIndex = 0; columnIndex < rows.Count; columnIndex++)
                {
                    cells.Add(new Cell
                    {
                        RowIndex = rowIndex, 
                        ColumnIndex = columnIndex, 
                        Value = (int)gameEngine.Board.Cells[rowIndex,columnIndex]
                    });
                }
                rows.Add(new CellRow { RowIndex = rowIndex, Cells = cells });
            }

            return new GameModel
            {
                ActivePlayerId = gameEngine.ActivePlayerId,
                PlayerOneId = gameEngine.PlayerOneId,
                PlayerTwoId = gameEngine.PlayerTwoId,
                Rows = rows,
                IsFinished = gameEngine.IsGameOver
            };
        }

        public void SaveGame(GameModel gameModel)
        {
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

        public void UpdateGame(GameModel gameModel, int gameId)
        {
            var gameBusinessObject = _unitOfWork.GameRepository.Get(gameId);
            var json = JsonConvert.SerializeObject(gameModel);

            gameBusinessObject.PlayerOneId = gameModel.PlayerOneId;
            gameBusinessObject.PlayerTwoId = gameModel.PlayerTwoId;
            gameBusinessObject.IsFinished = gameModel.IsFinished;
            gameBusinessObject.Json = json;
            gameBusinessObject.ActivePlayerId = gameModel.ActivePlayerId;
            
            _unitOfWork.GameRepository.Update(gameBusinessObject);
            _unitOfWork.Save();
        }

        public Int32 OpenNewGame(Int32 playerId)
        {
            var newGameBusinessObject = new Game
            {
                PlayerOneId = playerId,
                PlayerTwoId = null,
                ActivePlayerId = playerId,
                IsFinished = false
            };

            newGameBusinessObject.Json = GameJsonParser.ToJsonString(newGameBusinessObject);
            GameRepository.SaveNew(newGameBusinessObject);
            _unitOfWork.Save();

            return newGameBusinessObject.Id;
        }

        // custom exceptions
        public void CloseGame(int gameId, int playerId)
        {
            var game = GameRepository.Get(gameId);
            if (game == null)
            {
                throw new ArgumentException("Game does not exists.");
            }
            if (game.PlayerTwoId != null)
            {
                throw new ArgumentException("Game is already closed.");
            }

            game.PlayerTwoId = playerId;
            GameRepository.Update(game);
            _unitOfWork.Save();
        }

        public Game Turn(TurnModel turnModel, Int32 playerId)
        {
            // maybe get throw exception if does not exist
            var game = GameRepository.Get(turnModel.GameId);
            if (game == null)
            {
                throw new ArgumentException("Game does not exists.");
            }

            SimpleEngine.Classes.Game.Game innerGame = GetInnerGame(game);
            innerGame.Turn(turnModel.RowIndex, turnModel.ColumnIndex, playerId);
            game.Json = GetOuterGameJson(innerGame);

            // todo: move into converter
            if (game.ActivePlayerId == game.PlayerOneId)
            {
                game.ActivePlayerId = game.PlayerTwoId.Value; 
            }
            else
            {
                game.ActivePlayerId = game.PlayerOneId; 
            }

            GameRepository.Update(game);
            _unitOfWork.Save();

            return game;
        }

        private String GetOuterGameJson(SimpleEngine.Classes.Game.Game innerGame)
        {
            var gameModel = FillGameModelByEngineGame(innerGame);
            return JsonConvert.SerializeObject(gameModel);
        }

        private SimpleEngine.Classes.Game.Game GetInnerGame(Game game)
        {
            GameModel gameModel = JsonConvert.DeserializeObject<GameModel>(game.Json);

            Int32 gameSize = gameModel.Rows.Count;
            SimpleEngine.Classes.Board gameBoard = new Board(gameSize);
            for (var rowIndex = 0; rowIndex < gameSize; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < gameSize; columnIndex++)
                {
                    SimpleEngine.Classes.CellType newCellTypeValue = (SimpleEngine.Classes.CellType)Enum.Parse(typeof(SimpleEngine.Classes.CellType), gameModel.Rows[rowIndex].Cells[columnIndex].Value.ToString());
                    gameBoard.Cells[rowIndex, columnIndex] = newCellTypeValue;
                }
            }

            SimpleEngine.Classes.Game.Game resGame = new SimpleEngine.Classes.Game.Game(gameModel.PlayerOneId, gameModel.PlayerTwoId);
            resGame.LoadState(gameModel.ActivePlayerId, gameBoard);

            return resGame;
        }
    }
}
