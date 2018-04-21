using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Astrofox
{
	public class GameplayController : MonoBehaviour, IScoreController
	{
		private PlayerController m_playerController;
		private Actor m_playerActor;

		private Coroutine m_spawnerCoroutine;
		private bool m_hasStarted;
		private int m_currentScore;
		private int m_numSpawnedActors;

		public void StartGameplay()
		{
			if (m_hasStarted)
			{
				Debug.LogError("Gameplay has already started!");
				return;
			}
			m_hasStarted = true;
			SpawnPlayer();
			m_spawnerCoroutine = StartCoroutine(SpawnerCoroutine());
		}

		private void StopGameplay()
		{
			int bestScore = Systems.PlayerProfile.BestScore;
			if (m_currentScore > bestScore)
			{
				Systems.PlayerProfile.BestScore = m_currentScore;
				Systems.PlayerProfile.Commit();
			}
			StopCoroutine(m_spawnerCoroutine);
		}

		private IEnumerator SpawnerCoroutine()
		{
			// We cache a dummy object for the binary search below
			DifficultyMilestone lastKnownMilestone = null;
			DifficultyMilestone milestoneWithScore = new DifficultyMilestone();
			DropTable<ActorSpawnChance> actorDropTable = null;
			while (true)
			{
				milestoneWithScore.Score = m_currentScore;
				int currentMilestoneIndex = Array.BinarySearch(
					Systems.GameConfig.DifficultyProgression,
					milestoneWithScore,
					DifficultyMilestone.BinarySearchComparer);
				if (currentMilestoneIndex < 0)
				{
					// If it wasn't found, Array.BinarySearch returns the complement of the index to the closest-greater
					// element that it could find. We want that element.
					currentMilestoneIndex = ~currentMilestoneIndex - 1;
				}
				DifficultyMilestone currentMilestone = Systems.GameConfig.DifficultyProgression[currentMilestoneIndex];
				if (currentMilestone != lastKnownMilestone)
				{
					// Recompute drop table if we're on a new milestone
					actorDropTable = new DropTable<ActorSpawnChance>(currentMilestone.SpawnableActors,
						i => currentMilestone.SpawnableActors[i].Chance);
				}
				lastKnownMilestone = currentMilestone;
				yield return new WaitForSeconds(currentMilestone.SecondsBetweenSpawns);
				if (m_numSpawnedActors < currentMilestone.MaxSpawnedActors && currentMilestone.MaxSpawnedActors > 0)
				{
					ActorSpawnChance drop = actorDropTable.Evaluate();
					if (drop != null && drop.Actor != null)
					{
						SpawnActor(drop.Actor);
					}
				}
			}
		}

		private void SpawnActor(Actor prefab)
		{
			// Pick the spawn location
			Vector3 spawnPoint = Systems.GameCamera.GetRandomPointOnFrustumInWorldSpace();
			Actor actorInst = Systems.GameObjectFactory.Instantiate(prefab);
			// Make sure the object appears off-screen
			spawnPoint.x += actorInst.Bounds.extents.x * Mathf.Sign(spawnPoint.x);
			spawnPoint.y += actorInst.Bounds.extents.y * Mathf.Sign(spawnPoint.y);
			actorInst.transform.position = spawnPoint;
			++m_numSpawnedActors;
			actorInst.OnDeath += actor => --m_numSpawnedActors;
		}

		private void SpawnPlayer()
		{
			if (m_playerActor != null)
			{
				// Destroy old player if present
				Destroy(m_playerActor.gameObject);
			}
			m_playerActor = Systems.GameObjectFactory.Instantiate(Systems.GameConfig.PlayerPrefab);
			m_playerActor.transform.position = Vector3.zero;
			if (m_playerController == null)
			{
				m_playerController = new PlayerController(m_playerActor);
			}
			else
			{
				m_playerController.PlayerActor = m_playerActor;
			}
		}

		private void Update()
		{
			if (!m_hasStarted)
			{
				return;
			}
			m_playerController.Update();
		}

		public void AddScore(int amount)
		{
			m_currentScore += amount;
		}
	}
}
