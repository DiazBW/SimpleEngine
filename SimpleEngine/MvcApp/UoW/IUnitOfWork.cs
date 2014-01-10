using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp.UoW
{
    public interface IUnitOfWork
    {
        void Save();

        #region Repositories
        IGameRepository GameRepository { get; }
        INewGameRequestRepository NewGameRequestRepository { get; }
        #endregion Repositories
    }
}