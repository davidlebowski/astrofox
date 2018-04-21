using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Astrofox
{
	[RequireComponent(typeof(Actor))]
	public class TimeTrigger : MonoBehaviour
	{
		[SerializeField] private float m_secondsUntilTrigger;
		[SerializeField] private BaseAction[] m_actions;

		private Coroutine m_coroutine;

		private void OnEnable()
		{
			m_coroutine = StartCoroutine(TriggerAfterDelay());
		}

		private void OnDisable()
		{
			StopCoroutine(m_coroutine);
		}

		private IEnumerator TriggerAfterDelay()
		{
			yield return new WaitForSeconds(m_secondsUntilTrigger);
			foreach (BaseAction action in m_actions)
			{
				action.Run(new ActionContext
				{
					ThisGameObject = gameObject
				});
			}
		}
	}
}
