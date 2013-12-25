using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp.UoW
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> GetAll();
        void InsertOnCommit(TEntity entity);
        void DeleteOnCommit(TEntity entity);
        void AddOrUpdate(TEntity entity);
    }
}