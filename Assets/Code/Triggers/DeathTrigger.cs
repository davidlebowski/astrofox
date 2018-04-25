using UnityEngine;

namespace Astrofox
{
	[RequireComponent(typeof(Actor))]
	public class DeathTrigger : MonoBehaviour
	{
		[SerializeField] private BaseAction[] m_actions;

		private Actor m_actor;

		private void OnEnable()
		{
			m_actor = GetComponent<Actor>();
			m_actor.OnDeath += OnDeath;
		}

		private void OnDisable()
		{
			m_actor.OnDeath -= OnDeath;
		}

		private void OnDeath(Actor deadActor)
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
