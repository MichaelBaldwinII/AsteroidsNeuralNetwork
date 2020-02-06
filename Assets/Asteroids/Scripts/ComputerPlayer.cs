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
			List<Ray2D> rays = new List<Ray2D>();
			var stepAmount = 360.0f / (GenManager.Instance.numOfInputs / 2);
			for(var i = 0f; i < 360f; i += stepAmount)
				rays.Add(new Ray2D(ship.transform.position, Quaternion.AngleAxis(i, Vector3.forward) * ship.transform.right));

			List<RaycastHit2D> hits = new List<RaycastHit2D>();
			foreach(Ray2D iRay in rays)
				hits.Add(Physics2D.Raycast(iRay.origin, iRay.direction, float.MaxValue, LayerMask.GetMask("Asteroids")));

			if(drawRays)
				for(var i = 0; i < rays.Count; i++)
					Debug.DrawRay(rays[i].origin, hits[i].distance > 0 ? rays[i].direction * hits[i].distance : rays[i].direction * 100);

			//Input the collision detected inputs
			List<float> inputValues = new List<float>();
			foreach(RaycastHit2D iHit in hits)
			{
				inputValues.Add(iHit.collider != null ? 1.0f : 0.0f);
				inputValues.Add(iHit.distance);
			}

			return inputValues;
		}

		private void ProcessOutputs(List<float> outputs)
		{
			if(outputs[0] > 0.5f)
			{
				ship.Thrust();
			}
			if(outputs[1] > 0.5f)
			{
				ship.Rotate(true);
			}
			if(outputs[2] > 0.5f)
			{
				ship.Rotate(false);
			}
			if(outputs[3] > 0.5f)
			{
				ship.Fire();
			}
		}
	}
}
