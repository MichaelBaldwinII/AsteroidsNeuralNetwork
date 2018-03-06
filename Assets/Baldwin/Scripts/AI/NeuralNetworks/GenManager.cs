using System.Collections.Generic;
using System.Linq;
using Asteroids;
using UnityEngine;
using UnityEngine.UI;

namespace Baldwin.AI
{
	public class GenManager : Singleton<GenManager>
	{
		[Header("Neural Network Config")]
		public int numOfInputs = 16;
		public int numOfNodesPerHiddenLayer = 16;
		public int numOfOutputs = 4;
		public float mutationPercent = 0.05f;
		public ComputerPlayer comPlayer;

		[Header("Gen Control")]
		public int numPerGen = 10;
		public float timeBetweenInadequencyChecks = 5.0f;
		private float currentScore;
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

		public void AddFitness(float amount)
		{
			currentScore += amount;
		}

		private void Start()
		{
			currentGenNumber = 1;
			index = 0;
			currentRunText = Instantiate(currentGenTextPrefab, scrollViewContent.transform).GetOrAddComponent<Text>();
			lastTimeCheck = Time.time;

			//Populate the first generation
			for(int i = 0; i < numPerGen; i++)
			{
				neuralNetworks.Add(new NeuralNetwork(numOfInputs, numOfNodesPerHiddenLayer, numOfOutputs));
			}

			currentScore = 0;
		}

		private void Update()
		{
			if(Time.time - lastTimeCheck > timeBetweenInadequencyChecks)
			{
				if(Mathf.Approximately(currentScore, lastScore))
				{
					//Ship is just sitting still we need to remove it
					Next();
				}
				lastTimeCheck = Time.time;
				lastScore = 0;
			}

			neuralNetworks[index].fitness = currentScore;
			comPlayer.brain = neuralNetworks[index];

			currentRunText.text = (index + 1) + ": " + neuralNetworks[index].fitness;
			genText.text = "Gen: " + currentGenNumber;
		}

		public void Next()
		{
			AsteroidSpawner.Instance.Restart();
			currentScore = 0;
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
				if(lastGenWinner != null)
				{
					NeuralNetwork.SaveToFile(lastGenWinner, "Gen_" + currentGenNumber + "_Winner_" + lastGenWinner.fitness);
				}
				lastGenWinner = neuralNetworks[0];
				//Pop new gen using old gen winner
				neuralNetworks.Clear();
				for(int i = 0; i < numPerGen; i++)
				{
					neuralNetworks.Add(new NeuralNetwork(lastGenWinner));
					neuralNetworks[i].Mutate(mutationPercent);
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
			NeuralNetwork.SaveToFile(lastGenWinner, "LatestWinner_" + lastGenWinner.fitness);
		}
	}
}
