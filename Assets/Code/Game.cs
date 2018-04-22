using UnityEngine;

namespace Astrofox
{
	public class Game : MonoBehaviour, IGameplaySessionController
	{
		[SerializeField] private GameCamera m_gameCamera;
		[SerializeField] private GameUI m_gameUI;
		[SerializeField] private GameConfig m_gameConfig;
		private GameObject m_gameplayContainer;
		private GameObjectPool m_gameObjectPool;

		private GameplayController m_gameplayController;

		private void Start()
		{
			m_gameplayContainer = new GameObject("GameplayContainer");
			m_gameObjectPool = new GameObject("GameObjectPool").AddComponent<GameObjectPool>();
			m_gameObjectPool.SetConfigs(m_gameConfig.PoolConfigs);
			GameObjectFactory gameObjectFactory = new GameObjectFactory(m_gameplayContainer.transform, m_gameObjectPool);
			Systems.GameObjectFactory = gameObjectFactory;
			Systems.GameCamera = m_gameCamera;
			Systems.GameUI = m_gameUI;
			Systems.GameplaySessionController = this;
			Systems.GameConfig = m_gameConfig;
			Systems.PlayerProfile = PlayerProfile.Load();

			m_gameUI.OpenScreen(m_gameUI.ScreenMainMenu);
		}

		public bool StartGameplaySession()
		{
			if (IsSessionInProgress())
			{
				Transform containerTransform = m_gameplayController.transform;
				for (int i = 0; i < containerTransform.childCount; ++i)
				{
					Systems.GameObjectFactory.Release(containerTransform.GetChild(i).gameObject);
				}
				Destroy(m_gameplayController);
			}
			m_gameUI.CloseAllScreens();
			m_gameplayController = m_gameplayContainer.AddComponent<GameplayController>();
			Systems.PlayerActorProvider = m_gameplayController;
			Systems.ScoreController = m_gameplayController;
			Systems.GameResultProvider = m_gameplayController;
			m_gameplayController.StartGameplay();
			m_gameUI.OpenScreen(m_gameUI.ScreenGameHud);
			Systems.ScoreController = m_gameplayController;
			return true;
		}

		public bool IsSessionInProgress()
		{
			return m_gameplayController != null;
		}
	}
}
