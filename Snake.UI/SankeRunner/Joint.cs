using System;

namespace SankeRunner
{
	public class Joint
	{
		public Point Point { get; set; }
		public Direction Direction { get; set; }

		public Joint(Point point, Direction direction)
		{
			Point = point;
			Direction = direction;
		}

		public Joint()
		{

		}

		public Joint(int x, int y, Direction direction)
		{
			Point = new Point(x, y);
			Direction = direction;
		}
		public void Move()
		{
			switch (Direction)
			{
				case Direction.Up:

					break;
				case Direction.Right:
					break;
				case Direction.Down:
					break;
				case Direction.Left:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}