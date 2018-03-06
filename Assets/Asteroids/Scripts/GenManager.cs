using System.Collections.Generic;
using System.Linq;
using Baldwin;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids
{
	public class GenManager : Singleton<GenManager>
	{
		[Header("Neural Network Config")]
		public int numOfInputs = 16;
		public int numOfHiddenLayers = 2;
		public int numOfNodesPerHiddenLayer = 16;
		public int numOfOutputs = 4;
		public ComputerPlayer comPlayer;

		[Header("Gen Control")]
		public int numPerGen = 10;
		private int currentGenNumber;
		private readonly List<NeuralNetwork> neuralNetworks = new List<NeuralNetwork>();

		[Header("Rendering")]
		public Text genText;
		public GameObject currentGenTextPrefab;
		public GameObject scrollViewContent;
		private Text currentRunText;
		private int index;
		private NeuralNetwork lastGenWinner;

		private float lastTimeCheck;
		private float lastScore;

		private void Start()
		{
			currentGenNumber = 1;
			index = 0;
			currentRunText = Instantiate(currentGenTextPrefab, scrollViewContent.transform).GetOrAddComponent<Text>();
			lastTimeCheck = Time.time;

			//Populate the first generation
			for(int i = 0; i < numPerGen; i++)
			{
				neuralNetworks.Add(new NeuralNetwork(numOfInputs, numOfHiddenLayers, numOfNodesPerHiddenLayer, numOfOutputs));
			}
			ScoreManager.Instance.RestartScore();
		}

		private void Update()
		{
			if(Time.time - lastTimeCheck > 5f)
			{
				if(Mathf.Approximately(ScoreManager.Instance.currentScore, lastScore))
				{
					//Ship is just sitting still we need to remove it
					Next();
				}
				lastTimeCheck = Time.time;
				lastScore = 0;
			}

			neuralNetworks[index].fitness = ScoreManager.Instance.currentScore;
			comPlayer.brain = neuralNetworks[index];

			currentRunText.text = (index + 1) + ": " + neuralNetworks[index].fitness;
			genText.text = "Gen: " + currentGenNumber;
		}

		public void Next()
		{
			AsteroidSpawner.Instance.Restart();
			ScoreManager.Instance.RestartScore();
			index++;
			FindObjectOfType<Ship>().transform.position = Vector3.zero;
			lastTimeCheck = Time.time;
			lastScore = 0;
			List<Bullet> allBullets = FindObjectsOfType<Bullet>().ToList();
			foreach(Bullet iBullet in allBullets)
			{
				Destroy(iBullet.gameObject);
			}
			allBullets.Clear();

			if(index >= neuralNetworks.Count)
			{
				//Debug.Log("we have reached the end of the first gen.");
				neuralNetworks.Sort();
				neuralNetworks.Reverse();
				lastGenWinner.SaveToFile("Gen" + currentGenNumber + "Winner_F:" + lastGenWinner.fitness);
				lastGenWinner = neuralNetworks[0];
				//Pop new gen using old gen winner
				neuralNetworks.Clear();
				for(int i = 0; i < numPerGen; i++)
				{
					neuralNetworks.Add(new NeuralNetwork(lastGenWinner));
				}
				index = 0;
				currentGenNumber++;
				foreach(Transform iChild in scrollViewContent.transform)
				{
					Destroy(iChild.gameObject);
				}
			}

			currentRunText = Instantiate(currentGenTextPrefab, scrollViewContent.transform).GetOrAddComponent<Text>();
		}

		public void OnShipCollision()
		{
			Next();
		}

		public void SaveLatestWinner()
		{
			lastGenWinner.SaveToFile("LatestWinner:" + lastGenWinner.fitness);
		}
	}
}
