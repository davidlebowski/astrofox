using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astrofox
{
	public class GameObjectPool : MonoBehaviour
	{
		private Dictionary<GameObject, List<PooledGameObject>> m_pooledObjects = new Dictionary<GameObject, List<PooledGameObject>>();
		private Dictionary<GameObject, GameObjectPoolConfig> m_poolConfigs = new Dictionary<GameObject, GameObjectPoolConfig>();

		public void SetConfigs(IEnumerable<GameObjectPoolConfig> poolConfigs)
		{
			foreach (GameObjectPoolConfig config in poolConfigs)
			{
				m_poolConfigs[config.GameObject] = config;
			}
		}

		public bool IsConfiguredForPooling(GameObject prefab)
		{
			return m_poolConfigs.ContainsKey(prefab);
		}

		public GameObject Request(GameObject prefab)
		{
			List<PooledGameObject> pool;
			if (!m_pooledObjects.TryGetValue(prefab, out pool))
			{
				pool = new List<PooledGameObject>();
				GameObjectPoolConfig config;
				bool hasConfig = m_poolConfigs.TryGetValue(prefab, out config);
				GrowPool(prefab, pool, hasConfig ? config.InitialSize : Systems.GameConfig.DefaultInitialPoolSize);
				m_pooledObjects[prefab] = pool;
			}
			if (pool.Count == 0)
			{
				// Pool is empty, try to grow it
				GameObjectPoolConfig config;
				if (m_poolConfigs.TryGetValue(prefab, out config) && config.GrowthFactor > 0)
				{
					GrowPool(prefab, pool, Mathf.CeilToInt(pool.Count * config.GrowthFactor));
				}
			}
			GameObject acquiredGameObject = null;
			if (pool.Count > 0)
			{
				PooledGameObject pooled = pool[pool.Count - 1];
				pool.RemoveAt(pool.Count - 1);
				if (pooled == null)
				{
					Debug.LogError("Found a null entry in the object pool, which means it was destroyed unexpectedly." +
					               "Implicit or explicit destruction of a pooled object is not allowed.");
					return null;
				}
				acquiredGameObject = pooled.gameObject;
				pooled.TriggerAcquired();
				acquiredGameObject.SetActive(true);
			}
			return acquiredGameObject;
		}

		public void Release(PooledGameObject pooledGameObject)
		{
			List<PooledGameObject> pool;
			if (!m_pooledObjects.TryGetValue(pooledGameObject.OriginalPrefab, out pool))
			{
				Debug.LogErrorFormat("No pool exists for {0}", pooledGameObject);
				return;
			}
			pool.Add(pooledGameObject);
			pooledGameObject.gameObject.SetActive(false);
			pooledGameObject.TriggerReleased();
		}

		private void GrowPool(GameObject prefab, List<PooledGameObject> pool, int newSize)
		{
			if (newSize <= pool.Count)
			{
				Debug.LogErrorFormat("GrowPool: New pool size {0} must be greater than current {1}", newSize, pool.Count);
				return;
			}
			int delta = newSize - pool.Count;
			for (int i = 0; i < delta; ++i)
			{
				pool.Add(InstantiatePooledObject(prefab));
			}
		}

		private PooledGameObject InstantiatePooledObject(GameObject prefab)
		{
			PooledGameObject inst = Instantiate(prefab).AddComponent<PooledGameObject>();
			inst.OriginalPrefab = prefab;
			inst.gameObject.SetActive(false);
			inst.transform.parent = transform;
			return inst;
		}
	}
}
