using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astrofox
{
	// This class is responsible for applying local inputs to the player actor.
	// It doesn't know how the underlying actor responds to the inputs, which makes it perfect for separating
	// the ship from the input method. This means that hypothetically, we could replace this class with a RemotePlayerController for
	// multiplayer support.
	public class PlayerController
	{
		public Actor PlayerActor
		{
			set
			{
				m_playerActor = value;
				m_playerWeapon = m_playerActor.GetComponent<Weapon>();
				m_playerVisuals = m_playerActor.GetComponent<PlayerShipVisuals>();
			}
			get
			{
				return m_playerActor;
			}
		}
		private Weapon m_playerWeapon;
		private PlayerShipVisuals m_playerVisuals;
		private Actor m_playerActor;

		public PlayerController(Actor playerActor)
		{
			PlayerActor = playerActor;
		}

		public void Update()
		{
			if (m_playerActor == null || m_playerActor.IsDead)
			{
				return;
			}
			Vector3 thrustersDirection = Vector3.zero;
			bool thrustersEnabled = false;
			if (Input.GetKey(KeyCode.A))
			{
				m_playerActor.transform.Rotate(Vector3.forward * Systems.GameConfig.PlayerRotationSpeed * Time.deltaTime, Space.World);
			}
			else if (Input.GetKey(KeyCode.D))
			{
				m_playerActor.transform.Rotate(-Vector3.forward * Systems.GameConfig.PlayerRotationSpeed * Time.deltaTime, Space.World);
			}
			if (Input.GetKey(KeyCode.W))
			{
				thrustersEnabled = true;
				thrustersDirection = m_playerActor.Forward;
				m_playerActor.AddForce(thrustersDirection * Systems.GameConfig.PlayerForwardThrust);
			}
			else if (Input.GetKey(KeyCode.S))
			{
				thrustersEnabled = true;
				thrustersDirection = -m_playerActor.Forward;
				m_playerActor.AddForce(thrustersDirection * Systems.GameConfig.PlayerBackwardThrust);
			}
			if (m_playerWeapon && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
			{
				m_playerWeapon.Shoot(m_playerActor.Forward);
			}

			if (m_playerVisuals != null)
			{
				m_playerVisuals.SetThrustersState(thrustersEnabled);
				m_playerVisuals.SetThrustersDirection(-thrustersDirection);
			}
		}
	}
}
