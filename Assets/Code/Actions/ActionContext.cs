using System;
using System.Collections;
using System.Collections.Generic;
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
