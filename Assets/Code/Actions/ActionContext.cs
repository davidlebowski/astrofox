using System;
using UnityEngine;

namespace Astrofox
{
	[Serializable]
	public struct ActionContext
	{
		public GameObject ThisGameObject;
		public GameObject OtherGameObject;
	}
}
