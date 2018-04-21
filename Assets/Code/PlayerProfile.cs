using UnityEngine;

namespace Astrofox
{
	public sealed class PlayerProfile
	{
		private const string BestScoreKey = "best_score";
		public int BestScore;

		private PlayerProfile()
		{
			// We allow constructing this object only via the static method Load().
		}

		public void Commit()
		{
			// Rather than saving every time we change a key, we only save when we call Commit, to avoid spamming
			// the backend with requests.
			PlayerPrefs.SetInt(BestScoreKey, BestScore);
		}

		public static PlayerProfile Load()
		{
			// For simplicity, we load the profile synchronously and store it in the PlayerPrefs.
			// We can always change the paradigm to asynchronous to support the profile being stored remotely.
			return new PlayerProfile
			{
				BestScore = PlayerPrefs.GetInt(BestScoreKey, 0)
			};
		}
	}
}