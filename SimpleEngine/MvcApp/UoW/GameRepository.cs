using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MvcApp.EfDataModels;
using MvcApp.Models;
using Newtonsoft.Json;

namespace MvcApp.UoW
{
    public class GameRepository : BaseRepository<Game>, IGameRepository
    {
        public GameRepository(DbContext context) : base(context)
        {
        }

        public Game Get(Int32 id)
        {
            return GetAll().FirstOrDefault(g => g.Id == id);
        }

        public void SaveNew(Game game)
        {
            InsertOnCommit(game);
        }
    }
}