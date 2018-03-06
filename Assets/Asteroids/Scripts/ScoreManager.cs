using Baldwin;
using UnityEngine;

namespace Asteroids
{
	public class ScoreManager : Singleton<ScoreManager>
	{
		public float currentScore;

		public void RestartScore()
		{
			currentScore = 0;
		}
	}
}
