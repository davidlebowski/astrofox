using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astrofox
{
	public class GameObjectFactory
	{
		private readonly Transform m_container;

		public GameObjectFactory(Transform container)
		{
			m_container = container;
		}

		public T Instantiate<T>(T prefab) where T : UnityEngine.Object
		{
			return Object.Instantiate(prefab, m_container.transform);
		}
	}
}
