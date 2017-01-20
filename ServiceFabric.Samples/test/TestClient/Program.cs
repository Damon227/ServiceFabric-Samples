// ***********************************************************************
// Solution         : ServiceFabric.Samples
// Project          : TestClient
// File             : Program.cs
// Created          : 2017-01-18  15:38
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Game.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Player.Interfaces;

namespace TestClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IPlayer player1 = ActorProxy.Create<IPlayer>(ActorId.CreateRandom(), "fabric:/ActorTicTacToeApplication");
            IPlayer player2 = ActorProxy.Create<IPlayer>(ActorId.CreateRandom(), "fabric:/ActorTicTacToeApplication");

            ActorId gameId = ActorId.CreateRandom();

            IGame game = ActorProxy.Create<IGame>(gameId, "fabric:/ActorTicTacToeApplication");

            Task<bool> result1 = player1.JoinGameAsync(gameId, "Player 1");
            Task<bool> result2 = player1.JoinGameAsync(gameId, "Player 2");

            if (!result1.Result || !result2.Result)
            {
                Console.WriteLine("Failed to join game.");
                return;
            }

            Task.Run(() => MakeMoveAsync(player1, game, gameId));
            Task.Run(() => MakeMoveAsync(player2, game, gameId));

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
        private static async void MakeMoveAsync(IPlayer player, IGame game, ActorId gameId)
        {
            Random rand = new Random();
            while (true)
            {
                await player.MakeMoveAsync(gameId, rand.Next(0, 3), rand.Next(0, 3));
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