using System;
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
		private static readonly Bounds s_noBounds = new Bounds();

		public delegate void OnDeathDelegate(Actor deadActor);
		public event OnDeathDelegate OnDeath;

		[SerializeField] private bool m_wrap = true;
		[SerializeField] private float m_maxSpeed = 1;
		[SerializeField] private float m_mass = 1;
		private Vector3 m_velocity;
		private Transform m_transform;
		private Collider m_collider;

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
		public Vector3 Forward
		{
			get { return -transform.right; }
		}
		public Bounds Bounds
		{
			get
			{
				return m_collider != null ? m_collider.bounds : s_noBounds;
			}
		}
		public bool IsDead { set; get; }

		private void Awake()
		{
			m_transform = transform;
			Rigidbody body = GetComponent<Rigidbody>();
			if (body == null)
			{
				// Used for triggering collision events only
				body = gameObject.AddComponent<Rigidbody>();
				body.isKinematic = true;
				body.useGravity = false;
			}
			m_collider = GetComponent<Collider>();
		}

		private void Update()
		{
			m_transform.Translate(Velocity * Time.deltaTime, Space.World);
			if (m_wrap)
			{
				WrapAroundScreenEdges();
			}
		}

		private void WrapAroundScreenEdges()
		{
			Vector3 position = m_transform.position;
			Bounds worldBounds = Systems.GameCamera.WorldBounds;
			if (Bounds != s_noBounds)
			{
				// Take into account the actor's bounds
				worldBounds.max += Bounds.extents;
				worldBounds.min -= Bounds.extents;
			}
			if (position.x > worldBounds.max.x)
			{
				// Wrap right edge
				position.x = worldBounds.min.x + position.x % worldBounds.max.x;
			}
			else if (position.x < worldBounds.min.x)
			{
				// Wrap left edge
				position.x = worldBounds.max.x - position.x % worldBounds.min.x;
			}
			if (position.y > worldBounds.max.y)
			{
				// Wrap top edge
				position.y = worldBounds.min.y + position.y % worldBounds.max.y;
			}
			else if (position.y < worldBounds.min.y)
			{
				// Wrap bottom edge
				position.y = worldBounds.max.y - position.y % worldBounds.min.y;
			}
			m_transform.position = position;
		}

		public void AddForce(Vector3 force)
		{
			Velocity += (force * Time.deltaTime) / m_mass;
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
			Systems.GameObjectFactory.Release(gameObject);
		}
	}
}
