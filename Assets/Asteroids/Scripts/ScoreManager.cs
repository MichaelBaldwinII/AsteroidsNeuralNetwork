using Baldwin;
using UnityEngine;

namespace Asteroids
{
	public class ScoreManager : Singleton<ScoreManager>
	{
		public float currentScore;
		public bool startIncrement;

		public void StartScore()
		{
			currentScore = 0;
			startIncrement = true;
		}

		public void StopScore()
		{
			Debug.Log("Score: " + currentScore);
			startIncrement = false;
		}

		private void Update()
		{
			if(startIncrement)
			{
				currentScore += Time.deltaTime;
			}
		}

		public void OnShipCollision()
		{
			StopScore();
		}
	}
}
