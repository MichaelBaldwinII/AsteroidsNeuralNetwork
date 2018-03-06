using UnityEngine;

namespace Asteroids
{
	public class Asteroid : MonoBehaviour
	{
		public AsteroidSize size;

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
			if(other != null && other.gameObject.name == "Ship")
			{
				return;
			}

			switch(size)
			{
				case AsteroidSize.LARGE:
					AsteroidSpawner.Instance.Spawn(transform.position, AsteroidSize.MEDIUM);
					AsteroidSpawner.Instance.Spawn(transform.position, AsteroidSize.MEDIUM);
					Destroy(gameObject);
					ScoreManager.Instance.currentScore += 10;
					break;
				case AsteroidSize.MEDIUM:
					AsteroidSpawner.Instance.Spawn(transform.position, AsteroidSize.NORMAL);
					AsteroidSpawner.Instance.Spawn(transform.position, AsteroidSize.NORMAL);
					Destroy(gameObject);
					ScoreManager.Instance.currentScore += 20;
					break;
				case AsteroidSize.NORMAL:
					AsteroidSpawner.Instance.Spawn(transform.position, AsteroidSize.SMALL);
					AsteroidSpawner.Instance.Spawn(transform.position, AsteroidSize.SMALL);
					Destroy(gameObject);
					ScoreManager.Instance.currentScore += 30;
					break;
				case AsteroidSize.SMALL:
					AsteroidSpawner.Instance.Spawn(transform.position, AsteroidSize.TINY);
					AsteroidSpawner.Instance.Spawn(transform.position, AsteroidSize.TINY);
					Destroy(gameObject);
					ScoreManager.Instance.currentScore += 40;
					break;
				case AsteroidSize.TINY: //If tiny size, then we just disappear
					Destroy(gameObject);
					ScoreManager.Instance.currentScore += 50;
					break;
			}
		}
	}
}
