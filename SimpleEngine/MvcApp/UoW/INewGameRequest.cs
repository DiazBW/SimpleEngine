using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApp.EfDataModels;

namespace MvcApp.UoW
{
    public interface INewGameRequestRepository
    {
        NewGameRequest Get(Int32 id);
        IQueryable<NewGameRequest> GetAll();
        void SaveNew(NewGameRequest game);
        void Update(NewGameRequest game);
        void Remove(Int32 id);
    }
}