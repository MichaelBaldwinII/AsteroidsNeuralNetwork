using UnityEngine;
using UnityEngine.UI;

namespace Asteroids
{
	public class ScoreManager : MonoBehaviour
	{
		public float currentScore;
		public bool startIncrement;
		public Text scoreText;

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

			scoreText.text = "Score: " + currentScore;
		}
	}
}
