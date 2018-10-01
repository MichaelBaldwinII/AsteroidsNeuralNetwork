using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Baldwin.AI
{
	[Serializable]
	public class Layer
	{
		public int size;
		public List<float> biases;
		public List<float> values;

		public Layer(int size)
		{
			this.size = size;
			biases = new List<float>(size);
			values = new List<float>(size);

			for(var i = 0; i < size; i++)
			{
				biases.Add(1);
				values.Add(0);
			}
		}

		public Layer(Layer layer)
		{
			size = layer.size;
			biases = new List<float>(layer.biases);
			values = new List<float>(layer.values);
		}

		/// <summary>
		/// Process a previous layer values into this layer by summing the products of the previous layers values and this bias'
		/// </summary>
		/// <param name="listOfValues">List of floats to process.</param>
		public void Process(List<float> listOfValues)
		{
			for(var i = 0; i < size; i++)
			{
				values[i] = 0;
				foreach(var iValue in listOfValues)
				{
					values[i] += iValue * biases[i];
				}
			}
		}

		public void Randomize()
		{
			biases.Clear();
			for(var i = 0; i < size; i++)
				biases.Add(Random.Range(-1.0f, 1.0f));
		}

		public void Mutate(float percentChance)
		{
			for(var i = 0; i < size; i++)
				if(Random.value < percentChance)
					biases[i] = Random.Range(-1.0f, 1.0f);
		}

		public void Sigmoid()
		{
			for(var i = 0; i < size; i++)
				values[i] = Extensions.Sigmoid(values[i]);
		}
	}
}
