using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astrofox
{
	[RequireComponent(typeof(Actor))]
	public class CollisionTrigger : MonoBehaviour
	{
		[SerializeField] private BaseAction m_action;

		private void OnTriggerEnter(Collider other)
		{
			m_action.Run(new ActionContext
			{
				ThisGameObject = gameObject,
				OtherGameObject = other.gameObject
			});
		}
	}
}
