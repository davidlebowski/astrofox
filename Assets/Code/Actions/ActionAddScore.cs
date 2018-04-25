using UnityEngine;

namespace Astrofox
{
	[CreateAssetMenu]
	public class ActionAddScore : BaseAction
	{
		[SerializeField] private int m_score;

		public override void Run(ActionContext context)
		{
			Systems.ScoreController.AddScore(m_score);
		}
	}
}
