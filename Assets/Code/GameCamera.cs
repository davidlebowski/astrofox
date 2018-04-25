using UnityEngine;

namespace Astrofox
{
	[RequireComponent(typeof(Camera))]
	public class GameCamera : MonoBehaviour
	{
		public Bounds WorldBounds { private set; get; }

		private Camera m_camera;

		private void Awake()
		{
			m_camera = GetComponent<Camera>();
			CalculateWorldBounds();
		}

		private void CalculateWorldBounds()
		{
			Vector3 topRight = m_camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
			Vector3 bottomLeft = m_camera.ViewportToWorldPoint(Vector3.zero);
			WorldBounds = new Bounds(Vector3.zero, new Vector3
			{
				x = Mathf.Abs(topRight.x - bottomLeft.x),
				y = Mathf.Abs(topRight.y - bottomLeft.y),
				z = 1
			});
		}

		private void Update()
		{
			CalculateWorldBounds();
		}

		public Vector3 GetRandomPointOnFrustumInWorldSpace()
		{
			Vector3 viewportPoint = Vector3.zero;
			int frustumEdge = Random.Range(0, 4);
			switch (frustumEdge)
			{
				case 0: // Top
					viewportPoint.x = Random.Range(0f, 1f);
					viewportPoint.y = 1;
					break;
				case 1: // Bottom
					viewportPoint.x = Random.Range(0f, 1f);
					viewportPoint.y = 0;
					break;
				case 2: // Left
					viewportPoint.x = 0;
					viewportPoint.y = Random.Range(0f, 1f);
					break;
				case 3: // Right
					viewportPoint.x = 1;
					viewportPoint.y = Random.Range(0f, 1f);
					break;
			}
			Vector3 worldPoint = m_camera.ViewportToWorldPoint(viewportPoint);
			worldPoint.z = 0;
			return worldPoint;
		}
	}
}
