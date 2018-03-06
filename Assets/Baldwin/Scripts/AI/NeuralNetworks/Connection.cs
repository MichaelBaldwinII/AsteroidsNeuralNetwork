using UnityEngine;

namespace Baldwin.AI
{
	public class Connection
	{
		public Node child;
		public float weight = 0.5f;

		public Connection()
		{
			child = null;
			weight = Random.value;
		}

		public Connection(Node child)
		{
			this.child = child;
			weight = Random.value;
		}

		public Connection(Node child, float weight)
		{
			this.child = child;
			this.weight = weight;
		}

		public float GetOutput(float value)
		{
			if(child != null)
			{
				return child.GetOutput(value * weight);
			}

			return value * weight;
		}

		public void Mutate()
		{
			weight = Mathf.Clamp01(weight + Random.value);
		}
	}
}
