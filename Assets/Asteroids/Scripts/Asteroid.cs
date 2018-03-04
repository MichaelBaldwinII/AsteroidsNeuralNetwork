using UnityEngine;

namespace Asteroids
{
	public class Asteroid : MonoBehaviour
	{
		private bool isWrappingOnX;
		private bool isWrappingOnY;

		private void Start()
		{
			Vector3 startPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.value, Random.value));
			transform.position = startPos;
			GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * 50);
			GetComponent<Rigidbody2D>().AddTorque(3);
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

		private void OnBecameVisible()
		{
			isWrappingOnX = false;
			isWrappingOnY = false;
		}
	}
}
