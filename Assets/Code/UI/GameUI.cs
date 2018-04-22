using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Astrofox
{
	[RequireComponent(typeof(Canvas))]
	public class GameUI : MonoBehaviour
	{
		// EDITOR FIELDS
		[Header("Screens")]
		[SerializeField] private UIScreen m_screenMainMenu;
		[SerializeField] private UIScreen m_screenGameHud;
		[SerializeField] private UIScreen m_screenGameOver;

		// PROPERTIES
		public UIScreen ScreenMainMenu { get { return m_screenMainMenu; } }
		public UIScreen ScreenGameHud { get { return m_screenGameHud; } }
		public UIScreen ScreenGameOver { get { return m_screenGameOver; } }

		// PRIVATE FIELDS
		private Dictionary<UIScreen, UIScreen> m_screenInstanceByPrefab = new Dictionary<UIScreen, UIScreen>();

		// PUBLIC API
		public void OpenScreen(UIScreen prefab)
		{
			if (m_screenInstanceByPrefab.ContainsKey(prefab))
			{
				Debug.LogErrorFormat("AddScreen: Screen {0} is already open.", prefab);
				return;
			}
			UIScreen screen = Instantiate(prefab, transform);
			m_screenInstanceByPrefab[prefab] = screen;
			screen.OnShown();
		}

		public void CloseAllScreens()
		{
			// We iterate over a pre-populated list to avoid an out-of-sync-enumerator exception
			List<UIScreen> screensToClose = new List<UIScreen>(m_screenInstanceByPrefab.Keys);
			foreach (UIScreen screen in screensToClose)
			{
				CloseScreen(screen);
			}
		}

		public void CloseScreen(UIScreen prefab)
		{
			UIScreen screen;
			if (!m_screenInstanceByPrefab.TryGetValue(prefab, out screen))
			{
				Debug.LogErrorFormat("AddScreen: Screen {0} is not open.", prefab);
				return;
			}
			screen.OnClosed();
			Destroy(screen.gameObject);
			m_screenInstanceByPrefab.Remove(prefab);
		}
	}
}
