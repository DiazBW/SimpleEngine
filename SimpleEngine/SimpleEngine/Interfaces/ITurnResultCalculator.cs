using SimpleEngine.Classes;

namespace SimpleEngine.Interfaces
{
    internal interface ITurnResultCalculator
    {
        void CalculateNewBoardState(ref Board board);
    }
}