using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids
{
	[Serializable]
	public class NeuralNetwork : IComparable<NeuralNetwork>
	{
		public List<Matrix> weights;
		public float fitness;

		public NeuralNetwork(int numOfInputs, int numOfHiddenLayers, int numOfNodesPerHiddenLayer, int numOfOutputs)
		{
			weights = new List<Matrix>(numOfHiddenLayers + 1);
			weights.Add(new Matrix(numOfNodesPerHiddenLayer, numOfInputs));
			for (var i = 0; i < numOfHiddenLayers - 1; i++)
				weights.Add(new Matrix(numOfNodesPerHiddenLayer, numOfNodesPerHiddenLayer));
			weights.Add(new Matrix(numOfOutputs, numOfNodesPerHiddenLayer));

			foreach (var iWeights in weights)
				iWeights.Randomize();
		}

		public NeuralNetwork(NeuralNetwork otherNetwork)
		{
			weights = new List<Matrix>(otherNetwork.weights.Count);
			foreach (var iMatrix in otherNetwork.weights)
				weights.Add(new Matrix(iMatrix));
		}

		public List<float> Process(List<float> inputValues)
		{
			// First we add the NN inputs to build the first matrix, multiply that with the first weight matrix
			var inputMatrix = new Matrix(inputValues);
			var layerInputs = inputMatrix;

			foreach (var iWeights in weights)
			{
				var tempMat = iWeights * layerInputs;
				
				var activationValues = new Matrix(tempMat.RowCount, 1);
				for (var i = 0; i < tempMat.RowCount; i++)
				{
					var rowValues = tempMat.GetRow(i);
					var sum = rowValues.Sum();
					var activatedSum = Extensions.Sigmoid(sum);
					activationValues[i, 0] = activatedSum;
				}

				layerInputs = activationValues;
			}

			return layerInputs.GetColumn(0);
		}

		public void Mutate(float mutationPercent)
		{
			foreach (var iWeights in weights)
				if (Random.value <= mutationPercent)
					iWeights.Mutate(mutationPercent);
		}

		public int CompareTo(NeuralNetwork other)
		{
			if (fitness > other.fitness)
				return 1;
			if (fitness < other.fitness)
				return -1;
			return 0;
		}

		public static void SaveToFile(NeuralNetwork network, string fileName)
		{
			var destination = Application.persistentDataPath + "/" + fileName + ".NN";
			var file = File.Exists(destination) ? File.OpenWrite(destination) : File.Create(destination);
			var bf = new BinaryFormatter();
			bf.Serialize(file, network);
			file.Close();
		}

		public static NeuralNetwork LoadFromFile(string fileName)
		{
			var destination = Application.persistentDataPath + "/" + fileName + ".NN";
			var file = File.Exists(destination) ? File.OpenRead(destination) : File.Create(destination);
			var bf = new BinaryFormatter();
			var result = (NeuralNetwork) bf.Deserialize(file);
			file.Close();
			return result;
		}
	}
}
