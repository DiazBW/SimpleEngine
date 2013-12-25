using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcApp.EfDataModels;
using MvcApp.Models;
using MvcApp.Models.User;
using MvcApp.UoW;
using Newtonsoft.Json;
using Game = SimpleEngine.Classes.Game.Game;

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
            service.SaveGame(model);

            return Json("OK");
        }

        public ActionResult AjaxLoad()
        {
            var uow = new UnitOfWork(new GameModelContainer());
            GameService service = new GameService(uow);
            var gameModel = service.Get(id : 1);
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

        [HttpGet]
        public ActionResult CreateUser()
        {
            var model = new CreateUserModel
            {
                Username = "name",
                Password = "pass",
                Email = "mail",
                Msg = "message"
            };
            return View("CreateUserModel", model);
        }

        [HttpPost]
        public ActionResult CreateUser(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                //EfDataModels.Game game = new EfDataModels.Game();
                //game.ActivePlayerId = model.


                // Default UserStore constructor uses the default connection string named: DefaultConnection
                //var userStore = new UserStore<IdentityUser>();
                //var manager = new UserManager<IdentityUser>(userStore);

                //var user = new IdentityUser() { UserName = model.Username };
                //IdentityResult result = manager.Create(user, model.Password );

                //if (result.Succeeded)
                //{
                //    model.Msg = string.Format("User {0} was created successfully!", user.UserName);
                //}
                //else
                //{
                //    model.Msg = result.Errors.FirstOrDefault();
                //}
            }
            return View("CreateUserModel", model);
        }


        //var settings = new JsonSerializerSettings()
        //{
        //    TypeNameHandling = TypeNameHandling.Objects
        //};

        //var domExercise = JsonConvert.DeserializeObject<DomExercise>(exercise.EditorViewJson, settings);
    }
}


//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using System;
//using System.Linq;

//namespace WebFormsIdentity
//{
//   public partial class Register : System.Web.UI.Page
//   {
//      protected void CreateUser_Click(object sender, EventArgs e)
//      {
//         // Default UserStore constructor uses the default connection string named: DefaultConnection
//         var userStore = new UserStore<IdentityUser>();
//         var manager = new UserManager<IdentityUser>(userStore);

//         var user = new IdentityUser() { UserName = UserName.Text };
//         IdentityResult result = manager.Create(user, Password.Text);

//         if (result.Succeeded)
//         {
//            StatusMessage.Text = string.Format("User {0} was created successfully!", user.UserName);
//         }
//         else
//         {
//            StatusMessage.Text = result.Errors.FirstOrDefault();
//         }
//      }
//   }
//}

// ------------------------------------------------------------------------------------

//public partial class Register : System.Web.UI.Page
//{
//    protected void CreateUser_Click(object sender, EventArgs e)
//    {
//        // Default UserStore constructor uses the default connection string named: DefaultConnection
//        var userStore = new UserStore<IdentityUser>();
//        var manager = new UserManager<IdentityUser>(userStore);
//        var user = new IdentityUser() { UserName = UserName.Text };

//        IdentityResult result = manager.Create(user, Password.Text);

//        if (result.Succeeded)
//        {
//            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
//            var userIdentity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
//            authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
//            Response.Redirect("~/Login.aspx");
//        }
//        else
//        {
//            StatusMessage.Text = result.Errors.FirstOrDefault();
//        }
//    }
//}

// ----------------------------------------------------------------------------------------

//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.Owin.Security;
//using System;
//using System.Web;
//using System.Web.UI.WebControls;

//namespace WebFormsIdentity
//{
//   public partial class Login : System.Web.UI.Page
//   {
//      protected void Page_Load(object sender, EventArgs e)
//      {
//         if (!IsPostBack)
//         {
//            if (User.Identity.IsAuthenticated)
//            {
//               StatusText.Text = string.Format("Hello {0}!!", User.Identity.GetUserName());
//               LoginStatus.Visible = true;
//               LogoutButton.Visible = true;
//            }
//            else
//            {
//               LoginForm.Visible = true;
//            }
//         }
//      }

//      protected void SignIn(object sender, EventArgs e)
//      {
//         var userStore = new UserStore<IdentityUser>();
//         var userManager = new UserManager<IdentityUser>(userStore);
//         var user = userManager.Find(UserName.Text, Password.Text);

//         if (user != null)
//         {
//            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
//            var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

//            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);
//            Response.Redirect("~/Login.aspx");
//         }
//         else
//         {
//            StatusText.Text = "Invalid username or password.";
//            LoginStatus.Visible = true;
//         }
//      }

//      protected void SignOut(object sender, EventArgs e)
//      {
//         var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
//         authenticationManager.SignOut();
//         Response.Redirect("~/Login.aspx");
//      }
//   }
//}