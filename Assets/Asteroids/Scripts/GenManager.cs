using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids
{
	public class GenManager : MonoBehaviour
	{
		public int genNum;
		public int numPerGen = 10;
		public List<NeuralNetwork> currentRun = new List<NeuralNetwork>();
		public Text genText;
		public List<Text> fitnessTexts = new List<Text>();
		public int index;
		public Ship ship;
		public bool pause;

		private void Start()
		{
			//Populate the first generation
			for(int i = 0; i < numPerGen; i++)
			{
				currentRun.Add(new NeuralNetwork());
			}
			FindObjectOfType<ScoreManager>().StartScore();
		}

		private void Update()
		{
			if(pause)
			{
				return;
			}

			//There are 16 inputs, the 8 directions and the 8 distances to an asteroid in those directions.
			//First enter the inputs
			List<RaycastHit2D> hits = new List<RaycastHit2D>(8)
			{
				Physics2D.Raycast(ship.transform.position, ship.transform.up, float.MaxValue, LayerMask.NameToLayer("Asteroids")),
				Physics2D.Raycast(ship.transform.position, (ship.transform.up + ship.transform.right).normalized, float.MaxValue, LayerMask.NameToLayer("Asteroids")),
				Physics2D.Raycast(ship.transform.position, ship.transform.right, float.MaxValue, LayerMask.NameToLayer("Asteroids")),
				Physics2D.Raycast(ship.transform.position, (-ship.transform.up + ship.transform.right).normalized, float.MaxValue, LayerMask.NameToLayer("Asteroids")),
				Physics2D.Raycast(ship.transform.position, -ship.transform.up, float.MaxValue, LayerMask.NameToLayer("Asteroids")),
				Physics2D.Raycast(ship.transform.position, (-ship.transform.up + -ship.transform.right).normalized, float.MaxValue, LayerMask.NameToLayer("Asteroids")),
				Physics2D.Raycast(ship.transform.position, -ship.transform.right, float.MaxValue, LayerMask.NameToLayer("Asteroids")),
				Physics2D.Raycast(ship.transform.position, (ship.transform.up + -ship.transform.right).normalized, float.MaxValue, LayerMask.NameToLayer("Asteroids"))
			};

			//Input the collision detected inputs
			List<float> inputValues = new List<float>(16)
			{
				hits[0].collider != null ? 1.0f : 0.0f, hits[0].distance,
				hits[1].collider != null ? 1.0f : 0.0f, hits[1].distance,
				hits[2].collider != null ? 1.0f : 0.0f, hits[2].distance,
				hits[3].collider != null ? 1.0f : 0.0f, hits[3].distance,
				hits[4].collider != null ? 1.0f : 0.0f, hits[4].distance,
				hits[5].collider != null ? 1.0f : 0.0f, hits[5].distance,
				hits[6].collider != null ? 1.0f : 0.0f, hits[6].distance,
				hits[7].collider != null ? 1.0f : 0.0f, hits[7].distance
			};

			List<float> outputs = currentRun[index].Process(inputValues);

			//And finally we use the outputLayer to send input to the ship
			if(outputs[0] > 0.5f)
			{
				ship.Thrust();
			}
			if(outputs[1] > 0.5f)
			{
				ship.Rotate(true);
			}
			if(outputs[2] > 0.5f)
			{
				ship.Rotate(false);
			}
			if(outputs[3] > 0.5f)
			{
				ship.Fire();
			}

			currentRun[index].fitness = FindObjectOfType<ScoreManager>().currentScore;
			fitnessTexts[0].text = "Run #0: " + currentRun[0].fitness;
			fitnessTexts[1].text = "Run #1: " + currentRun[1].fitness;
			fitnessTexts[2].text = "Run #2: " + currentRun[2].fitness;
			fitnessTexts[3].text = "Run #3: " + currentRun[3].fitness;
			fitnessTexts[4].text = "Run #4: " + currentRun[4].fitness;
			fitnessTexts[5].text = "Run #5: " + currentRun[5].fitness;
			fitnessTexts[6].text = "Run #6: " + currentRun[6].fitness;
			fitnessTexts[7].text = "Run #7: " + currentRun[7].fitness;
			fitnessTexts[8].text = "Run #8: " + currentRun[8].fitness;
			fitnessTexts[9].text = "Run #9: " + currentRun[9].fitness;
		}

		public void Next()
		{
			FindObjectOfType<AsteroidSpawner>().Restart();
			FindObjectOfType<ScoreManager>().StartScore();
			index++;
			ship.ResetPos();

			if(index >= currentRun.Count)
			{
				Debug.Log("we have reached the end of the first gen.");
				List<NeuralNetwork> sortedNN = currentRun.OrderBy(f => f.fitness).ToList();
				NeuralNetwork winner = sortedNN[0];
				//Pop new gen using old gen winner
				currentRun.Clear();
				for(int i = 0; i < numPerGen; i++)
				{
					currentRun.Add(new NeuralNetwork(winner));
				}
				FindObjectOfType<ScoreManager>().StartScore();
				index = 0;
				genNum++;
				genText.text = "Gen: " + genNum;
			}
		}

		public void OnShipCollision()
		{
			if(pause)
			{
				return;
			}

			currentRun[index].fitness = FindObjectOfType<ScoreManager>().currentScore;
			FindObjectOfType<ScoreManager>().StopScore();
			Next();
		}
	}
}
