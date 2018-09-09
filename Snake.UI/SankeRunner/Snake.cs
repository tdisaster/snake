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
			switch (direction)
			{
				case Direction.Up:
					nextPoint = new Point(Head.X, Head.Y - 1 % ApplicationSettings.MatrixSize);
					break;
				case Direction.Right:
					nextPoint = new Point(Head.X + 1 % ApplicationSettings.MatrixSize, Head.Y);
					break;
				case Direction.Down:
					nextPoint = new Point(Head.X, Head.Y + 1 % ApplicationSettings.MatrixSize);
					break;
				case Direction.Left:
					nextPoint = new Point(Head.X - 1 % ApplicationSettings.MatrixSize, Head.Y);
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
			State = new bool[ApplicationSettings.MatrixSize,ApplicationSettings.MatrixSize];
			var points = GetPoints();
			foreach (var point in points)
			{
				State[point.X, point.Y] = true;
			}
		}

	}
}