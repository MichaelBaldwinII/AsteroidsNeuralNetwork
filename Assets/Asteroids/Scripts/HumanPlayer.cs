using UnityEngine;

namespace Asteroids
{
	public class HumanPlayer : MonoBehaviour
	{
		public string thrustInputString = "Ship Thrust";
		public string rotateCCWInput = "Ship Rotate CCW";
		public string rotateCWInput = "Ship Rotate CW";
		public string fireInputString = "Ship Fire";
		private Ship ship;

		private void Start()
		{
			ship = GetComponent<Ship>();
		}

		private void Update()
		{
			if(Input.GetButton(thrustInputString))
			{
				ship.Thrust();
			}
			if(Input.GetButton(rotateCCWInput))
			{
				ship.Rotate(true);
			}
			if(Input.GetButton(rotateCWInput))
			{
				ship.Rotate(false);
			}
			if(Input.GetButton(fireInputString))
			{
				ship.Fire();
			}
		}
	}
}
