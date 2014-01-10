using System;
using Newtonsoft.Json;

namespace SimpleEngine.Classes.Game
{
    //TODO: add serialization
    public class GameState
    {
        public Int32 ActivePlayerId;
        public Int32 PlayerOneId;
        public Int32 PlayerTwoId;
        public Boolean IsPlayerOneSkip;
        public Boolean IsPlayerTwoSkip;
        public Nullable<Int32> SurrenderPlayerId;
        public Board Board;

        [JsonIgnore]
        public CellType ActiveCellType
        {
            get { return ActivePlayerId == PlayerOneId ? CellType.Black : CellType.White; }
        }
        [JsonIgnore]
        public bool IsGameOver
        {
            get { return SurrenderPlayerId.HasValue || (IsPlayerOneSkip && IsPlayerTwoSkip); }
        }

        public static GameState GetDeepCopy(GameState source)
        {
            return new GameState
            {
                ActivePlayerId = source.ActivePlayerId,
                PlayerOneId = source.PlayerOneId,
                PlayerTwoId = source.PlayerTwoId,
                IsPlayerOneSkip = source.IsPlayerOneSkip,
                IsPlayerTwoSkip = source.IsPlayerTwoSkip,
                SurrenderPlayerId = source.SurrenderPlayerId,
                Board = Board.GetDeepCopy(source.Board)
            };
        }
    }

    // TODO: Get versioning for serialization ?
    public class GameStateSerializer
    {
        public static GameState Deserialize(String json)
        {
            return JsonConvert.DeserializeObject<GameState>(json);
        }

        public static String Serialize(GameState gameState)
        {
            return JsonConvert.SerializeObject(gameState);
        }
    }
}