using System;
using System.Web.Mvc;
using MvcApp.EfDataModels;
using MvcApp.Models;
using MvcApp.UoW;
using Newtonsoft.Json;

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
            throw new NotImplementedException();
        }

        // Output detail like links, available games and other HowTo.
        [HttpGet]
        public ActionResult Details()
        {
            throw new NotImplementedException();
        }

        // way to get more universal
        [HttpPost]
        public ActionResult CreateGame(GameCreateModel gameCreateModel)
        {
            var uow = new UnitOfWork(new GameModelContainer());
            GameService service = new GameService(uow);
            
            var newGameId = service.OpenNewGame(playerId);

            return Json(new { NewGameId = newGameId });
        }

        [HttpPost]
        public ActionResult GetGame(GetGameModel getGameModel)
        {
            var uow = new UnitOfWork(new GameModelContainer());
            GameService service = new GameService(uow);

            var game = service.Get(getGameModel.GameId);
            var jsonGame = GameJsonParser.ToJsonString(game);

            return Json(jsonGame);
        }

        [HttpPost]
        public ActionResult PlayerTurn(TurnModel turnModel)
        {
            var uow = new UnitOfWork(new GameModelContainer());
            GameService service = new GameService(uow);

            var gameAfterTurn = service.Turn(turnModel.GameId, turnModel.PlayerId, turnModel.RowIndex, turnModel.ColumnIndex);
            var jsonGame = GameJsonParser.ToJsonString(gameAfterTurn);

            // maybe show only success/failed version for traffic minimization
            return Json(jsonGame);
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
