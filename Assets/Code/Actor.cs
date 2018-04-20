using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astrofox
{
	// An Actor is a high-level game entity that is somewhat aware of the game rules. For instance, it knows that:
	// - Game entities can die.
	// - Game entities wrap around the screen edges.
	// - Game entities can drift in space, therefore it has a Velocity property that it uses to move the object every frame.
	public class Actor : MonoBehaviour
	{
		public delegate void OnDeathDelegate(Actor deadActor);
		public event OnDeathDelegate OnDeath;

		[SerializeField] private bool m_wrap = true;
		[SerializeField] private float m_maxSpeed = 1;
		private Vector3 m_velocity;
		private float m_angularSpeed;
		private Transform m_transform;

		public Vector3 Velocity
		{
			get { return m_velocity; }
			set
			{
				m_velocity = value;
				m_velocity.z = 0; // Only moving across the XY plane is allowed.
				float magnitude = m_velocity.magnitude;
				if (magnitude > m_maxSpeed)
				{
					m_velocity = m_velocity / magnitude * m_maxSpeed;
				}
			}
		}
		public float Mass = 1;
		public bool IsDead { set; get; }

		private void Awake()
		{
			m_transform = transform;
		}

		private void Update()
		{
			m_transform.Translate(Velocity * Time.deltaTime, Space.World);
		}

		public void AddForce(Vector3 force, Space space = Space.World)
		{
			if (space == Space.Self)
			{
				force = transform.localToWorldMatrix * force;
			}
			Velocity += (force * Time.deltaTime) / Mass;
		}

		public void Kill()
		{
			if (IsDead)
			{
				return;
			}
			IsDead = true;
			if (OnDeath != null)
			{
				OnDeath(this);
			}
			Destroy(gameObject);
		}
	}
}
