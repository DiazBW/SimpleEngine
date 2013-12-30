using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcApp.EfDataModels;
using MvcApp.Models;
using MvcApp.Models.User;
using MvcApp.UoW;
using Newtonsoft.Json;
using NotFoundMvc;
using Game = SimpleEngine.Classes.Game.Game;

namespace MvcApp.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        private static Game TheGame;

        [HttpPost]
        public ActionResult RequestCheck()
        {
            return RedirectToAction("ShowBoard");
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult AjaxSave(GameModel model)
        {
            var uow = new UnitOfWork(new GameModelContainer());
            GameService service = new GameService(uow);
            service.UpdateGame(model, 1);

            return Json("OK");
        }

        //TODO: Ajax only attribute
        public ActionResult AjaxLoad()
        {
            var uow = new UnitOfWork(new GameModelContainer());
            GameService service = new GameService(uow);

            var gameModel = service.Get(id: 1);
            var json = JsonConvert.SerializeObject(gameModel);
            return Json(json);
        }
    }
}
