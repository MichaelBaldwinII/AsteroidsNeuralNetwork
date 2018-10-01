using System.Collections.Generic;
using Asteroids;
using UnityEngine;
using UnityEngine.Events;

namespace Baldwin.AI
{
	public class GenManager : Singleton<GenManager>
	{
		[Header("Neural Network Config")]
		public int numOfInputs = 16;
		public int numOfHiddenMatrices = 2;
		public int numOfHiddenNodes = 16;
		public int numOfOutputs = 4;
		public float mutationPercent = 0.05f;
		public ComputerPlayer comPlayer;

		[Header("Gen Control")]
		public int numPerGen = 10;
		public NeuralNetwork CurrentNN { get; private set; }
		public float CurrentFitness { get; private set; }
		public int CurrentGenNumber { get; private set; }
		public int Index { get; private set; }
		private List<NeuralNetwork> neuralNetworks = new List<NeuralNetwork>();

		[Header("Events")]
		public UnityEvent onNextNetworkEvent;
		public UnityEvent onNextGenerationEvent;

		public void AddFitness(float amount)
		{
			CurrentFitness += amount;
		}

		public void SetNumOfInputs(string value)
		{
			numOfInputs = int.Parse(value);
		}

		public void SetNumOfHiddenMatrices(string value)
		{
			numOfHiddenMatrices = int.Parse(value);
		}

		public void SetMutationChance(string value)
		{
			mutationPercent = int.Parse(value) / 100.0f;
		}

		public void SetNumPerGen(string value)
		{
			numPerGen = int.Parse(value);
		}

		private void Start()
		{
			CurrentGenNumber = 1;
			Index = 0;
			CurrentFitness = 0;

			//Populate the first generation
			for(var i = 0; i < numPerGen; i++)
			{
				neuralNetworks.Add(new NeuralNetwork(numOfInputs, numOfHiddenMatrices, numOfHiddenNodes, numOfOutputs));
			}

			CurrentNN = neuralNetworks[0];
			comPlayer.brain = CurrentNN;
		}

		public void Next()
		{
			CurrentNN.fitness = CurrentFitness;
			Index++;

			if(Index >= numPerGen)
			{
				neuralNetworks.Sort();
				neuralNetworks.Reverse();
				NeuralNetwork lastGenWinner = neuralNetworks[0];
				NeuralNetwork.SaveToFile(lastGenWinner, "Gen_" + CurrentGenNumber + "_Winner_" + lastGenWinner.fitness);
				neuralNetworks.Clear();
				for(var i = 0; i < numPerGen; i++)
				{
					NeuralNetwork network = new NeuralNetwork(lastGenWinner);
					network.Mutate(mutationPercent);
					neuralNetworks.Add(network);
				}
				Index = 0;
				CurrentGenNumber++;
				CurrentNN = neuralNetworks[Index];
				comPlayer.brain = CurrentNN;
				CurrentFitness = 0;
				onNextGenerationEvent.Invoke();
			}
			else
			{
				CurrentNN = neuralNetworks[Index];
				comPlayer.brain = CurrentNN;
				CurrentFitness = 0;
				onNextNetworkEvent.Invoke();
			}
		}
	}
}
