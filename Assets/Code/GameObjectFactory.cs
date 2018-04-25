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

		public GameObject Instantiate(GameObject prefab)
		{
			GameObject result = null;
			if (m_gameObjectPool.IsConfiguredForPooling(prefab))
			{
				GameObject pooledObject = m_gameObjectPool.Request(prefab);
				if (pooledObject != null)
				{
					result = pooledObject;
				}
			}
			else
			{
				result = Object.Instantiate(prefab, m_container.transform);
			}
			return result;
		}

		public T Instantiate<T>(T prefab) where T : UnityEngine.MonoBehaviour
		{
			GameObject inst = Instantiate(prefab.gameObject);
			if (inst != null)
			{
				return inst.GetComponent<T>();
			}
			return null;
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
