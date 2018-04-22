using UnityEngine;
using UnityEngine.UI;

namespace Astrofox
{
	public class UIScreenGameHud : UIScreen
	{
		[SerializeField] private Text m_scoreText;

		private void Awake()
		{
			Systems.ScoreController.OnScoreChanged -= HandleScoreChanged;
			Systems.ScoreController.OnScoreChanged += HandleScoreChanged;
			UpdateScoreText();
		}

		private void HandleScoreChanged()
		{
			UpdateScoreText();
		}

		private void UpdateScoreText()
		{
			if (m_scoreText != null)
			{
				m_scoreText.text = Systems.ScoreController.Score.ToString();
			}
		}
	}
}
