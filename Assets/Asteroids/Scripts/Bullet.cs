using UnityEngine;

namespace Asteroids
{
	public class Bullet : MonoBehaviour
	{
		public float lifeSpan = 1.5f;
		private bool isWrappingOnX;
		private bool isWrappingOnY;

		private void Start()
		{
			Destroy(gameObject, lifeSpan);
		}

		private void OnBecameVisible()
		{
			isWrappingOnX = false;
			isWrappingOnY = false;
		}

		private void OnBecameInvisible()
		{
			Vector2 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

			if((viewportPos.x < 0 || viewportPos.x > 1) && !isWrappingOnX)
			{
				transform.position = new Vector2(-transform.position.x, transform.position.y);
				isWrappingOnX = true;
			}

			if((viewportPos.y < 0 || viewportPos.y > 1) && !isWrappingOnY)
			{
				transform.position = new Vector2(transform.position.x, -transform.position.y);
				isWrappingOnY = true;
			}
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			Destroy(gameObject);
		}
	}
}
