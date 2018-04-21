using UnityEngine;

namespace Astrofox
{
	public class Game : MonoBehaviour, IGameplaySessionController
	{
		[SerializeField] private GameCamera m_gameCamera;
		[SerializeField] private GameUI m_gameUI;
		[SerializeField] private GameConfig m_gameConfig;
		private GameObject m_gameplayContainer;

		private GameplayController m_gameplayController;

		private void Start()
		{
			m_gameplayContainer = new GameObject("GameplayContainer");
			GameObjectFactory gameObjectFactory = new GameObjectFactory(m_gameplayContainer.transform);
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
				Debug.LogError("StartGameplaySession: Gameplay session is already in progress.");
				return false;
			}
			m_gameUI.CloseAllScreens();
			m_gameplayController = m_gameplayContainer.AddComponent<GameplayController>();
			m_gameplayController.StartGameplay();
			Systems.ScoreController = m_gameplayController;
			return true;
		}

		public bool IsSessionInProgress()
		{
			return m_gameplayController != null;
		}
	}
}
