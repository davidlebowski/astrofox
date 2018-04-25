using System;

namespace Astrofox
{
	public interface IScoreController
	{
		event Action OnScoreChanged;

		int Score { get; }
		void AddScore(int amount);
	}
}
