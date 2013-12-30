using System;
using System.Collections.Generic;
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

        public GameModel Get(Int32 id)
        {
            var dbGame = GameRepository.Get(id);
            return CustomSpecificParser.DbGameToGameModel(dbGame);
        }

        public GameModel GetActualGameForPlayer(Int32 playerId)
        {
            var dbGame = GameRepository.GetAll().Single(g => g.PlayerOneId == playerId && !g.IsFinished);
            return CustomSpecificParser.DbGameToGameModel(dbGame);
        }

        public void SaveGame(GameModel gameModel)
        {
            var dbGame = CustomSpecificParser.GameModelToDbGame(gameModel);
            _unitOfWork.GameRepository.SaveNew(dbGame);
            _unitOfWork.Save();
        }

        public void UpdateGame(GameModel gameModel, int gameId)
        {
            var existedDbGame = _unitOfWork.GameRepository.Get(gameId);
            var dbGame = CustomSpecificParser.GameModelToDbGame(gameModel);
            dbGame.Id = existedDbGame.Id;

            _unitOfWork.GameRepository.Update(dbGame);
            _unitOfWork.Save();
        }

        public Int32 OpenNewGame(Int32 playerId)
        {
            var newDbGame = new Game
            {
                PlayerOneId = playerId,
                PlayerTwoId = null,
                ActivePlayerId = playerId,
                IsFinished = false
            };

            newDbGame.Json = GameJsonParser.ToJsonString(newDbGame);

            GameRepository.SaveNew(newDbGame);
            _unitOfWork.Save();

            return newDbGame.Id;
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

            var gameModelEmpty = GameModel.GetFake(19, game.PlayerOneId, game.PlayerTwoId.Value);
            UpdateGame(gameModelEmpty, game.Id);
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
    }

    public class CustomSpecificParser
    {
        public static SimpleEngine.Classes.Game.Game DbGameToEngineGame(Game dbGame)
        {
            GameModel gameModel = CustomSpecificParser.DbGameToGameModel(dbGame);

            SimpleEngine.Classes.Game.Game engineGame = new SimpleEngine.Classes.Game.Game(gameModel.PlayerOneId, gameModel.PlayerTwoId);
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

            engineGame.LoadState(dbGame.ActivePlayerId, gameBoard);
            return engineGame;
        }

        public static Game EngineGameToDbGame(SimpleEngine.Classes.Game.Game gameEngine)
        {
            var gameSize = gameEngine.Board.Size;
            var rows = new List<CellRow>(gameSize);

            for (var rowIndex = 0; rowIndex < gameSize; rowIndex++)
            {
                var cells = new List<Cell>(gameSize);
                for (var columnIndex = 0; columnIndex < gameSize; columnIndex++)
                {
                    var cell = new Cell
                    {
                        RowIndex = rowIndex,
                        ColumnIndex = columnIndex,
                        Value = (int)gameEngine.Board.Cells[rowIndex, columnIndex]
                    };
                    cells.Add(cell);
                }
                rows.Add(new CellRow
                {
                    Cells = cells,
                    RowIndex = rowIndex
                });
            }

            GameModel gameModel = new GameModel
            {
                ActivePlayerId = gameEngine.ActivePlayerId,
                PlayerOneId = gameEngine.PlayerOneId,
                PlayerTwoId = gameEngine.PlayerTwoId,
                IsFinished = gameEngine.IsGameOver,
                Rows = rows
            };

            return CustomSpecificParser.GameModelToDbGame(gameModel);
        }

        public static GameModel DbGameToGameModel(Game dbGame)
        {
            GameModel gameModel = JsonConvert.DeserializeObject<GameModel>(dbGame.Json);
            gameModel.ActivePlayerId = dbGame.ActivePlayerId;
            gameModel.PlayerOneId = dbGame.PlayerOneId;
            gameModel.PlayerTwoId = dbGame.PlayerTwoId.Value;
            gameModel.IsFinished = dbGame.IsFinished;
            return gameModel;
        }

        public static Game GameModelToDbGame(GameModel gameModel)
        {
            return new Game
            {
                PlayerOneId = gameModel.PlayerOneId,
                PlayerTwoId = gameModel.PlayerTwoId,
                ActivePlayerId = gameModel.ActivePlayerId,
                IsFinished = gameModel.IsFinished,
                Json = JsonConvert.SerializeObject(gameModel)
            };
        }
    }
}
