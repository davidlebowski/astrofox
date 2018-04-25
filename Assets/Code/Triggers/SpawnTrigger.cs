using UnityEngine;

namespace Astrofox
{
	[RequireComponent(typeof(Actor))]
	public class SpawnTrigger : MonoBehaviour
	{
		[SerializeField] private BaseAction[] m_actions;

		private void OnEnable()
		{
			ActionContext context = new ActionContext
			{
				ThisGameObject = gameObject
			};
			foreach (BaseAction action in m_actions)
			{
				action.Run(context);
			}
		}
	}
}
