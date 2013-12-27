using System;
using SimpleEngine.Classes;

namespace SimpleEngine.Interfaces
{
    public interface IGame
    {
        Int32 ActivePlayerId { get; }
        CellType ActiveCellType { get; }
        Boolean IsGameOver { get; }

        Board Board { get; }

        void PlayerSkipTurn(Int32 playerId);
        void Turn(Int32 rowIndex, Int32 columnIndex, Int32 playerId);
        void LoadState(Int32 activePlayerId, Board board);
    }
}