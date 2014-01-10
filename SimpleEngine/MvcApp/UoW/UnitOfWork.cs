using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApp.EfDataModels;

namespace MvcApp.UoW
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly GameModelContainer _context;
        private bool _disposed = false;

        #region RepositoriesPrivate
        private IGameRepository _gameRepository;
        private INewGameRequestRepository _newGameRequestRepository;
        #endregion RepositoriesPrivate

        public UnitOfWork(GameModelContainer context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        #region Repositories
        public IGameRepository GameRepository
        {
            get
            {
                return _gameRepository ?? (_gameRepository = new GameRepository(_context));
            }
        }

        public INewGameRequestRepository NewGameRequestRepository
        {
            get
            {
                return _newGameRequestRepository ?? (_newGameRequestRepository = new NewGameRequestRepository(_context));
            }
        }
        #endregion Repositories

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    ((IDisposable)_context).Dispose();
                }
            }
            this._disposed = true;
        }
    }
}