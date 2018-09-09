using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace SankeRunner
{
	public class Game
	{
		private Snake Snake { get; set; }
		private bool IsRunning { get; set; }
		public Game()
		{
			Snake = new Snake
			{
				Head = new Point(3, 3),
				Joints = new List<Joint>
				{
					new Joint
					{
						Point = new Point(6, 3),
						Direction = Direction.Left
					},
					new Joint
					{
						Point = new Point(6, 4),
						Direction = Direction.Up
					},
					new Joint
					{
						Point = new Point(5, 4),
						Direction = Direction.Right
					}
				}
			};
			Snake.SetState();
		}
		Direction GetDirection(Direction defaultDirection, int duration)
		{
			ReadKeyDelegate d = Console.ReadKey;
			if (duration <= 0)
			{
				return defaultDirection;
			}
			if (ReadResult == null || ReadResult.IsCompleted)
			{
				ReadResult = d.BeginInvoke(null, null);
				_oldDelegate = d;
			}

			ReadResult.AsyncWaitHandle.WaitOne(duration);
			if (ReadResult.IsCompleted)
			{
				ConsoleKeyInfo resultstr = _oldDelegate.EndInvoke(ReadResult);
				switch (resultstr.Key)
				{
					case ConsoleKey.UpArrow:
						return Direction.Up;
					case ConsoleKey.DownArrow:
						return Direction.Down;
					case ConsoleKey.LeftArrow:
						return Direction.Left;
					case ConsoleKey.RightArrow:
						return Direction.Right;
					default:
						return defaultDirection;
				}
			}
			return defaultDirection;
		}

		private  delegate ConsoleKeyInfo ReadKeyDelegate();

		private ReadKeyDelegate _oldDelegate;
		private IAsyncResult ReadResult = null;

		private static void PrintSnake(Snake snake)
		{
			Console.Clear();
			for (int y = 0; y < snake.State.GetLength(1); y++)
			{
				for (int x = 0; x < snake.State.GetLength(0); x++)
				{
					if (snake.State[x, y])
					{
						Console.SetCursorPosition(x, y);
						var head = snake.Head.X == x && snake.Head.Y == y;
						Console.Write(head ? '@' : '*');
					}
					else
					{
						Console.Write(' ');
					}
				}

				Console.Write("|\n");
			}
			for (int i = 0; i < snake.State.GetLength(0); i++)
			{
				Console.Write('-');
			}
			Console.Write('+');
		}

		public void Start()
		{
			IsRunning = true;
			PrintSnake(Snake);
			while (IsRunning)
			{
				var s = new Stopwatch();
				s.Start();
				Console.Write($"\nReading first");
				var direction = GetDirection( Snake.CurrentDirection, ApplicationSettings.TickSpeed);
				Console.Write($"\nRead {direction}");

				while (s.ElapsedMilliseconds < ApplicationSettings.TickSpeed)
				{
					Console.Write($"\nReading N");
					direction = GetDirection( direction, ApplicationSettings.TickSpeed - (int)s.ElapsedMilliseconds);
					Console.Write($"\nRead {direction}");
				}
				s.Stop();
				s.Reset();
				try
				{
					Snake.Move(direction, false);

				}
				catch (GameOverException)
				{
					IsRunning = false;
				}
				PrintSnake(Snake);

				if (IsRunning)
				{
					Console.Write($"\nMoved{direction}");
				}
				else
				{
					Console.WriteLine("\nGame Over");
				}
			}
		}
	}
}
