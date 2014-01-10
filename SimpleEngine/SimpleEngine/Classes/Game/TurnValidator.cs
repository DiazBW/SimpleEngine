using System;
using SimpleEngine.Exceptions;
using SimpleEngine.Interfaces;

namespace SimpleEngine.Classes.Game
{
    public partial class Game : IGame
    {
        private class TurnValidator
        {
            private readonly Game _game;

            // MaybeTake GameState instead of game
            public TurnValidator(Game game)
            {
                _game = game;
            }

            /// <exception cref="SuicideException">Trying to turn out of board.</exception>
            /// <exception cref="TurnOutOfRangeException">Trying to turn on not-empty cell.</exception>
            /// <exception cref="TurnToBusyCellException">Player is trying to turn that brings board to same state as at previous turn.</exception>
            /// <exception cref="RepeatBoardStateException">Trying to turn that leads to suicide without attack.</exception>
            public void Validate(GameTurnStruct turn, String previousBoardStateHash)
            {
                if (!IsTurnIntoBoard(turn, _game.CurrentGameState.Board.Size))
                {
                    throw new TurnOutOfRangeException(turn);
                }

                if (!IsCellFree(turn, _game.CurrentGameState.Board))
                {
                    throw new TurnToBusyCellException(turn);
                }

                if (IsSuicide(turn, _game.CurrentGameState.ActivePlayerId, _game.CurrentGameState.Board))
                {
                    throw new SuicideException(turn);
                }

                if (IsBoardStateRepeated(turn, _game.CurrentGameState.ActivePlayerId, previousBoardStateHash))
                {
                    throw new RepeatBoardStateException(turn, previousBoardStateHash);
                }
            }

            private Boolean IsTurnIntoBoard(GameTurnStruct turn, Int32 boardSize)
            {
                return (turn.RowIndex >= 0 && turn.RowIndex < boardSize)
                       && (turn.ColumnIndex >= 0 && turn.ColumnIndex < boardSize);
            }

            private Boolean IsCellFree(GameTurnStruct turn, Board board)
            {
                return board.Cells[turn.RowIndex, turn.ColumnIndex] == CellType.Empty;
            }

            private Boolean IsSuicide(GameTurnStruct turn, Int32 playerId, Board board)
            {
                Func<bool> suicideCheck = () =>
                {
                    var shapesForRemove = _game.GetShapesWithoutBreath();
                    return shapesForRemove.Count == 1 && shapesForRemove[0].Contains(turn.RowIndex, turn.ColumnIndex);
                };

                return _game.EmulateTurnAndCheck(turn, playerId, suicideCheck);
            }

            private Boolean IsBoardStateRepeated(GameTurnStruct turn, Int32 playerId, String previousBoardStateHash)
            {
                Func<bool> boardRepeatCheck = () =>
                {
                    var newBoardStateHash = _game.CurrentGameState.Board.GetCustomHash();
                    return newBoardStateHash == previousBoardStateHash;
                };

                return _game.EmulateTurnAndCheck(turn, playerId, boardRepeatCheck);
            }
        }
    }
}