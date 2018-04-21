using UnityEngine;

namespace Astrofox
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] private Actor m_projectile;
		[SerializeField] private float m_speed = 1;

		public void Shoot(Vector3 direction)
		{
			if (m_projectile != null)
			{
				Actor inst = Systems.GameObjectFactory.Instantiate(m_projectile);
				inst.transform.position = transform.position;
				inst.Velocity = direction.normalized * m_speed;
			}
		}
	}
}