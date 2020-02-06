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
			var fitnessAmt = 0;

			switch (size)
			{
				case RoidSize.Tiny: //If tiny size, then we just disappear
					fitnessAmt = 50;
					break;
				case RoidSize.Small:
					roidSize = RoidSize.Tiny;
					fitnessAmt = 25;
					break;
				case RoidSize.Normal:
					roidSize = RoidSize.Small;
					fitnessAmt = 10;
					break;
				case RoidSize.Medium:
					roidSize = RoidSize.Normal;
					fitnessAmt = 5;
					break;
				case RoidSize.Large:
					roidSize = RoidSize.Medium;
					fitnessAmt = 3;
					break;
			}
			
			if (size != RoidSize.Tiny)
			{
				AsteroidSpawner.Instance.Spawn(transform.position, roidSize);
				AsteroidSpawner.Instance.Spawn(transform.position, roidSize);
				GenManager.Instance.AddFitness(fitnessAmt);
			}
			
			Destroy(gameObject);
		}

		//Have to do this to prevent errors when stopping play in the editor
		private void OnApplicationQuit()
		{
			gameObject.Disable();
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
