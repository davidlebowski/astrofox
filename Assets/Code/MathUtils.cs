using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astrofox
{
	public static class MathUtils
	{
		public static Vector3 GetClosestReachablePosition(Bounds worldBounds, Vector3 origin, Vector3 target)
		{
			Vector3 closestTarget = Vector3.zero;
			Vector2 wrappedTarget = Vector2.zero;
			// Horizontal
			wrappedTarget.x = origin.x > target.x ?
				worldBounds.max.x + target.x - worldBounds.min.x	// Approach from right edge
				:
				worldBounds.min.x + target.x - worldBounds.max.x;	// Approach from left edge
			// Vertical
			wrappedTarget.y = origin.y > target.y ?
				worldBounds.max.y + target.y - worldBounds.min.y 	// Approach from top edge
				:
				worldBounds.min.y + target.y - worldBounds.max.y;	// Approach from bottom edge
			float straightHorizontalDistance = Mathf.Abs(target.x - origin.x);
			float straightVerticalDistance = Mathf.Abs(target.y - origin.y);
			float wrappedHorizontalDistance = Mathf.Abs(wrappedTarget.x - origin.x);
			float wrappedVerticalDistance = Mathf.Abs(wrappedTarget.y - origin.y);
			closestTarget.x = straightHorizontalDistance < wrappedHorizontalDistance ? target.x : wrappedTarget.x;
			closestTarget.y = straightVerticalDistance < wrappedVerticalDistance ? target.y : wrappedTarget.y;
			closestTarget.z = target.z;
			return closestTarget;
		}
	}
}
