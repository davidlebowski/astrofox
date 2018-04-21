using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astrofox
{
	[RequireComponent(typeof(Actor))]
	public class DeathTrigger : MonoBehaviour
	{
		[SerializeField] private BaseAction[] m_actions;

		private void Awake()
		{
			Actor actor = GetComponent<Actor>();
			actor.OnDeath += OnDeath;
		}

		private void OnDeath(Actor deadActor)
		{
			ActionContext context = new ActionContext
			{
				ThisGameObject = gameObject
			};
			foreach (BaseAction action in m_actions)
			{
				action.Run(context);
			}
		}
	}
}
