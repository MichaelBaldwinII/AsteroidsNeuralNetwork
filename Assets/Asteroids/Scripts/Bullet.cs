using UnityEngine;

namespace Asteroids
{
	public class Bullet : MonoBehaviour
	{
		public float lifeSpan = 1.5f;

		private void Start()
		{
			Destroy(gameObject, lifeSpan);
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			Destroy(gameObject);
		}

		//Have to do this to prevent errors when stopping play in the editor
		private void OnApplicationQuit()
		{
			gameObject.Disable();
		}
	}
}
