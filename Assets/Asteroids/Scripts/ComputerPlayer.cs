using System.Collections.Generic;
using Baldwin.AI;
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
			//There are 16 inputs, the 8 directions and the 8 distances to an asteroid in those directions.
			//First enter the inputs
			/*Vector2 deg0 = ship.transform.right;
			Vector2 deg90 = ship.transform.up;
			Vector2 deg180 = -ship.transform.right;
			Vector2 deg270 = -ship.transform.up;

			Vector2 deg45 = (deg0 + deg90).normalized;
			Vector2 deg135 = (deg90 + deg180).normalized;
			Vector2 deg225 = (deg180 + deg270).normalized;
			Vector2 deg315 = (deg270 + deg0).normalized;

			Vector2 deg30 = (deg0 + deg45).normalized;
			Vector2 deg60 = (deg45 + deg90).normalized;
			Vector2 deg120 = (deg90 + deg135).normalized;
			Vector2 deg150 = (deg135 + deg180).normalized;
			Vector2 deg210 = (deg180 + deg225).normalized;
			Vector2 deg240 = (deg225 + deg270).normalized;
			Vector2 deg300 = (deg270 + deg315).normalized;
			Vector2 deg330 = (deg315 + deg0).normalized;*/

			List<Ray2D> rays = new List<Ray2D>();
			for(int i = 0; i < 360; i++)
			{
				rays.Add(new Ray2D(ship.transform.position, Quaternion.AngleAxis(i, Vector3.forward) * ship.transform.right));
			}//*/

			/*List<Ray2D> rays = new List<Ray2D>
			{
				new Ray2D(ship.transform.position, deg0),
				new Ray2D(ship.transform.position, deg30),
				new Ray2D(ship.transform.position, deg45),
				new Ray2D(ship.transform.position, deg60),
				new Ray2D(ship.transform.position, deg90),
				new Ray2D(ship.transform.position, deg120),
				new Ray2D(ship.transform.position, deg135),
				new Ray2D(ship.transform.position, deg150),
				new Ray2D(ship.transform.position, deg180),
				new Ray2D(ship.transform.position, deg210),
				new Ray2D(ship.transform.position, deg225),
				new Ray2D(ship.transform.position, deg240),
				new Ray2D(ship.transform.position, deg270),
				new Ray2D(ship.transform.position, deg300),
				new Ray2D(ship.transform.position, deg315),
				new Ray2D(ship.transform.position, deg330)
			};//*/

			List<RaycastHit2D> hits = new List<RaycastHit2D>();
			foreach(Ray2D iRay in rays)
			{
				hits.Add(Physics2D.Raycast(iRay.origin, iRay.direction, float.MaxValue, LayerMask.GetMask("Asteroids")));
			}

			if(drawRays)
			{
				for(int i = 0; i < rays.Count; i++)
				{
					Debug.DrawRay(rays[i].origin, hits[i].distance > 0 ? rays[i].direction * hits[i].distance : rays[i].direction * 100);
				}
			}

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
