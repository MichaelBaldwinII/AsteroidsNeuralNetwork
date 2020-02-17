using System.Linq;
using UnityEngine;

namespace Asteroids
{
	public class AsteroidSpawner : Singleton<AsteroidSpawner>
	{
		[Header("Config")]
		public GameObject asteroidPrefab;
		public int numOfRoidsToSpawn = 10;
		public int minLargeRoids = 3;
		public float largeRoidScale = 3.0f;
		public float mediumRoidScale = 2.0f;
		public float normalRoidScale = 1.0f;
		public float smallRoidScale = 0.5f;
		public float tinyRoidScale = 0.25f;

		public void SetMinLargeRoids(string value)
		{
			minLargeRoids = int.Parse(value);
		}

		public void SetInitialRoidCount(string value)
		{
			numOfRoidsToSpawn = int.Parse(value);
		}

		private void Update()
		{
			var allRoids = FindObjectsOfType<Asteroid>().ToList();
			var largeRoidCount = 0;
			foreach (var iRoid in allRoids)
				if (iRoid.size == Asteroid.RoidSize.Large)
					largeRoidCount++;

			if (largeRoidCount < minLargeRoids)
				for (var i = 0; i < minLargeRoids - largeRoidCount; i++)
					Spawn();
		}

		public void ClearAll()
		{
			foreach (var iRoid in FindObjectsOfType<Asteroid>())
				Destroy(iRoid.gameObject);
		}

		public void Restart()
		{
			//Clear all the bullets in the game. TODO: this should be in a different class, less mom's sphagetti code
			foreach (var iBullet in FindObjectsOfType<Bullet>())
				Destroy(iBullet.gameObject);

			foreach (var iRoid in FindObjectsOfType<Asteroid>())
				Destroy(iRoid.gameObject);

			//Spawn the first one towards the ship
			var startPos = Camera.main.ViewportToWorldPoint(Extensions.OutsideOfUnitBox());
			Spawn(startPos, (FindObjectOfType<Ship>().transform.position - startPos).normalized * 50f, Asteroid.RoidSize.Large);

			for (var i = 1; i < numOfRoidsToSpawn; i++)
				Spawn();
		}

		public void Spawn()
		{
			var startPos = Camera.main.ViewportToWorldPoint(Extensions.OutsideOfUnitBox());
			Spawn(startPos, Asteroid.RoidSize.Large);
		}

		public void Spawn(Vector2 pos, Asteroid.RoidSize size)
		{
			Spawn(pos, Random.insideUnitCircle.normalized * 50f, size);
		}

		public void Spawn(Vector2 pos, Vector2 dir, Asteroid.RoidSize size)
		{
			var asteroid = Instantiate(asteroidPrefab, pos, Quaternion.identity).GetComponent<Asteroid>();
			asteroid.size = size;
			asteroid.transform.localScale = Vector3.one * GetScaleFromSize(size);
			asteroid.GetComponent<Rigidbody2D>().AddForce(dir);
			asteroid.GetComponent<Rigidbody2D>().AddTorque(3);
		}

		public float GetScaleFromSize(Asteroid.RoidSize size)
		{
			switch (size)
			{
				case Asteroid.RoidSize.Large:
					return largeRoidScale;
				case Asteroid.RoidSize.Medium:
					return mediumRoidScale;
				case Asteroid.RoidSize.Normal:
					return normalRoidScale;
				case Asteroid.RoidSize.Small:
					return smallRoidScale;
				case Asteroid.RoidSize.Tiny:
					return tinyRoidScale;
			}

			return normalRoidScale;
		}
	}
}
