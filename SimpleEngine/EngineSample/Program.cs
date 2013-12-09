﻿using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Hosting;
//using System.Security.AccessControl;
//using System.Text;
//using System.Threading.Tasks;
using SimpleEngine;

namespace EngineSample
{
    class Program
    {
        private static int activePlayerId;

        static void Main(string[] args)
        {
            activePlayerId = 1;

            Console.WriteLine("Current board!");

            var newGame = new Game(playerOneId: 0, playerTwoId: 1);

            newGame.SetCellStatus(0, 0, Cell.White, activePlayerId);
            newGame.SetCellStatus(1, 1, Cell.White, activePlayerId);
            newGame.SetCellStatus(2, 2, Cell.White, activePlayerId);
            newGame.SetCellStatus(7, 7, Cell.Black, activePlayerId);
            newGame.SetCellStatus(8, 8, Cell.Black, activePlayerId);
            newGame.SetCellStatus(9, 9, Cell.Black, activePlayerId);

            ActionCycle(newGame);
            
        }

        private static void ShowGame(Game game)
        {
            var textBoard = game.GetBoardTextRepresentation();
            Console.Clear();
            Console.WriteLine("Active player id is " + activePlayerId);
            foreach (var line in textBoard)
            {
                Console.WriteLine(line);
            }
        }

        private static void ActionCycle(Game game)
        {
            var action = String.Empty;
            while (action != "exit")
            {
                ShowGame(game);

                var inputArgs = Console.ReadLine().Split(' ');
                action = inputArgs[0];
                switch (action)
                {
                    case "move":
                        var x = int.Parse(inputArgs[1]);
                        var y = int.Parse(inputArgs[2]);
                        var value = inputArgs[3];
                        if (value == "x")
                            game.SetCellStatus(x, y, Cell.White, activePlayerId);
                        if (value == "o")
                            game.SetCellStatus(x, y, Cell.Black, activePlayerId);
                        if (value == ".")
                            game.SetCellStatus(x, y, Cell.Empty, activePlayerId);
                        break;
                    case "clear":
                        game.ClearBoard();
                        break;
                    case "player":
                        activePlayerId = int.Parse(inputArgs[1]);

                        break;
                }
            }
        }
    }
}