using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astrofox
{
	[CreateAssetMenu]
	public class ActionRandomForces : BaseAction
	{
		[SerializeField] private float m_maxMagnitude;
		[SerializeField] private float m_minMagnitude;

		public override void Run(ActionContext context)
		{
			Actor actor = context.ThisGameObject.GetComponent<Actor>();
			if (actor != null)
			{
				Vector3 force = Random.insideUnitCircle * Random.Range(m_minMagnitude, m_maxMagnitude);
				actor.AddForce(force);
			}
		}
	}
}
