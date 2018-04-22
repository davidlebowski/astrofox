using UnityEngine;

namespace Astrofox
{
	public sealed class PlayerProfile
	{
		private const string HighScoreKey = "high_score";
		public int HighScore;

		private PlayerProfile()
		{
			// We allow constructing this object only via the static method Load().
		}

		public void Commit()
		{
			// Rather than saving every time we change a key, we only save when we call Commit, to avoid spamming
			// the backend with requests.
			PlayerPrefs.SetInt(HighScoreKey, HighScore);
		}

		public static PlayerProfile Load()
		{
			// For simplicity, we load the profile synchronously and store it in the PlayerPrefs.
			// We can always change the paradigm to asynchronous to support the profile being stored remotely.
			return new PlayerProfile
			{
				HighScore = PlayerPrefs.GetInt(HighScoreKey, 0)
			};
		}
	}
}