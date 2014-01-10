using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MvcApp.EfDataModels;

namespace MvcApp.UoW
{
    public class NewGameRequestRepository : BaseRepository<NewGameRequest>, INewGameRequestRepository
    {
        public NewGameRequestRepository(DbContext context)
            : base(context)
        {
        }

        public NewGameRequest Get(Int32 id)
        {
            return GetAll().FirstOrDefault(g => g.Id == id);
        }

        public void SaveNew(NewGameRequest game)
        {
            InsertOnCommit(game);
        }

        public void Update(NewGameRequest game)
        {
            AddOrUpdate(game);
        }

        public void Remove(Int32 id)
        {
            var entity = GetAll().FirstOrDefault(g => g.Id == id);
            DeleteOnCommit(entity);
        }
    }
}