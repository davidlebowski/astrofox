using UnityEngine;

namespace Astrofox
{
	[CreateAssetMenu]
	public class GameConfig : ScriptableObject
	{
		[Header("Core")]
		public Actor PlayerPrefab;
		[Header("Difficulty")]
		public float m_secondsIntervalBetweenAsteroidSpawns;
	}
}