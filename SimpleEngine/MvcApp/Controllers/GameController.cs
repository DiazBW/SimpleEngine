using System;
using System.Linq;
using System.Collections.Generic;
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

            var game = service.Get(gameId);
            // TODO: rewrite
            var gameModelEmpty = GameModel.GetFake(19, game.PlayerOneId, game.PlayerTwoId.Value);
            service.UpdateGame(gameModelEmpty, game.Id);

            return Json("OK");
        }

        [HttpPost]
        //public ActionResult GetGame(GetGameModel getGameModel)
        public ActionResult GetGame(Int32 gameId)
        {
            var uow = new UnitOfWork(new GameModelContainer());
            GameService service = new GameService(uow);

            var game = service.Get(gameId);
            //GameModel gameModel = CreateGameModel(game);

            //var jsonGame = GameJsonParser.ToJsonString(gameModel);
            return Json(game.Json);
        }

        //[HttpPost]
        //public ActionResult PlayerTurn(TurnModel turnModel)
        //{
        //    // get player id from controller 
        //    var uow = new UnitOfWork(new GameModelContainer());
        //    GameService service = new GameService(uow);

        //    var gameAfterTurn = service.Turn(turnModel);
        //    var jsonGame = GameJsonParser.ToJsonString(gameAfterTurn);

        //    // maybe show only success/failed version for traffic minimization
        //    return Json(jsonGame);
        //}

        //TODO: Ajax only attribute
        public ActionResult Turn(TurnModel model)
        {
            var uow = new UnitOfWork(new GameModelContainer());
            GameService service = new GameService(uow);

            if (ModelState.IsValid && Request.Cookies.AllKeys.Contains("playerId"))
            {
                String playerId = Request.Cookies.Get("playerId").Value;
                if (!String.IsNullOrWhiteSpace(playerId))
                {
                    var game = service.Turn(model, Int32.Parse(playerId));
                    return Json(game.Json);
                }
            }

            return new NotFoundViewResult();
        }

        // player surrender 
        // player skip 
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
