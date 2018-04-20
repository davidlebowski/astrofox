﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Astrofox
{
	public class GameplayController : MonoBehaviour
	{
		private PlayerController m_playerController;
		private Actor m_playerActor;

		private bool m_hasStarted;

		public void StartGameplay()
		{
			if (m_hasStarted)
			{
				Debug.LogError("Gameplay has already started!");
				return;
			}
			m_hasStarted = true;
			SpawnPlayer();
		}

		private void SpawnPlayer()
		{
			if (m_playerActor != null)
			{
				// Destroy old player if present
				Destroy(m_playerActor.gameObject);
			}
			m_playerActor = Systems.GameObjectFactory.Instantiate(Systems.GameConfig.PlayerPrefab);
			m_playerActor.transform.position = Vector3.zero;
			if (m_playerController == null)
			{
				m_playerController = new PlayerController(m_playerActor);
			}
			else
			{
				m_playerController.PlayerActor = m_playerActor;
			}
		}

		private void Update()
		{
			if (!m_hasStarted)
			{
				return;
			}
			m_playerController.Update();
		}
	}
}