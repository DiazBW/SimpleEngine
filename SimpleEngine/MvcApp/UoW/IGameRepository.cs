using System;
using MvcApp.EfDataModels;

namespace MvcApp.UoW
{
    public interface IGameRepository
    {
        Game Get(Int32 id);
        void SaveNew(Game game);
        void Update(Game game);
    }
}