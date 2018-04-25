using UnityEngine;

namespace Astrofox
{
	[CreateAssetMenu]
	public class ActionSpawnGameObjects : BaseAction
	{
		[SerializeField] private GameObject[] m_prefabs;

		public override void Run(ActionContext context)
		{
			foreach (GameObject prefab in m_prefabs)
			{
				GameObject inst = Systems.GameObjectFactory.Instantiate(prefab);
				inst.transform.position = context.ThisGameObject.transform.position;
			}
		}
	}
}
