using System;
using System.Linq;
using System.Web.Mvc;
using MvcApp.EfDataModels;
using MvcApp.Models;
using MvcApp.UoW;
using Newtonsoft.Json;
using NotFoundMvc;

namespace MvcApp.Controllers
{
    //TODO: validation for all
    public class GameController : Controller
    {
        //
        // GET: /Game/

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PlayerId = Request.Cookies.Get("playerId").Value;
            return View();
        }

        // Output detail like links, available games and other HowTo.
        [HttpGet]
        public ActionResult Details()
        {
            throw new NotImplementedException();
        }

        // way to get more universal
        [HttpPost]
        public ActionResult OpenGame(Int32 playerId)
        {
            var uow = new UnitOfWork(new GameModelContainer());
            GameService service = new GameService(uow);

            var newGameId = service.OpenNewGame(playerId);

            return Json(new { NewGameId = newGameId });
        }

        [HttpPost]
        public ActionResult CloseGame(Int32 gameId, Int32 playerId)
        {
            var uow = new UnitOfWork(new GameModelContainer());
            GameService service = new GameService(uow);

            service.CloseGame(gameId, playerId);

            return Json("OK");
        }

        [HttpPost]
        public ActionResult GetGame(Int32 gameId)
        {
            var uow = new UnitOfWork(new GameModelContainer());
            GameService service = new GameService(uow);

            var game = service.Get(gameId);

            var json = JsonConvert.SerializeObject(game);

            ViewBag.PlayerId = Request.Cookies.Get("playerId").Value;
            return Json(json);
        }

        //TODO: Ajax only attribute
        public ActionResult Turn(TurnModel model)
        {
            if (ModelState.IsValid && Request.Cookies.AllKeys.Contains("playerId"))
            {
                String playerId = Request.Cookies.Get("playerId").Value;
                if (!String.IsNullOrWhiteSpace(playerId))
                {
                    var uow = new UnitOfWork(new GameModelContainer());
                    GameService service = new GameService(uow);

                    service.Turn(model, Int32.Parse(playerId));
                    return Json("OK");
                }
            }

            return new NotFoundViewResult();
        }

        public ActionResult SkipTurn(Int32 gameId)
        {
            if (ModelState.IsValid && Request.Cookies.AllKeys.Contains("playerId"))
            {
                String playerId = Request.Cookies.Get("playerId").Value;
                if (!String.IsNullOrWhiteSpace(playerId))
                {
                    var uow = new UnitOfWork(new GameModelContainer());
                    GameService service = new GameService(uow);

                    service.SkipTurn(gameId, Int32.Parse(playerId));

                    return Json("OK");
                }
            }

            return new NotFoundViewResult();
        }

        public ActionResult Surrender(Int32 gameId)
        {
            if (ModelState.IsValid && Request.Cookies.AllKeys.Contains("playerId"))
            {
                String playerId = Request.Cookies.Get("playerId").Value;
                if (!String.IsNullOrWhiteSpace(playerId))
                {
                    var uow = new UnitOfWork(new GameModelContainer());
                    GameService service = new GameService(uow);

                    service.Surrender(gameId, Int32.Parse(playerId));
                    return Json("OK");
                }
            }

            return new NotFoundViewResult();
        }
    }

    // TODO: separate somewhere
    public static class GameJsonParser
    {
        public static String ToJsonString(Game game)
        {
            return JsonConvert.SerializeObject(game);
        }

        public static Game FromJsonString(String gameStr)
        {
            return JsonConvert.DeserializeObject<Game>(gameStr);
        }
    }
}
