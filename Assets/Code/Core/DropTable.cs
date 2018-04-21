using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

// Utility class that stores a normalized drop table for each item, and provides an interface for evaluating random items.
namespace Astrofox
{
	public sealed class DropTable<T>
	{
		public class TableEntry
		{
			public T item;
			public int index;
			public float cumulativeChance;
#if UNITY_EDITOR
			public float normalisedChance;
#endif
		}

		private List<TableEntry> sortedEntries;

		public DropTable(IEnumerable<T> sourceItems, Func<int, float> chanceGetter)
		{
			sortedEntries = new List<TableEntry>();
			int index = 0;
			foreach (T sourceItem in sourceItems)
			{
				sortedEntries.Add(new TableEntry {index = index, item = sourceItem});
				++index;
			}
			sortedEntries.Sort((l, r) => (int) (chanceGetter(l.index) - chanceGetter(r.index)));

			float prevChance = 0;
			for (int i = 0; i < sortedEntries.Count; ++i)
			{
				var currentEntry = sortedEntries[i];
				var chance = chanceGetter(currentEntry.index);
				currentEntry.cumulativeChance = prevChance = chance + prevChance;
			}
#if UNITY_EDITOR
			float totalChance = sortedEntries[sortedEntries.Count - 1].cumulativeChance;
			for (int i = 0; i < sortedEntries.Count; ++i)
			{
				var currentEntry = sortedEntries[i];
				prevChance = i > 0 ? sortedEntries[i - 1].cumulativeChance : 0;
				currentEntry.normalisedChance = (currentEntry.cumulativeChance - prevChance) / totalChance;
			}
#endif
		}

		public T Evaluate()
		{
			float dropValue = Random.Range(0, sortedEntries[sortedEntries.Count - 1].cumulativeChance);
			foreach (var entry in sortedEntries)
			{
				if (dropValue <= entry.cumulativeChance)
				{
					return entry.item;
				}
			}
			return sortedEntries[sortedEntries.Count - 1].item;
		}

		public void ForEachEntry(System.Action<TableEntry> callback)
		{
			foreach (var entry in sortedEntries)
			{
				callback(entry);
			}
		}
	}
}
