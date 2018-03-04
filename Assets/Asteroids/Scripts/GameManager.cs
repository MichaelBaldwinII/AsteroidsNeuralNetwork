using System.Linq.Expressions;
using UnityEngine;

namespace Asteroids
{
	public class GameManager : MonoBehaviour
	{
		public GameObject asteroidPrefab;
		public int numOfRoidsToSpawn = 10;

		private void Awake()
		{
			for(int i = 0; i < numOfRoidsToSpawn; i++)
			{
				Instantiate(asteroidPrefab);
			}
		}
	}
}
