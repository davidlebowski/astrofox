using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astrofox
{
	public class PooledGameObject : MonoBehaviour
	{
		public GameObject OriginalPrefab;

		public delegate void OnAcquiredDelegate();
		public delegate void OnReleasedDelegate();

		public event OnAcquiredDelegate OnAcquired; // Called when the object is retrieved from the pool
		public event OnReleasedDelegate OnReleased; // Called when the object is returned to the pool

		public void TriggerAcquired()
		{
			if (OnAcquired != null)
			{
				OnAcquired();
			}
		}

		public void TriggerReleased()
		{
			if (OnReleased != null)
			{
				OnReleased();
			}
		}
	}
}
