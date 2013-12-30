using System.Web.Mvc;
using MvcApp.EfDataModels;
using MvcApp.Models;
using MvcApp.UoW;
using Newtonsoft.Json;

namespace MvcApp.Controllers
{
    public class TestController : Controller
    {
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
