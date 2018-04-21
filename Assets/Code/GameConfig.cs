using UnityEngine;

namespace Astrofox
{
	[CreateAssetMenu]
	public class GameConfig : ScriptableObject
	{
		[Header("Core")]
		public Actor PlayerPrefab;
		public float PlayerRotationSpeed = 180f;
		public float PlayerForwardThrust = 1f;
		public float PlayerBackwardThrust = 0.5f;
		[Header("Pooling")]
		public int DefaultInitialPoolSize = 10;
		public GameObjectPoolConfig[] PoolConfigs;
		[Header("Difficulty")]
		public DifficultyMilestone[] DifficultyProgression;
	}
}