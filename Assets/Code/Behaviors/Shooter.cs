using System.Collections;
using UnityEngine;

namespace Astrofox
{
	[RequireComponent(typeof(Weapon))]
	public class Shooter : MonoBehaviour
	{
		[SerializeField] private float m_shootIntervalSeconds = 3;
		private Weapon m_weapon;

		private void Awake()
		{
			m_weapon = GetComponent<Weapon>();
			StartCoroutine(ShootingCoroutine());
		}

		private IEnumerator ShootingCoroutine()
		{
			while (true)
			{
				yield return new WaitForSeconds(m_shootIntervalSeconds);
				Actor playerActor = Systems.PlayerActorProvider.PlayerActor;
				if (playerActor != null && !playerActor.IsDead)
				{
					Vector3 closestTargetPosition = MathUtils.GetClosestReachablePosition(Systems.GameCamera.WorldBounds,
						transform.position, playerActor.transform.position);
					Vector3 dir = (closestTargetPosition - transform.position).normalized;
					m_weapon.Shoot(dir);
				}
			}
		}
	}
}
