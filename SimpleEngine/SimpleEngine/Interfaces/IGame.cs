using System;
using SimpleEngine.Classes.Game;

namespace SimpleEngine.Interfaces
{
    public interface IGame
    {
        GameState CurrentGameState { get; }

        void SkipTurn(Int32 playerId);
        void Surrender(Int32 playerId);
        void Turn(Int32 rowIndex, Int32 columnIndex, Int32 playerId);
        void LoadState(GameState gameState);
    }
}