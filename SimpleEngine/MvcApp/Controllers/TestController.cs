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
            TheGame = new Game(1, 2);
            var model = new BoardModel(TheGame);
            return View(model);
        }

        public JsonResult AjaxSave(GameModel model)
        {
            //EfDataModels.GameModelContainer container = new GameModelContainer();
            
            //EfDataModels.Game game = new EfDataModels.Game();
            //game.ActivePlayerId = model.ActivePlayerId;
            //game.PlayerOneId = model.PlayerOneId;
            //game.PlayerTwoId = model.PlayerTwoId;

            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //var jsonModel = JsonConvert.SerializeObject(model);
            //game.Json = jsonModel;
            

            //container.SaveChanges();

            var uow = new UnitOfWork(new GameModelContainer());
            GameService service = new GameService(uow);
            //service.UpdateGame(model,);
            service.UpdateGame(model, 1);

            return Json("OK");
        }

        //TODO: Ajax only attribute
        public ActionResult AjaxLoad()
        {
            var uow = new UnitOfWork(new GameModelContainer());
            GameService service = new GameService(uow);
            MvcApp.EfDataModels.Game gameModel = service.Get(id : 1);
            var res = Json(gameModel.Json);
            return res;

            //var gameModel = GameModel.GetFake(10);
            //var gameModelDe = new GameModel();

            //JsonSerializerSettings settings = new JsonSerializerSettings();

            //var jsonModel = JsonConvert.SerializeObject(gameModel);
            //gameModelDe = JsonConvert.DeserializeObject<GameModel>(jsonModel);
            //var res = Json(jsonModel);
            //return res;

            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //var writer = new JsonTextWriter(sw) { Formatting = Formatting.Indented };
            //var serializer = JsonSerializer.Create(new JsonSerializerSettings());
            //serializer.Serialize(writer, gameModel);
            //var res = Json(sb.ToString());
            //return res;
        }

        //TODO: Ajax only attribute
        //public ActionResult AjaxTurn(TurnModel model)
        //{
        //    var uow = new UnitOfWork(new GameModelContainer());
        //    GameService service = new GameService(uow);
            
        //    if (ModelState.IsValid && Request.Cookies.AllKeys.Contains("playerId"))
        //    {
        //        String playerId = Request.Cookies.Get("playerId").Value;
        //        if (!String.IsNullOrWhiteSpace(playerId))
        //        {
        //            var actualGame = service.GetActualGameForPlayer(playerId);
        //            if (actualGame != null)
        //            {
        //                service.TurnV2(model);
        //                return RedirectToAction("AjaxLoad");
        //            }
        //        }
        //    }

        //    return new NotFoundViewResult();
        //}

        //[HttpGet]
        //public ActionResult CreateUser()
        //{
        //    var model = new CreateUserModel
        //    {
        //        Username = "name",
        //        Password = "password",
        //        Email = "mail",
        //        Msg = "message"
        //    };
        //    return View("CreateUserModel", model);
        //}

        //[HttpGet]
        //public ActionResult SignIn()
        //{
        //    var model = new SignInModel
        //    {
        //        Login = "name",
        //        Password = "password"
        //    };
        //    return View("SignIn", model);
        //}

        //[HttpPost]
        //public ActionResult SignIn(SignInModel model)
        //{
        //    var userStore = new UserStore<IdentityUser>();
        //    var userManager = new UserManager<IdentityUser>(userStore);

        //    var user = userManager.Find(model.Login, model.Password);

        //    if (user != null)
        //    {
        //        //var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
        //        //var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
        //        //authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);

        //        //var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
        //        //HttpContext..
        //        //var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
        //        //authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);

        //        return RedirectToAction("ShowBoard");
        //    }
        //    else
        //    {
        //        ViewBag.Msg = "Invalid username or password.";
        //    }

        //    return View("SignIn", model);
        //}

        ////protected void SignOut(object sender, EventArgs e)
        ////{
        ////    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
        ////    authenticationManager.SignOut();
        ////    Response.Redirect("~/Login.aspx");
        ////}

        ////[HttpPost]
        ////public ActionResult CreateUser(CreateUserModel model)
        ////{
        ////    if (ModelState.IsValid)
        ////    {
        ////        // Default UserStore constructor uses the default connection string named: DefaultConnection
        ////        var userStore = new UserStore<IdentityUser>();
        ////        var manager = new UserManager<IdentityUser>(userStore);
        ////        var user = new IdentityUser() { UserName = model.Username };
                
        ////        IdentityResult result = manager.Create(user, model.Password );

        ////        if (result.Succeeded)
        ////        {
        ////            model.Msg = string.Format("User {0} was created successfully!", user.UserName);
        ////        }
        ////        else
        ////        {
        ////            model.Msg = result.Errors.FirstOrDefault();
        ////        }
        ////    }
        ////    return View("CreateUserModel", model);
        ////}
    }
}
