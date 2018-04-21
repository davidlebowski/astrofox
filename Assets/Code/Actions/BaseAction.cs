using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astrofox
{
	public abstract class BaseAction : ScriptableObject
	{
		public abstract void Run(ActionContext context);
	}
}
