using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Baldwin.AI
{
	[Serializable]
	public class NeuralNetwork : IComparable<NeuralNetwork>
	{
		public readonly Matrix inputMatrix;
		public readonly Matrix firstHiddenMatrix;
		public readonly Matrix secondHiddenMatrix;
		public readonly Matrix outputMatrix;
		public readonly Matrix inputBiases;
		public readonly Matrix firstHiddenBiases;
		public readonly Matrix secondHiddenBiases;
		public readonly Matrix outputBiases;
		public float fitness;

		public NeuralNetwork(int numOfInputNodes, int numOfHiddenNodes, int numOfOutputNodes)
		{
			inputMatrix = new Matrix(numOfHiddenNodes, numOfInputNodes);
			firstHiddenMatrix = new Matrix(numOfHiddenNodes, numOfHiddenNodes);
			secondHiddenMatrix = new Matrix(numOfHiddenNodes, numOfHiddenNodes);
			outputMatrix = new Matrix(numOfOutputNodes, numOfHiddenNodes);
			inputBiases = new Matrix(numOfInputNodes, 1);
			firstHiddenBiases = new Matrix(numOfHiddenNodes, 1);
			secondHiddenBiases = new Matrix(numOfHiddenNodes, 1);
			outputBiases = new Matrix(numOfOutputNodes, 1);
			fitness = 0;

			inputMatrix.Randomize();
			firstHiddenMatrix.Randomize();
			secondHiddenMatrix.Randomize();
			outputMatrix.Randomize();
			inputBiases.Randomize();
			firstHiddenBiases.Randomize();
			secondHiddenBiases.Randomize();
			outputBiases.Randomize();
		}

		public NeuralNetwork(NeuralNetwork otherNetwork)
		{
			inputMatrix = new Matrix(otherNetwork.inputMatrix);
			firstHiddenMatrix = new Matrix(otherNetwork.firstHiddenMatrix);
			secondHiddenMatrix = new Matrix(otherNetwork.secondHiddenMatrix);
			outputMatrix = new Matrix(otherNetwork.outputMatrix);
			inputBiases = new Matrix(otherNetwork.inputBiases);
			firstHiddenBiases = new Matrix(otherNetwork.firstHiddenBiases);
			secondHiddenBiases = new Matrix(otherNetwork.secondHiddenBiases);
			outputBiases = new Matrix(otherNetwork.outputBiases);
			fitness = 0;
		}

		public List<float> Process(List<float> inputValues)
		{
			Matrix inputToHidden = inputMatrix * new Matrix(inputValues);
			inputToHidden -= inputBiases;
			inputToHidden.Sigmoid();

			Matrix hiddenToHidden = firstHiddenMatrix * inputToHidden;
			hiddenToHidden -= firstHiddenBiases;
			hiddenToHidden.Sigmoid();

			Matrix hiddenToOutput = secondHiddenMatrix * hiddenToHidden;
			hiddenToOutput -= secondHiddenBiases;
			hiddenToOutput.Sigmoid();

			Matrix outputToResult = outputMatrix * hiddenToOutput;
			outputToResult -= outputBiases;
			outputToResult.Sigmoid();

			return outputToResult.values;
		}

		public void Mutate(float mutationPercent)
		{
			inputMatrix.Mutate(mutationPercent);
			firstHiddenMatrix.Mutate(mutationPercent);
			secondHiddenMatrix.Mutate(mutationPercent);
			outputMatrix.Mutate(mutationPercent);
			inputBiases.Mutate(mutationPercent);
			firstHiddenBiases.Mutate(mutationPercent);
			secondHiddenBiases.Mutate(mutationPercent);
			outputBiases.Mutate(mutationPercent);
		}

		public int CompareTo(NeuralNetwork other)
		{
			if(fitness > other.fitness)
			{
				return 1;
			}

			if(fitness < other.fitness)
			{
				return -1;
			}

			return 0;
		}

		public static void SaveToFile(NeuralNetwork network, string fileName)
		{
			string destination = Application.persistentDataPath + "/" + fileName + ".NN";
			FileStream file = File.Exists(destination) ? File.OpenWrite(destination) : File.Create(destination);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(file, network);
			file.Close();
		}

		public static NeuralNetwork LoadFromFile(string fileName)
		{
			string destination = Application.persistentDataPath + "/" + fileName + ".NN";
			FileStream file = File.Exists(destination) ? File.OpenRead(destination) : File.Create(destination);
			BinaryFormatter bf = new BinaryFormatter();
			NeuralNetwork result = (NeuralNetwork)bf.Deserialize(file);
			file.Close();
			return result;
		}
	}
}
