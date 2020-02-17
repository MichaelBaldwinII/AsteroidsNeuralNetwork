using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Asteroids
{
	public class GenManager : Singleton<GenManager>
	{
		[Header("Config")]
		[Tooltip("The number of inputs into each Neural Network, this is also the number of raycasts performed")]
		public int numOfInputs = 360;
		[Tooltip("The number of hidden layers each Neural Network has")]
		public int numOfHiddenLayers = 2;
		[Tooltip("The number of nodes each hidden layer has, for each Neural Network")]
		public int numOfHiddenNodesPerLayer = 16;
		[Tooltip("The number of output nodes each Neural Network has")]
		public int numOfOutputs = 4;
		[Tooltip("Current mutation percentage, this increases with each unsuccessful generation")]
		public float mutationPercent = 0.05f;
		[Tooltip("The starting mutation percentage, current percentage is reset to this upon a successful generation")]
		public float mutationPercentStart = 0.05f;
		[Tooltip("Mutation percentage change per each unsuccessful generation (if the gen doesn't score a higher fitness than the best, it's considered unsuccessful)")]
		public float mutationPercentChange = 0.01f;
		[Tooltip("How many iterations per generation")]
		public int numPerGen = 10;
		[Tooltip("Computer player")]
		public ComputerPlayer comPlayer;

		[Header("Events")]
		public UnityEvent onNextNetworkEvent;
		public UnityEvent onNextGenerationEvent;

		public NeuralNetwork CurrentNN { get; private set; }
		public NeuralNetwork BestNN { get; private set; }
		public float CurrentFitness { get; private set; }
		public int CurrentGenNumber { get; private set; }
		public int Index { get; private set; }
		private List<NeuralNetwork> networks = new List<NeuralNetwork>();

		public void AddFitness(float amount)
		{
			CurrentFitness += amount;
		}

		public void SetNumOfInputs(string value)
		{
			numOfInputs = int.Parse(value);
		}

		public void SetNumOfHiddenLayers(string value)
		{
			numOfHiddenLayers = int.Parse(value);
		}

		public void SetNumOfHiddenNodesPerLayer(string value)
		{
			numOfHiddenNodesPerLayer = int.Parse(value);
		}

		public void SetNumOfOutputs(string value)
		{
			numOfOutputs = int.Parse(value);
		}

		public void SetMutationPercentStart(string value)
		{
			mutationPercentStart = int.Parse(value) / 100.0f;
			mutationPercent = mutationPercentStart;
		}

		public void SetMutationPercentDelta(string value)
		{
			mutationPercentStart = int.Parse(value) / 100.0f;
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

			// Populate the first generation
			for(var i = 0; i < numPerGen; i++)
				networks.Add(new NeuralNetwork(numOfInputs, numOfHiddenLayers, numOfHiddenNodesPerLayer, numOfOutputs));

			CurrentNN = networks[0];
			comPlayer.brain = CurrentNN;
		}

		public void Next()
		{
			CurrentNN.fitness = CurrentFitness;
			Index++;

			if(Index >= numPerGen)
			{
				networks.Sort();
				networks.Reverse();

				if (BestNN == null || networks[0].fitness > BestNN.fitness)
				{
					BestNN = networks[0];
					mutationPercent = mutationPercentStart;
				}
				else
					mutationPercent += mutationPercentChange;
				
				// NeuralNetwork.SaveToFile(lastGenWinner, "Gen_" + CurrentGenNumber + "_Winner_" + lastGenWinner.fitness);
				networks.Clear();
				for(var i = 0; i < numPerGen; i++)
				{
					var network = new NeuralNetwork(BestNN);
					network.Mutate(mutationPercent);
					networks.Add(network);
				}
				Index = 0;
				CurrentGenNumber++;
				CurrentNN = networks[Index];
				comPlayer.brain = CurrentNN;
				CurrentFitness = 0;
				onNextGenerationEvent.Invoke();
			}
			else
			{
				CurrentNN = networks[Index];
				comPlayer.brain = CurrentNN;
				CurrentFitness = 0;
				onNextNetworkEvent.Invoke();
			}
		}
	}
}
