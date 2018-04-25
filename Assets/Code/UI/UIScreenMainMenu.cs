using UnityEngine;

namespace Astrofox
{
	public class UIScreenMainMenu : UIScreen
	{
		private void Update()
		{
			if (!Systems.GameplaySessionController.IsSessionInProgress() && Input.GetMouseButtonDown(0))
			{
				Systems.GameplaySessionController.StartGameplaySession();
			}
		}
	}
}
