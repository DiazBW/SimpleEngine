using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApp.Models;
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
    }
}
