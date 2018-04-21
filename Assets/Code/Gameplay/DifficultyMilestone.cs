using System;
using System.Collections;
using System.Collections.Generic;

namespace Astrofox
{
	[Serializable]
	public class DifficultyMilestone
	{
		public float Score;
		public float SecondsBetweenSpawns;
		public int MaxSpawnedActors; // Once this number is reached, SpawnableActors don't spawn anymore. Note: actors spawned via other means don't count towards this number.
		public ActorSpawnChance[] SpawnableActors;

		public static readonly Comparer<DifficultyMilestone> BinarySearchComparer = new RelationalComparer();
		private sealed class RelationalComparer : Comparer<DifficultyMilestone>
		{
			public override int Compare(DifficultyMilestone x, DifficultyMilestone y)
			{
				if (ReferenceEquals(x, y))
				{
					return 0;
				}
				if (ReferenceEquals(null, y))
				{
					return 1;
				}
				if (ReferenceEquals(null, x))
				{
					return -1;
				}
				return x.Score.CompareTo(y.Score);
			}
		}
	}
}
