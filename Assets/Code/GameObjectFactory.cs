using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astrofox
{
	public class GameObjectFactory
	{
		private readonly Transform m_container;
		private readonly GameObjectPool m_gameObjectPool;

		public GameObjectFactory(Transform container, GameObjectPool gameObjectPool)
		{
			m_container = container;
			m_gameObjectPool = gameObjectPool;
		}

		public T Instantiate<T>(T prefab) where T : UnityEngine.Object
		{
			return Object.Instantiate(prefab, m_container.transform);
		}

		public void Release(GameObject inst)
		{
			PooledGameObject pooled = inst.GetComponent<PooledGameObject>();
			if (pooled == null)
			{
				Object.Destroy(inst);
			}
			else
			{
				m_gameObjectPool.Release(pooled);
			}
		}
	}
}
