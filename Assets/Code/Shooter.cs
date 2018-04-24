using System.Collections;
using UnityEngine;

namespace Astrofox
{
	[RequireComponent(typeof(Actor), typeof(Weapon))]
	public class Shooter : MonoBehaviour
	{
		[SerializeField] private bool m_chasePlayer;
		[SerializeField] private float m_chaseThrust = 1;
		[SerializeField] private float m_shootIntervalSeconds = 3;
		private Actor m_actor;
		private Weapon m_weapon;

		private void Awake()
		{
			m_actor = GetComponent<Actor>();
			m_weapon = GetComponent<Weapon>();
			StartCoroutine(ShootingCoroutine());
			if (m_chasePlayer)
			{
				StartCoroutine(ChasingCoroutine());
			}
		}

		private IEnumerator ShootingCoroutine()
		{
			while (true)
			{
				yield return new WaitForSeconds(m_shootIntervalSeconds);
				Actor playerActor = GetAlivePlayerActor();
				if (playerActor != null)
				{
					Vector3 dir = (playerActor.transform.position - transform.position).normalized;
					m_weapon.Shoot(dir);
				}
			}
		}

		private IEnumerator ChasingCoroutine()
		{
			while (true)
			{
				Actor playerActor = GetAlivePlayerActor();
				if (playerActor != null)
				{
					Vector3 dir = (playerActor.transform.position - transform.position).normalized;
					m_actor.AddForce(dir * m_chaseThrust * Time.deltaTime);
				}
				yield return null;
			}
		}

		private static Actor GetAlivePlayerActor()
		{
			Actor playerActor = Systems.PlayerActorProvider.PlayerActor;
			return playerActor != null && !playerActor.IsDead ? playerActor : null;
		}
	}
}
