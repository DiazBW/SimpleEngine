using System;
using System.Collections.Generic;

namespace SimpleEngine.Interfaces
{
    public interface IGame
    {
        bool IsPlayerInGame(Int32 playerId);
        bool IsGameOver();

        Int32 GetActivePlayerId();
        Int32 GetActivePlayerCellType();
        Int32 GetWinPlayerId();
        List<String> GetBoardTextRepresentation();
        
        Int32 Turn(Int32 rowIndex, Int32 columnIndex, Int32 playerId);

        void ClearBoard();
    }
}