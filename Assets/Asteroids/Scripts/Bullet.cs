using Baldwin;
using UnityEngine;

namespace Asteroids
{
	public class Bullet : MonoBehaviour, Pauseable
	{
		public float lifeSpan = 1.5f;
		private Vector2 storedVelocity;
		private float storedAngularVelocity;

		private void Start()
		{
			Destroy(gameObject, lifeSpan);
		}

		private void OnBecameInvisible()
		{
			Vector2 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

			if(viewportPos.x < 0 || viewportPos.x > 1)
			{
				transform.position = new Vector2(-transform.position.x, transform.position.y);
			}

			if(viewportPos.y < 0 || viewportPos.y > 1)
			{
				transform.position = new Vector2(transform.position.x, -transform.position.y);
			}
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			Destroy(gameObject);
		}

		#region Pauseable

		public void OnPause()
		{
			storedVelocity = GetComponent<Rigidbody2D>().velocity;
			storedAngularVelocity = GetComponent<Rigidbody2D>().angularVelocity;
			GetComponent<Rigidbody2D>().simulated = false;
		}

		public void OnUnpause()
		{
			GetComponent<Rigidbody2D>().simulated = true;
			GetComponent<Rigidbody2D>().velocity = storedVelocity;
			GetComponent<Rigidbody2D>().angularVelocity = storedAngularVelocity;
		}

		#endregion
	}
}
