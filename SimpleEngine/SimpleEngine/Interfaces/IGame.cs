using System;
using System.Collections.Generic;
using SimpleEngine.Classes;

namespace SimpleEngine.Interfaces
{
    public interface IGame
    {
        bool IsPlayerInGame(Int32 playerId);

        Int32 GetWinPlayerId();
        List<String> GetBoardTextRepresentation();
        CellType GetCellValue(Int32 rowIndex, Int32 columnIndex);
        
        void Turn(Int32 rowIndex, Int32 columnIndex, Int32 playerId);
        void DevTurn(Int32 rowIndex, Int32 columnIndex);

        void ClearBoard();
    }
}