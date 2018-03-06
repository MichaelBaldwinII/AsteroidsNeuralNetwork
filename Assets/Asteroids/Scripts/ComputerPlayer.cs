using System.Collections.Generic;
using Baldwin.AI;
using UnityEngine;

namespace Asteroids
{
	public class ComputerPlayer : MonoBehaviour
	{
		public NeuralNetwork brain;
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
			List<Ray2D> rays = new List<Ray2D>(8)
			{
				new Ray2D(ship.transform.position, ship.transform.up),
				new Ray2D(ship.transform.position, (ship.transform.up + ship.transform.right).normalized),
				new Ray2D(ship.transform.position, ship.transform.right),
				new Ray2D(ship.transform.position, (-ship.transform.up + ship.transform.right).normalized),
				new Ray2D(ship.transform.position, -ship.transform.up),
				new Ray2D(ship.transform.position, (-ship.transform.up + -ship.transform.right).normalized),
				new Ray2D(ship.transform.position, -ship.transform.right),
				new Ray2D(ship.transform.position, (ship.transform.up + -ship.transform.right).normalized)
			};
			List<RaycastHit2D> hits = new List<RaycastHit2D>(8)
			{
				Physics2D.Raycast(ship.transform.position, ship.transform.up, float.MaxValue, LayerMask.GetMask("Asteroids")),
				Physics2D.Raycast(ship.transform.position, (ship.transform.up + ship.transform.right).normalized, float.MaxValue, LayerMask.GetMask("Asteroids")),
				Physics2D.Raycast(ship.transform.position, ship.transform.right, float.MaxValue, LayerMask.GetMask("Asteroids")),
				Physics2D.Raycast(ship.transform.position, (-ship.transform.up + ship.transform.right).normalized, float.MaxValue, LayerMask.GetMask("Asteroids")),
				Physics2D.Raycast(ship.transform.position, -ship.transform.up, float.MaxValue, LayerMask.GetMask("Asteroids")),
				Physics2D.Raycast(ship.transform.position, (-ship.transform.up + -ship.transform.right).normalized, float.MaxValue, LayerMask.GetMask("Asteroids")),
				Physics2D.Raycast(ship.transform.position, -ship.transform.right, float.MaxValue, LayerMask.GetMask("Asteroids")),
				Physics2D.Raycast(ship.transform.position, (ship.transform.up + -ship.transform.right).normalized, float.MaxValue, LayerMask.GetMask("Asteroids"))
			};

			for(int i = 0; i < 8; i++)
			{
				Debug.DrawRay(rays[i].origin, hits[i].distance > 0 ? rays[i].direction * hits[i].distance : rays[i].direction * 100);
			}

			//Input the collision detected inputs
			List<float> inputValues = new List<float>(16)
			{
				hits[0].collider != null ? 1.0f : 0.0f, hits[0].distance,
				hits[1].collider != null ? 1.0f : 0.0f, hits[1].distance,
				hits[2].collider != null ? 1.0f : 0.0f, hits[2].distance,
				hits[3].collider != null ? 1.0f : 0.0f, hits[3].distance,
				hits[4].collider != null ? 1.0f : 0.0f, hits[4].distance,
				hits[5].collider != null ? 1.0f : 0.0f, hits[5].distance,
				hits[6].collider != null ? 1.0f : 0.0f, hits[6].distance,
				hits[7].collider != null ? 1.0f : 0.0f, hits[7].distance
			};

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
