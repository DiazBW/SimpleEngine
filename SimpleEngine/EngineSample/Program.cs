using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Hosting;
//using System.Security.AccessControl;
//using System.Text;
//using System.Threading.Tasks;
//using SimpleEngine;
//using System.Data;
using SimpleEngine.Classes;
using SimpleEngine.Classes.Game;
using SimpleEngine.Interfaces;

namespace EngineSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Current board!");

            Int32 playerOneId = 0;
            Int32 playerTwoId = 1;
            var game = new Game(playerOneId, playerTwoId);

            //ShowGame(game);
            //Console.ReadKey();
            //game.Turn(0, 0, playerOneId);
            //ShowGame(game);
            //Console.ReadKey();
            //game.Turn(1, 1, playerTwoId);
            //ShowGame(game);
            //Console.ReadKey();
            //game.Turn(2, 2, playerOneId);
            //ShowGame(game);
            //Console.ReadKey();
            //game.Turn(3, 3, playerTwoId);
            //ShowGame(game);
            //Console.ReadKey();

            ActionCycle(game);
        }

        private static void ShowGame(Game game)
        {
            var textBoard = game.GetBoardTextRepresentation();
            Console.Clear();
            //Console.WriteLine("Active player id is " + game.);
            foreach (var line in textBoard)
            {
                Console.WriteLine(line);
            }
        }

        private static void ActionCycle(Game game)
        {
            throw new NotImplementedException();
            //var action = String.Empty;
            //while (action != "exit")
            //{
            //    ShowGame(game);

            //    var inputArgs = Console.ReadLine().Split(' ');
            //    action = inputArgs[0];
            //    switch (action)
            //    {
            //        case "move":
            //            var rowIndex = int.Parse(inputArgs[1]);
            //            var columnIndex = int.Parse(inputArgs[2]);
            //            //var playerId = int.Parse(inputArgs[3]);
            //            ///game.Turn(rowIndex, columnIndex, playerId);
            //            game.DevTurn(rowIndex, columnIndex);
            //            break;
            //        case "clear":
            //            game.ClearBoard();
            //            break;
            //    }
            //}
        }
    }
}
