using UnityEngine;

namespace Asteroids
{
	public class Asteroid : MonoBehaviour
	{
		public RoidSize size;

		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other != null && other.gameObject.name == "Ship")
				return;

			var roidSize = RoidSize.Normal;

			switch (size)
			{
				case RoidSize.Small:
					roidSize = RoidSize.Tiny;
					break;
				case RoidSize.Normal:
					roidSize = RoidSize.Small;
					break;
				case RoidSize.Medium:
					roidSize = RoidSize.Normal;
					break;
				case RoidSize.Large:
					roidSize = RoidSize.Medium;
					break;
			}
			
			if (size != RoidSize.Tiny)
			{
				AsteroidSpawner.Instance.Spawn(transform.position, roidSize);
				AsteroidSpawner.Instance.Spawn(transform.position, roidSize);
			}
			
			Destroy(gameObject);
		}

		public enum RoidSize
		{
			Tiny,
			Small,
			Normal,
			Medium,
			Large
		}
	}
}
