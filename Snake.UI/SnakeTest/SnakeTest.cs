using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SankeRunner;

namespace SnakeTest
{
	[TestClass]
	public class SnakeTest
	{

		public Snake SmallSanke { get; set; } 

		public Snake AllDirectionSnake { get; set; } 

		[TestInitialize]
		public void TestInitialize()
		{
			SmallSanke = new Snake
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
			SmallSanke.SetState();
			AllDirectionSnake = new Snake
			{
				Head = new Point(3, 4),
				Joints = new List<Joint>
				{
					new Joint
					{
						Point = new Point(2, 4),
						Direction = Direction.Right
					},
					new Joint
					{
						Point = new Point(2, 2),
						Direction = Direction.Down
					},
					new Joint
					{
						Point = new Point(5, 2),
						Direction = Direction.Left
					},
					new Joint
					{
						Point = new Point(5, 3),
						Direction = Direction.Up
					},
					new Joint
					{
						Point = new Point(4, 3),
						Direction = Direction.Right
					}
				}
			};
			AllDirectionSnake.SetState();
		}

		[TestMethod]
		public void TestPoints()
		{
			var points = SmallSanke.GetPoints();
			Assert.IsTrue(points.Count == 6);
		}

		[TestMethod]
		public void TestAllDirectionPoints()
		{
			var points = AllDirectionSnake.GetPoints();
			Assert.IsTrue(points.Count == 9);
		}


		[TestMethod]
		public void TestState()
		{
			int count = 0;
			for (int x = 0; x < SmallSanke.State.GetLength(0); x++)
			{
				for (int y = 0; y < SmallSanke.State.GetLength(1); y++)
				{
					if (SmallSanke.State[x, y])
					{
						count++;
					}
				}
			}
			Assert.IsTrue(count == 6);
		}

		[TestMethod]
		public void TestMove()
		{
			SmallSanke.Move(Direction.Left, false);
			SmallSanke.Move(Direction.Up, false);
			SmallSanke.Move(Direction.Left, false);
			SmallSanke.Move(Direction.Down, false);
		}

		[TestMethod]
		[ExpectedException(typeof(GameOverException))]
		public void TestInvalidMove()
		{
			SmallSanke.Move(Direction.Right, false);
		}
	}
}
