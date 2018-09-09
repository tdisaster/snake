using System;

namespace SankeRunner
{
	public class GameOverException : Exception
	{
		public GameOverException(string message) : base(message)
		{
		}
	}
}