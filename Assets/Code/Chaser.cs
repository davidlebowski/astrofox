using System.Collections;
using UnityEngine;

namespace Astrofox
{
	[RequireComponent(typeof(Actor))]
	public class Chaser : MonoBehaviour
	{
		[SerializeField] private float m_chaseThrust = 1;
		[SerializeField] private bool m_lookAtTarget;
		private Actor m_actor;

		private void Awake()
		{
			m_actor = GetComponent<Actor>();
		}

		private void Update()
		{
			Actor playerActor = Systems.PlayerActorProvider.PlayerActor;
			if (playerActor == null || playerActor.IsDead)
			{
				return;
			}
			Vector3 dir = (playerActor.transform.position - transform.position).normalized;
			m_actor.AddForce(dir * m_chaseThrust * Time.deltaTime);

			if (m_lookAtTarget)
			{
				m_actor.Forward = dir;
			}
		}
	}
}
