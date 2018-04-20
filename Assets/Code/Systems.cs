using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Astrofox;
using UnityEngine;

namespace Astrofox
{
	// A hybrid between dependency injection and singleton pattern.
	// It felt a bit overkill integrating a DI framework (e.g.: Zenject) just for this, so I decided to go for something
	// in between. All singleton classes are passed to this Systems class, which is initialized by the Game class.
	public static class Systems
	{
		public static GameObjectFactory GameObjectFactory;
		public static GameCamera GameCamera;
		public static GameUI GameUI;
		public static GameConfig GameConfig;
		public static IGameplaySessionController GameplaySessionController;
	}
}
