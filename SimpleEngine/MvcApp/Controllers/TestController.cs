using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MvcApp.Models;
using Newtonsoft.Json;
using SimpleEngine.Classes.Game;

namespace MvcApp.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        private static Game TheGame;

        public ActionResult Index()
        {
            //return RedirectToAction("UserTest", new { userId = 12 } );
            //return RedirectToAction("StartBoard", new { userOneId = 12, userTwoId = 13 });
            return RedirectToAction("ShowBoard");
        }

        public ActionResult UserTest(Int32 userId)
        {
            var model = new TestModel();
            model.UserID = userId;
            return View(model);
        }

        //public ActionResult StartBoard(Int32 userOneId, Int32 userTwoId)
        //{
        //    TheGame = new Game(userOneId, userTwoId);

        //    //return View(BoardModel);
        //    return RedirectToAction("ShowBoard");
        //}

        public ActionResult ShowBoard()
        {
            TheGame = new Game(11, 13);
            var model = new BoardModel(TheGame);
            return View(model);
        }

        public JsonResult AjaxSave()
        {
            return Json("");
        }

        public ActionResult AjaxLoad()
        {
            var gameModel = GameModel.GetFake(10);
            var gameModelDe = new GameModel();

            JsonSerializerSettings settings = new JsonSerializerSettings();

            var jsonModel = JsonConvert.SerializeObject(gameModel);
            gameModelDe = JsonConvert.DeserializeObject<GameModel>(jsonModel);
            var res = Json(jsonModel);
            return res;

            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //var writer = new JsonTextWriter(sw) { Formatting = Formatting.Indented };
            //var serializer = JsonSerializer.Create(new JsonSerializerSettings());
            //serializer.Serialize(writer, gameModel);
            //var res = Json(sb.ToString());
            //return res;
        }


        //var settings = new JsonSerializerSettings()
        //{
        //    TypeNameHandling = TypeNameHandling.Objects
        //};

        //var domExercise = JsonConvert.DeserializeObject<DomExercise>(exercise.EditorViewJson, settings);
    }
}
