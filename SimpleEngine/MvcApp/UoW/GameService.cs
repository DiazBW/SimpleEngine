using System;
using MvcApp.EfDataModels;
using MvcApp.Models;
using Newtonsoft.Json;

namespace MvcApp.UoW
{
    public class GameService : BaseService
    {
        public GameService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Game Get(Int32 id)
        {
            return GameRepository.Get(id);
        }

        public void SaveGame(GameModel gameModel)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(gameModel);

            var gameDataObject = new Game
            {
                PlayerOneId = gameModel.PlayerOneId,
                PlayerTwoId = gameModel.PlayerTwoId,
                ActivePlayerId = gameModel.ActivePlayerId,
                Json = json
            };

            _unitOfWork.GameRepository.SaveNew(gameDataObject);
            _unitOfWork.Save();
        }
    }
}
