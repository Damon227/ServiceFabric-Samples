// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : TestClient2
// File             : Program.cs
// Created          : 2017-01-18  15:51
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Game.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Player.Interfaces;

namespace TestClient2
{
    internal class Program
    {
        private static int s_total;

        private static void Main(string[] args)
        {
            IPlayer player1 = ActorProxy.Create<IPlayer>(ActorId.CreateRandom(), new Uri("fabric:/ActorTicTacToeApplication/PlayerActorService"));
            IPlayer player2 = ActorProxy.Create<IPlayer>(ActorId.CreateRandom(), new Uri("fabric:/ActorTicTacToeApplication/PlayerActorService"));

            //int i = 0;
            //do
            //{
            //    i++;
            //    string result = player1.GetCurrentInstanceAsync(1, 2).GetAwaiter().GetResult();
            //    Console.WriteLine(result);
            //} while (i < 1000);

            ActorId gameId = ActorId.CreateRandom();

            IGame game = ActorProxy.Create<IGame>(gameId, new Uri("fabric:/ActorTicTacToeApplication/GameActorService"));

            List<Tuple<long, string>> players0 = game.GetPlayersAsync().GetAwaiter().GetResult();

            //Task<bool> result1 = player1.JoinGameAsync(gameId, "Player 1");

            bool a = game.JoinGameAsync(ActorId.CreateRandom().GetLongId(), "Player 1").GetAwaiter().GetResult();
            bool b = game.JoinGameAsync(ActorId.CreateRandom().GetLongId(), "Player 2").GetAwaiter().GetResult();

            //Task<bool> result2 = player2.JoinGameAsync(gameId, "Player 2");

            if (!a || !b)
            {
                Console.WriteLine("Failed to join game.");
                return;
            }


            List<Tuple<long, string>> players = game.GetPlayersAsync().GetAwaiter().GetResult();

            Task.Run(() => MakeMoveAsync(player1, game, gameId));
            int[] board1 = game.GetGameBoardAsync().GetAwaiter().GetResult();
            Task.Run(() => MakeMoveAsync(player2, game, gameId));
            int[] board2 = game.GetGameBoardAsync().GetAwaiter().GetResult();

            Task gameTask = Task.Run(() =>
            {
                string winner = null;
                while (winner == null)
                {
                    int[] board = game.GetGameBoardAsync().GetAwaiter().GetResult();
                    PrintBoard(board);

                    winner = game.GetWinnerAsync().GetAwaiter().GetResult();
                    Task.Delay(1000).Wait();
                }

                Console.WriteLine($"Winner is : {winner}");
            });

            gameTask.Wait();
            Console.Read();
        }

        [SuppressMessage("ReSharper", "FunctionNeverReturns")]
        private static async Task MakeMoveAsync(IPlayer player, IGame game, ActorId gameId)
        {
            Random rand = new Random();
            while (true)
            {
                //await game.MakeMoveAsync(player.GetActorId().GetLongId(), rand.Next(0, 3), rand.Next(0, 3));
                string now = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Console.WriteLine(now);
                await Task.Delay(rand.Next(500, 2000));
            }
        }

        [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
        private static void PrintBoard(int[] board)
        {
            Console.Clear();

            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == -1)
                {
                    Console.WriteLine(" X ");
                }
                else if (board[i] == 1)
                {
                    Console.WriteLine(" 0 ");
                }
                else
                {
                    Console.WriteLine(" . ");
                }

                if ((i + 1) % 3 == 0)
                {
                    Console.WriteLine();
                }
            }
        }
    }
}