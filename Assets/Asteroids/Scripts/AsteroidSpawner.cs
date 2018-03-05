using UnityEngine;

namespace Asteroids
{
	public class AsteroidSpawner : MonoBehaviour
	{
		public GameObject asteroidPrefab;
		public int numOfRoidsToSpawn = 10;
		public float largeRoidScale = 3.0f;
		public float mediumRoidScale = 2.0f;
		public float normalRoidScale = 1.0f;
		public float smallRoidScale = 0.5f;
		public float tinyRoidScale = 0.25f;

		private void Awake()
		{
			for(int i = 0; i < numOfRoidsToSpawn; i++)
			{
				Vector3 startPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.value, Random.value));
				Spawn(startPos, AsteroidSize.LARGE);
			}
		}

		public void Spawn(Vector2 pos, AsteroidSize size)
		{
			Asteroid asteroid = Instantiate(asteroidPrefab, pos, Quaternion.identity).GetComponent<Asteroid>();
			asteroid.size = size;
			asteroid.transform.localScale = Vector3.one * GetScaleFromSize(size);
			asteroid.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * 50);
			asteroid.GetComponent<Rigidbody2D>().AddTorque(3);
		}

		public float GetScaleFromSize(AsteroidSize size)
		{
			switch(size)
			{
				case AsteroidSize.LARGE:
					return largeRoidScale;
				case AsteroidSize.MEDIUM:
					return mediumRoidScale;
				case AsteroidSize.NORMAL:
					return normalRoidScale;
				case AsteroidSize.SMALL:
					return smallRoidScale;
				case AsteroidSize.TINY:
					return tinyRoidScale;
			}

			return normalRoidScale;
		}
	}
}
