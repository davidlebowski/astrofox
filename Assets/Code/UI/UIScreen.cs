using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astrofox
{
	public abstract class UIScreen : MonoBehaviour
	{
		public virtual void OnShown() {}
		public virtual void OnClosed() {}
	}
}
