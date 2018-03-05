using UnityEngine;

namespace Asteroids
{
	public class Asteroid : MonoBehaviour
	{
		public AsteroidSize size;
		private bool isWrappingOnX;
		private bool isWrappingOnY;
		private AsteroidSpawner spawner;
		private ScoreManager scoreManager;

		private void Awake()
		{
			spawner = FindObjectOfType<AsteroidSpawner>();
			scoreManager = FindObjectOfType<ScoreManager>();
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

		private void OnCollisionEnter2D(Collision2D other)
		{
			//We split into two smaller asteroids of the next size down
			switch(size)
			{
				case AsteroidSize.LARGE:
					spawner.Spawn(transform.position, AsteroidSize.MEDIUM);
					spawner.Spawn(transform.position, AsteroidSize.MEDIUM);
					Destroy(gameObject);
					scoreManager.currentScore += 50;
					break;
				case AsteroidSize.MEDIUM:
					spawner.Spawn(transform.position, AsteroidSize.NORMAL);
					spawner.Spawn(transform.position, AsteroidSize.NORMAL);
					Destroy(gameObject);
					scoreManager.currentScore += 40;
					break;
				case AsteroidSize.NORMAL:
					spawner.Spawn(transform.position, AsteroidSize.SMALL);
					spawner.Spawn(transform.position, AsteroidSize.SMALL);
					Destroy(gameObject);
					scoreManager.currentScore += 30;
					break;
				case AsteroidSize.SMALL:
					spawner.Spawn(transform.position, AsteroidSize.TINY);
					spawner.Spawn(transform.position, AsteroidSize.TINY);
					Destroy(gameObject);
					scoreManager.currentScore += 20;
					break;
				case AsteroidSize.TINY: //If tiny size, then we just disappear
					Destroy(gameObject);
					scoreManager.currentScore += 10;
					break;
			}
		}
	}
}
