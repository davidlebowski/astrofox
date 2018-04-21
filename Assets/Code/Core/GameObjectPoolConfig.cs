using System;
using UnityEngine;

namespace Astrofox
{
	[Serializable]
	public struct GameObjectPoolConfig
	{
		public GameObject GameObject;
		public int InitialSize;
		// The factor by which the pool grows once all elements have been acquired. A factor of 0 means it does not grow.
		// A factor of 2 means it doubles in size.
		public float GrowthFactor;
	}
}