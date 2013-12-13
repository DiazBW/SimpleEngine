using System;
using SimpleEngine.Exceptions;

namespace SimpleEngine.Classes.Game
{
    public partial class Game
    {
        private class TurnValidator
        {
            private readonly Game _game;

            public TurnValidator(Game game)
            {
                _game = game;
            }

            /// <exception cref="SuicideException">Trying to turn out of board.</exception>
            /// <exception cref="TurnOutOfRangeException">Trying to turn on not-empty cell.</exception>
            /// <exception cref="TurnToBusyCellException">Player is trying to turn that brings board to same state as at previous turn.</exception>
            /// <exception cref="RepeatBoardStateException">Trying to turn that leads to suicide without attack.</exception>
            public void Validate(int rowIndex, int columnIndex, CellType newCellValue, Board board,
                String previousBoardStateHash)
            {
                var turn = new GameTurnStruct()
                {
                    RowIndex = rowIndex,
                    ColumnIndex = columnIndex,
                    Value = newCellValue
                };

                if (!IsTurnIntoBoard(turn, board.Size))
                {
                    throw new TurnOutOfRangeException(rowIndex, columnIndex, newCellValue);
                }

                if (!IsCellFree(turn, board))
                {
                    throw new TurnToBusyCellException(rowIndex, columnIndex, newCellValue);
                }

                if (IsSuicide(turn, board))
                {
                    throw new SuicideException(rowIndex, columnIndex, newCellValue);
                }

                if (IsBoardStateRepeated(turn, previousBoardStateHash))
                {
                    throw new RepeatBoardStateException(rowIndex, columnIndex, newCellValue, previousBoardStateHash);
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

            private Boolean IsSuicide(GameTurnStruct turn, Board board)
            {
                // Copy state
                var shapesBeforeStep = _game.DeepCopy(_game.Shapes);

                // Calculate new turn on copy
                var newShapeId = _game.CreateNewShape(turn.RowIndex, turn.ColumnIndex);
                _game.MergeNewShapeIfPossible(newShapeId);
                _game.RemoveWithoutBreathAlt(newShapeId);

                // Get Board Hash
                //var board = GetBoard();
                //var newBoardStateHash = board.GetCustomHash();
                var shapesForRemove = _game.GetShapesWithoutBreath();

                // return state
                _game.Shapes = shapesBeforeStep;

                return shapesForRemove.Count == 1 && shapesForRemove[0].Contains(turn.RowIndex, turn.ColumnIndex);

                //return newBoardStateHash == previousBoardStateHash;

                //var shapesForRemove = GetShapesWithoutBreath();
                //return shapesForRemove.Count == 1 && shapesForRemove[0].Contains(turn.RowIndex, turn.ColumnIndex);
            }

            private Boolean IsBoardStateRepeated(GameTurnStruct turn, String previousBoardStateHash)
            {
                // Copy state
                var shapesBeforeStep = _game.DeepCopy(_game.Shapes);

                // Calculate new turn on copy
                var newShapeId = _game.CreateNewShape(turn.RowIndex, turn.ColumnIndex);
                _game.MergeNewShapeIfPossible(newShapeId);
                _game.RemoveWithoutBreathAlt(newShapeId);

                // Get Board Hash
                var board = _game.GetBoard();
                var newBoardStateHash = board.GetCustomHash();

                // return state
                _game.Shapes = shapesBeforeStep;
                return newBoardStateHash == previousBoardStateHash;
            }
        }
    }
}