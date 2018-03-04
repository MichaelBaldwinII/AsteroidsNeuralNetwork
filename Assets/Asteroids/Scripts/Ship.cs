using UnityEngine;

namespace Asteroids
{
	public class Ship : MonoBehaviour
	{
		public bool isPlayer;
		public float moveSpeed = 1.0f;
		public float rotationSpeed = 1.0f;
		public GameObject thrustGobject;
		public GameObject bulletPrefab;
		private bool isWrappingOnX;
		private bool isWrappingOnY;

		private void Update()
		{
			float verticalInput = Input.GetAxis("Vertical");
			thrustGobject.SetActive(!Mathf.Approximately(verticalInput, 0));

			if(Input.GetButtonDown("Fire1"))
			{
				GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
				bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * 300);
			}
		}

		private void FixedUpdate()
		{
			if(isPlayer)
			{
				float verticalInput = Input.GetAxis("Vertical");
				float horizontalInput = Input.GetAxis("Horizontal");

				GetComponent<Rigidbody2D>().AddTorque(-horizontalInput * rotationSpeed);
				GetComponent<Rigidbody2D>().AddForce(transform.up * verticalInput * moveSpeed);
				//transform.up  = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
				//transform.Translate(transform.up * moveSpeed * verticalInput);
			}
		}

		private void OnBecameInvisible()
		{
			Vector2 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

			if((viewportPos.x < 0 || viewportPos.x > 1) && !isWrappingOnX)
			{
				transform.position = new Vector2(-transform.position.x, transform.position.y);
				isWrappingOnX = true;
			}

			if((viewportPos.y < 0 || viewportPos.y > 1) && !isWrappingOnY)
			{
				transform.position = new Vector2(transform.position.x, -transform.position.y);
				isWrappingOnY = true;
			}
		}
	}
}
