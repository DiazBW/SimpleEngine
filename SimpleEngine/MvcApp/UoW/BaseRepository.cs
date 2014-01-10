using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace MvcApp.UoW
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly DbContext _dataContext;

        protected DbSet<TEntity> Table
        {
            get { return _dataContext.Set<TEntity>(); }
        }

        protected BaseRepository(DbContext dataContext)
            : base()
        {
            if (dataContext == null)
            {
                throw new ArgumentNullException("dataContext");
            }
            _dataContext = dataContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            return Table;
        }

        public void InsertOnCommit(TEntity entity)
        {
            Table.Add(entity);
        }

        // TODO: add delete by id!
        public void DeleteOnCommit(TEntity entity)
        {
            Table.Remove(entity);
        }

        public void AddOrUpdate(TEntity entity)
        {
            Table.AddOrUpdate(entity);
        }
    }
}