﻿using UnityEngine;

namespace Astrofox
{
	public class PlayerShipVisuals : MonoBehaviour
	{
		[SerializeField] private ParticleSystem m_thrusters;

		public void SetThrustersDirection(Vector3 direction)
		{
			if (m_thrusters != null && direction != Vector3.zero)
			{
				m_thrusters.transform.forward = direction;
			}
		}

		public void SetThrustersState(bool enabled)
		{
			if (m_thrusters != null)
			{
				ParticleSystem.EmissionModule emissionModule = m_thrusters.emission;
				emissionModule.enabled = enabled;
			}
		}
	}
}
