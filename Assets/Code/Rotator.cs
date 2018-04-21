using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

namespace Astrofox
{
	public sealed class Rotator : MonoBehaviour
	{
		[SerializeField] private Transform m_target;
		[SerializeField] private Vector3 m_randomEulerSpeedA;
		[SerializeField] private Vector3 m_randomEulerSpeedB;

		private Vector3 m_eulerSpeed;

		private void Awake()
		{
			if (m_target == null)
			{
				m_target = transform;
			}
			m_eulerSpeed = Vector3.zero;
			m_eulerSpeed.x = Random.Range(m_randomEulerSpeedA.x, m_randomEulerSpeedB.x);
			m_eulerSpeed.y = Random.Range(m_randomEulerSpeedA.y, m_randomEulerSpeedB.y);
			m_eulerSpeed.z = Random.Range(m_randomEulerSpeedA.z, m_randomEulerSpeedB.z);
		}

		private void Update()
		{
			m_target.Rotate(m_eulerSpeed * Time.deltaTime, Space.World);
		}
	}
}
