using UnityEngine;

namespace Astrofox
{
	[CreateAssetMenu]
	public class ActionKillActors : BaseAction
	{
		[SerializeField] private bool m_killThis;
		[SerializeField] private bool m_killOther;

		public override void Run(ActionContext context)
		{
			if (m_killOther)
			{
				TryKillActorIfExists(context.OtherGameObject);
			}
			if (m_killThis)
			{
				TryKillActorIfExists(context.ThisGameObject);
			}
		}

		private static void TryKillActorIfExists(GameObject gameObject)
		{
			if (gameObject != null)
			{
				Actor actor = gameObject.GetComponent<Actor>();
				if (actor != null)
				{
					actor.Kill();
				}
			}
		}
	}
}
