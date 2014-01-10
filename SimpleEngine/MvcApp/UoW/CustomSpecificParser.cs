using System;
using System.Collections.Generic;
using MvcApp.EfDataModels;
using MvcApp.Models;
using Newtonsoft.Json;
using SimpleEngine.Classes;

namespace MvcApp.UoW
{
    // TODO: maybe instead of half of them use GameStateSerializer
    public class CustomSpecificParser
    {
        public static SimpleEngine.Classes.Game.Game DbGameToEngineGame(Game dbGame)
        {
            SimpleEngine.Classes.Game.GameState engineGameState = SimpleEngine.Classes.Game.GameStateSerializer.Deserialize(dbGame.JsonGameState);
            // TODO: playerTwoId to NOT NULL ! -> deprecated cause of new table for open game requests
            SimpleEngine.Classes.Game.Game engineGame = new SimpleEngine.Classes.Game.Game(dbGame.PlayerOneId, dbGame.PlayerTwoId.Value);
            engineGame.LoadState(engineGameState);
            return engineGame;
        }

        public static Game EngineGameToDbGame(SimpleEngine.Classes.Game.Game gameEngine)
        {
            var gameState = gameEngine.CurrentGameState;
            return new Game
            {
                PlayerOneId = gameState.PlayerOneId,
                PlayerTwoId = gameState.PlayerTwoId,
                IsFinished = gameState.IsGameOver,
                JsonGameState = SimpleEngine.Classes.Game.GameStateSerializer.Serialize(gameState)
            };
        }

        // TODO: maybe work with gameState only ?
        public static GameModel EngineGameToGameModel(SimpleEngine.Classes.Game.Game gameEngine)
        {
            var gameState = gameEngine.CurrentGameState;

            var gameSize = gameState.Board.Size;
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
                        Value = (int)gameState.Board.Cells[rowIndex, columnIndex]
                    };
                    cells.Add(cell);
                }
                rows.Add(new CellRow
                {
                    Cells = cells,
                    RowIndex = rowIndex
                });
            }

            return new GameModel
            {
                ActivePlayerId = gameState.ActivePlayerId,
                PlayerOneId = gameState.PlayerOneId,
                PlayerTwoId = gameState.PlayerTwoId,
                IsFinished = gameState.IsGameOver,
                Rows = rows
            };
        }

        //public static GameModel DbGameToGameModel(Game dbGame)
        //{
        //    GameModel gameModel = JsonConvert.DeserializeObject<GameModel>(dbGame.Json);
        //    //TODO: Change to inner temporary id maybe
        //    gameModel.GameId = dbGame.Id;
        //    gameModel.ActivePlayerId = dbGame.ActivePlayerId;
        //    gameModel.PlayerOneId = dbGame.PlayerOneId;
        //    gameModel.PlayerTwoId = dbGame.PlayerTwoId.Value;
        //    gameModel.IsFinished = dbGame.IsFinished;
        //    return gameModel;
        //}

        //public static Game GameModelToDbGame(GameModel gameModel)
        //{
        //    return new Game
        //    {
        //        Id = gameModel.GameId,
        //        PlayerOneId = gameModel.PlayerOneId,
        //        PlayerTwoId = gameModel.PlayerTwoId,
        //        ActivePlayerId = gameModel.ActivePlayerId,
        //        IsFinished = gameModel.IsFinished,
        //        Json = JsonConvert.SerializeObject(gameModel)
        //    };
        //}
    }

    //// TODO: maybe instead of half of them use GameStateSerializer
    //public class CustomSpecificParser
    //{
    //    public static SimpleEngine.Classes.Game.Game DbGameToEngineGame(Game dbGame)
    //    {
    //        GameModel gameModel = CustomSpecificParser.DbGameToGameModel(dbGame);

    //        SimpleEngine.Classes.Game.Game engineGame = new SimpleEngine.Classes.Game.Game(gameModel.PlayerOneId, gameModel.PlayerTwoId);
    //        Int32 gameSize = gameModel.Rows.Count;
    //        SimpleEngine.Classes.Board gameBoard = new Board(gameSize);
    //        for (var rowIndex = 0; rowIndex < gameSize; rowIndex++)
    //        {
    //            for (var columnIndex = 0; columnIndex < gameSize; columnIndex++)
    //            {
    //                SimpleEngine.Classes.CellType newCellTypeValue = (SimpleEngine.Classes.CellType)Enum.Parse(typeof(SimpleEngine.Classes.CellType), gameModel.Rows[rowIndex].Cells[columnIndex].Value.ToString());
    //                gameBoard.Cells[rowIndex, columnIndex] = newCellTypeValue;
    //            }
    //        }

    //        //TODO: save into DB scheme suitable game structure without json or add surrendering and skiping status for GameModel and take from there
    //        //BUG: commented
    //        //engineGame.LoadState(dbGame.ActivePlayerId, gameBoard);
            

    //        return engineGame;
    //    }

    //    public static Game EngineGameToDbGame(SimpleEngine.Classes.Game.Game gameEngine)
    //    {
    //        var gameSize = gameEngine.CurrentGameState.Board.Size;
    //        var rows = new List<CellRow>(gameSize);

    //        for (var rowIndex = 0; rowIndex < gameSize; rowIndex++)
    //        {
    //            var cells = new List<Cell>(gameSize);
    //            for (var columnIndex = 0; columnIndex < gameSize; columnIndex++)
    //            {
    //                var cell = new Cell
    //                {
    //                    RowIndex = rowIndex,
    //                    ColumnIndex = columnIndex,
    //                    Value = (int)gameEngine.CurrentGameState.Board.Cells[rowIndex, columnIndex]
    //                };
    //                cells.Add(cell);
    //            }
    //            rows.Add(new CellRow
    //            {
    //                Cells = cells,
    //                RowIndex = rowIndex
    //            });
    //        }

    //        GameModel gameModel = new GameModel
    //        {
    //            ActivePlayerId = gameEngine.CurrentGameState.ActivePlayerId,
    //            PlayerOneId = gameEngine.CurrentGameState.PlayerOneId,
    //            PlayerTwoId = gameEngine.CurrentGameState.PlayerTwoId,
    //            IsFinished = gameEngine.CurrentGameState.IsGameOver,
    //            Rows = rows
    //        };

    //        return CustomSpecificParser.GameModelToDbGame(gameModel);
    //    }

    //    public static GameModel DbGameToGameModel(Game dbGame)
    //    {
    //        GameModel gameModel = JsonConvert.DeserializeObject<GameModel>(dbGame.Json);
    //        //TODO: Change to inner temporary id maybe
    //        gameModel.GameId = dbGame.Id;
    //        gameModel.ActivePlayerId = dbGame.ActivePlayerId;
    //        gameModel.PlayerOneId = dbGame.PlayerOneId;
    //        gameModel.PlayerTwoId = dbGame.PlayerTwoId.Value;
    //        gameModel.IsFinished = dbGame.IsFinished;
    //        return gameModel;
    //    }

    //    public static Game GameModelToDbGame(GameModel gameModel)
    //    {
    //        return new Game
    //        {
    //            Id = gameModel.GameId,
    //            PlayerOneId = gameModel.PlayerOneId,
    //            PlayerTwoId = gameModel.PlayerTwoId,
    //            ActivePlayerId = gameModel.ActivePlayerId,
    //            IsFinished = gameModel.IsFinished,
    //            Json = JsonConvert.SerializeObject(gameModel)
    //        };
    //    }
    //}
}