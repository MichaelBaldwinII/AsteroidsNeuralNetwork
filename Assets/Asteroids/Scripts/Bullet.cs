using Baldwin;
using UnityEngine;

namespace Asteroids
{
	public class Bullet : MonoBehaviour, Pauseable
	{
		public float lifeSpan = 1.5f;
		private Vector2 storedVelocity;
		private float storedAngularVelocity;
		private float timeAtCreation;
		private float timeAtPause;

		private void Start()
		{
			timeAtCreation = Time.time;
			Invoke(nameof(DestoryMeInvokable), lifeSpan);
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
			DestoryMeInvokable();
		}

		public void DestoryMeInvokable()
		{
			Destroy(gameObject);
		}

		//Have to do this to prevent errors when stopping play in the editor
		private void OnApplicationQuit()
		{
			gameObject.Disable();
		}

		#region Pauseable

		public void OnPause()
		{
			storedVelocity = GetComponent<Rigidbody2D>().velocity;
			storedAngularVelocity = GetComponent<Rigidbody2D>().angularVelocity;
			GetComponent<Rigidbody2D>().simulated = false;
			timeAtPause = Time.time;
			CancelInvoke(nameof(DestoryMeInvokable));
		}

		public void OnUnpause()
		{
			GetComponent<Rigidbody2D>().simulated = true;
			GetComponent<Rigidbody2D>().velocity = storedVelocity;
			GetComponent<Rigidbody2D>().angularVelocity = storedAngularVelocity;
			Invoke(nameof(DestoryMeInvokable), 1.5f - timeAtPause - timeAtCreation);
		}

		#endregion
	}
}
