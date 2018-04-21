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
		public Actor PlayerActor { set; get; }
		private readonly Weapon m_playerWeapon;

		public PlayerController(Actor playerActor)
		{
			PlayerActor = playerActor;
			m_playerWeapon = PlayerActor.GetComponent<Weapon>();
		}

		public void Update()
		{
			if (PlayerActor == null || PlayerActor.IsDead)
			{
				return;
			}
			if (Input.GetKey(KeyCode.A))
			{
				PlayerActor.transform.Rotate(Vector3.forward * Systems.GameConfig.PlayerRotationSpeed * Time.deltaTime, Space.World);
			}
			else if (Input.GetKey(KeyCode.D))
			{
				PlayerActor.transform.Rotate(-Vector3.forward * Systems.GameConfig.PlayerRotationSpeed * Time.deltaTime, Space.World);
			}
			if (Input.GetKey(KeyCode.W))
			{
				PlayerActor.AddForce(PlayerActor.Forward * Systems.GameConfig.PlayerForwardThrust);
			}
			else if (Input.GetKey(KeyCode.S))
			{
				PlayerActor.AddForce(-PlayerActor.Forward * Systems.GameConfig.PlayerBackwardThrust);
			}
			if (m_playerWeapon && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
			{
				m_playerWeapon.Shoot(PlayerActor.Forward);
			}
		}
	}
}
