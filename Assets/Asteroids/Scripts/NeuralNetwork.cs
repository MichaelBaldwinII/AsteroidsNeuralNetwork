using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

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
		}

		public NeuralNetwork(NeuralNetwork otherNetwork)
		{
			weights = new List<Matrix>(otherNetwork.weights);
		}

		public List<float> Process(List<float> inputValues)
		{
			// First we add the NN inputs to build the first matrix, multiply that with the first weight matrix
			var inputMatrix = new Matrix(inputValues.Count, 1);
			inputMatrix.SetValues(inputValues);
			var matValues = inputMatrix;
			
			foreach(var iWeights in weights)
			{
				matValues *= iWeights;
				var actValues = new Matrix(matValues.RowCount, 1);
				for (var i = 0; i < matValues.RowCount; i++)
				{
					var rowValues = matValues.GetRow(i);
					var sum = rowValues.Sum();
					actValues[i, 0] = sum;
				}
				actValues.Sigmoid();

				matValues = actValues;
			}

			return matValues.GetColumn(0);
		}

		public void Mutate(float mutationPercent)
		{
		}
		
		public int CompareTo(NeuralNetwork other)
		{
			if(fitness > other.fitness)
				return 1;
			if(fitness < other.fitness)
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
			var result = (NeuralNetwork)bf.Deserialize(file);
			file.Close();
			return result;
		}
	}
}
