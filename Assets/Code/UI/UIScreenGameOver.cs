using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Astrofox
{
	public class UIScreenGameOver : UIScreen
	{
		[SerializeField] private Text m_resultText;
		[SerializeField] private Text m_retryText;

		private bool m_canRetry;

		public override void OnShown()
		{
			if (m_retryText != null)
			{
				m_retryText.gameObject.SetActive(false);
			}
			if (m_resultText != null)
			{
				int currentHighScore = Systems.GameResultProvider.CurrentHighScore;
				int previousHighScore = Systems.GameResultProvider.PreviousHighScore;
				bool hasBeatenHighScore = currentHighScore > previousHighScore;
				m_resultText.text = hasBeatenHighScore ?
					string.Format("Congratulations! Your new high score is {0}", currentHighScore) :
					string.Format("Your high score is {0}", currentHighScore);
			}
			StartCoroutine(EnableRetryAfterDelay());
		}

		private IEnumerator EnableRetryAfterDelay()
		{
			yield return new WaitForSeconds(Systems.GameConfig.GameOverRetryDelaySeconds);
			m_canRetry = true;
			if (m_retryText != null)
			{
				m_retryText.gameObject.SetActive(true);
			}
		}

		private void Update()
		{
			if (m_canRetry && Input.GetMouseButtonDown(0))
			{
				Systems.GameplaySessionController.StartGameplaySession();
			}
		}
	}
}
