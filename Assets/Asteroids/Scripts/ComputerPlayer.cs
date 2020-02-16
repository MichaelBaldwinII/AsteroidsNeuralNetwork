using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
	public class ComputerPlayer : MonoBehaviour
	{
		public NeuralNetwork brain;
		public bool drawRays = false;
		private Ship ship;

		private void Start()
		{
			ship = GetComponent<Ship>();
		}

		private void Update()
		{
			ProcessOutputs(brain.Process(GetInputs()));
		}

		private List<float> GetInputs()
		{
			var rays = new List<Ray2D>();
			var stepAmount = 360.0f / GenManager.Instance.numOfInputs;
			for(var i = 0f; i < 360f; i += stepAmount)
				rays.Add(new Ray2D(ship.transform.position, Quaternion.AngleAxis(i, Vector3.forward) * ship.transform.right));

			var hits = new List<RaycastHit2D>();
			foreach(var iRay in rays)
				hits.Add(Physics2D.Raycast(iRay.origin, iRay.direction, float.MaxValue, LayerMask.GetMask("Asteroids")));

			if(drawRays)
				for(var i = 0; i < rays.Count; i++)
					Debug.DrawRay(rays[i].origin, hits[i].distance > 0 ? rays[i].direction * hits[i].distance : rays[i].direction * 25);

			//Input the collision detected inputs
			var inputValues = new List<float>();
			foreach(var iHit in hits)
				inputValues.Add(iHit.distance);

			return inputValues;
		}

		private void ProcessOutputs(List<float> outputs)
		{
			var largest = 0;
			for(var i = 0; i < outputs.Count; i++)
				if (outputs[i] > outputs[largest])
					largest = i;

			switch (largest)
			{
				case 0:
					break;
				case 1:
					ship.Fire();
					break;
				case 2:
					ship.Thrust();
					break;
				case 3:
					ship.Rotate(true);
					break;
				case 4:
					ship.Rotate(false);
					break;
			}
		}
	}
}
