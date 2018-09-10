using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SankeRunner
{
	public class Snake
	{
		public Point Head { get; set; }
		public List<Joint> Joints { get; set; } = new List<Joint>();

		public Direction CurrentDirection => Joints[0].Direction;
		public Point End => Joints.Last().Point;

		public bool[,] State { get; private set; } = new bool[ApplicationSettings.MatrixSize, ApplicationSettings.MatrixSize];

		public void Move(Direction direction, bool eat)
		{
			Point nextPoint;
			var mod = ApplicationSettings.MatrixSize - 1;
			switch (direction)
			{
				case Direction.Up:
					var y = (Head.Y - 1) % mod;
					if (y < 0)
					{
						y = mod;
					}
					nextPoint = new Point(Head.X, y);
					break;
				case Direction.Right:
					nextPoint = new Point((Head.X + 1) % mod, Head.Y);
					break;
				case Direction.Down:
					nextPoint = new Point(Head.X, (Head.Y + 1) % mod);
					break;
				case Direction.Left:
					var x = (Head.X - 1) % mod;
					if (x < 0)
					{
						x = mod;
					}
					nextPoint = new Point(x, Head.Y);

					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}

			if (State[nextPoint.X, nextPoint.Y])
			{
				throw new GameOverException("Invalid coordinate");
			}

			State[nextPoint.X, nextPoint.Y] = true;

			if (direction != CurrentDirection)
			{
				Joints.Insert(0, new Joint(Head, direction));
			}

			if (!eat)
			{
				var count = Joints.Count;
				var end = Joints[count - 1];
				State[end.Point.X, end.Point.Y] = false;
				switch (end.Direction)
				{
					case Direction.Up:
						end.Point.Y--;
						break;
					case Direction.Right:
						end.Point.X++;
						break;
					case Direction.Down:
						end.Point.Y++;
						break;
					case Direction.Left:
						end.Point.X--;
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
				}

				if (count > 2 && Joints[count - 2].Point.Equals(end.Point))
				{
					Joints.Remove(end);
				}
			}

			Head = nextPoint;
		}

		public List<Point> GetPoints()
		{
			var result = new List<Point>();
			result.Add(Head);
			var lastPoint = Head;
			foreach (var joint in Joints)
			{
				switch (joint.Direction)
				{
					case Direction.Left:
						for (int i = lastPoint.X + 1; i <= joint.Point.X; i++)
						{
							result.Add(new Point(i, lastPoint.Y));
						}

						break;
					case Direction.Up:
						for (int i = lastPoint.Y + 1; i <= joint.Point.Y; i++)
						{
							result.Add(new Point(lastPoint.X, i));
						}

						break;
					case Direction.Right:
						for (int i = lastPoint.X - 1; i >= joint.Point.X; i--)
						{
							result.Add(new Point(i, lastPoint.Y));
						}

						break;
					case Direction.Down:
						for (int i = lastPoint.Y - 1; i >= joint.Point.Y; i--)
						{
							result.Add(new Point(lastPoint.X, i));
						}

						break;
				}

				lastPoint = joint.Point;
			}

			return result;
		}

		public void SetState()
		{
			State = new bool[ApplicationSettings.MatrixSize, ApplicationSettings.MatrixSize];
			var points = GetPoints();
			foreach (var point in points)
			{
				State[point.X, point.Y] = true;
			}
		}
	}
}