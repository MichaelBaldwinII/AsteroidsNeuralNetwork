using UnityEngine;

namespace Asteroids
{
	public class WarpOffScreen : MonoBehaviour
	{
		private Transform transformRef;

		private void Awake()
		{
			transformRef = transform;
		}

		//Have to do this to prevent errors when stopping play in the editor
		private void OnApplicationQuit()
		{
			gameObject.Disable();
		}
		
		private void OnBecameInvisible()
		{
			Vector2 viewportPos = Camera.main.WorldToViewportPoint(transformRef.position);

			if (viewportPos.x < 0 || viewportPos.x > 1)
				transformRef.position = new Vector2(-transformRef.position.x, transformRef.position.y);
			if (viewportPos.y < 0 || viewportPos.y > 1)
				transformRef.position = new Vector2(transformRef.position.x, -transformRef.position.y);
		}
	}
}
