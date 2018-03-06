using System.Collections.Generic;

namespace Baldwin.AI
{
	public class Layer
	{
		public Layer child;
		public readonly List<Node> nodes = new List<Node>();
		public bool IsOutputLayer { get { return child == null; } }

		public Layer(int numOfNodes)
		{
			nodes = new List<Node>(numOfNodes);

			for(int i = 0; i < numOfNodes; i++)
			{
				nodes.Add(new Node());
			}
		}

		public void AddChildLayer(Layer childLayer)
		{
			child = childLayer;
			foreach(Node iNode in nodes)
			{
				iNode.AddConnections(child.nodes);
			}
		}

		public List<float> GetOutputs(List<float> inputs)
		{
			List<float> outputs = new List<float>(nodes.Count);

			for(int i = 0; i < nodes.Count || i < inputs.Count; i++)
			{
				outputs.Add(nodes[i].GetOutput(inputs[i]));
			}

			return outputs;
		}
	}
}
