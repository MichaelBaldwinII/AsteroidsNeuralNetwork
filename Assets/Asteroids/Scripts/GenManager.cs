using System.Collections.Generic;
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

		private void Start()
		{
			currentGenNumber = 0;
			index = 0;
			currentRunText = new GameObject("GenRun#" + index, typeof(Text)).GetOrAddComponent<Text>();
			currentRunText.SetParent(scrollViewContent);

			//Populate the first generation
			for(int i = 0; i < numPerGen; i++)
			{
				neuralNetworks.Add(new NeuralNetwork(numOfInputs, numOfHiddenLayers, numOfNodesPerHiddenLayer, numOfOutputs));
			}
			ScoreManager.Instance.StartScore();
		}

		private void Update()
		{
			neuralNetworks[index].fitness = ScoreManager.Instance.currentScore;
			comPlayer.brain = neuralNetworks[index];

			currentRunText.text = index + ": " + neuralNetworks[index].fitness;
			genText.text = "Gen: " + currentGenNumber;
		}

		public void Next()
		{
			AsteroidSpawner.Instance.Restart();
			ScoreManager.Instance.StartScore();
			index++;

			if(index >= neuralNetworks.Count)
			{
				//Debug.Log("we have reached the end of the first gen.");
				neuralNetworks.Sort();
				neuralNetworks.Reverse();
				NeuralNetwork winner = neuralNetworks[0];
				//Pop new gen using old gen winner
				neuralNetworks.Clear();
				for(int i = 0; i < numPerGen; i++)
				{
					neuralNetworks.Add(new NeuralNetwork(winner));
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
			ScoreManager.Instance.StopScore();
			Next();
		}
	}
}
