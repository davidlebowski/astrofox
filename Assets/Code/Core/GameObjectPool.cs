using System.Collections.Generic;
using UnityEngine;

namespace Astrofox
{
	public class GameObjectPool : MonoBehaviour
	{
		private class PoolContainer
		{
			public int Capacity;
			public List<PooledGameObject> Pool = new List<PooledGameObject>();
		}

		private Dictionary<GameObject, PoolContainer> m_poolsByGameObject = new Dictionary<GameObject, PoolContainer>();
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
			PoolContainer poolContainer;
			if (!m_poolsByGameObject.TryGetValue(prefab, out poolContainer))
			{
				poolContainer = new PoolContainer();
				GameObjectPoolConfig config;
				bool hasConfig = m_poolConfigs.TryGetValue(prefab, out config);
				GrowPool(prefab, poolContainer, hasConfig ? config.InitialSize : Systems.GameConfig.DefaultInitialPoolSize);
				m_poolsByGameObject[prefab] = poolContainer;
			}
			if (poolContainer.Pool.Count == 0)
			{
				// Pool is empty, try to grow it
				GameObjectPoolConfig config;
				if (m_poolConfigs.TryGetValue(prefab, out config) && config.GrowthFactor > 1)
				{
					GrowPool(prefab, poolContainer, Mathf.CeilToInt(poolContainer.Pool.Capacity * config.GrowthFactor));
				}
			}
			GameObject acquiredGameObject = null;
			if (poolContainer.Pool.Count > 0)
			{
				var pool = poolContainer.Pool;
				PooledGameObject pooled = poolContainer.Pool[pool.Count - 1];
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
			PoolContainer poolContainer;
			if (!m_poolsByGameObject.TryGetValue(pooledGameObject.OriginalPrefab, out poolContainer))
			{
				Debug.LogErrorFormat("No pool exists for {0}", pooledGameObject);
				return;
			}
			poolContainer.Pool.Add(pooledGameObject);
			pooledGameObject.gameObject.SetActive(false);
			pooledGameObject.TriggerReleased();
		}

		private void GrowPool(GameObject prefab, PoolContainer poolContainer, int newCapacity)
		{
			if (newCapacity <= poolContainer.Capacity)
			{
				Debug.LogErrorFormat("GrowPool: New pool size {0} must be greater than current {1}", newCapacity, poolContainer.Capacity);
				return;
			}
			int delta = newCapacity - poolContainer.Capacity;
			for (int i = 0; i < delta; ++i)
			{
				poolContainer.Pool.Add(InstantiatePooledObject(prefab));
			}
			poolContainer.Capacity = newCapacity;
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
