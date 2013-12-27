using System;
using System.Linq;
using MvcApp.EfDataModels;

namespace MvcApp.UoW
{
    public interface IGameRepository
    {
        Game Get(Int32 id);
        IQueryable<Game> GetAll();
        void SaveNew(Game game);
        void Update(Game game);
    }
}