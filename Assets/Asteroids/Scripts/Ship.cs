using UnityEngine;

namespace Asteroids
{
	public class Ship : MonoBehaviour
	{
		public bool isPlayer;
		public GameObject thrustGobject;
		public GameObject bulletPrefab;
		public float moveSpeed = 3.0f;
		public float rotationSpeed = 100.0f;
		public float reloadSpeed = 1.0f;
		private float lastShotTime;
		private bool isWrappingOnX;
		private bool isWrappingOnY;
		private Vector3 lastPos;

		private void Update()
		{
			if(isPlayer)
			{
				float verticalInput = Input.GetAxis("Vertical");
				float horizontalInput = Input.GetAxis("Horizontal");
				//thrustGobject.SetActive(!Mathf.Approximately(verticalInput, 0));
				transform.Translate(transform.up * moveSpeed * verticalInput * Time.deltaTime, Space.World);
				transform.Rotate(0, 0, rotationSpeed * -horizontalInput * Time.deltaTime);

				if(Input.GetButton("Fire1"))
				{
					Fire();
				}
			}

			thrustGobject.SetActive(!transform.position.Equals(lastPos));
			lastPos = transform.position;
		}

		public void Thrust()
		{
			transform.Translate(transform.up * moveSpeed * Time.deltaTime, Space.World);
		}

		public void Rotate(bool inPosDir)
		{
			transform.Rotate(0, 0, rotationSpeed * (inPosDir ? 1 : -1) * Time.deltaTime, Space.World);
		}

		public void Fire()
		{
			if(Time.time - lastShotTime >= reloadSpeed)
			{
				GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
				bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * 300);
				lastShotTime = Time.time;
			}
		}

		private void OnBecameVisible()
		{
			isWrappingOnX = false;
			isWrappingOnY = false;
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

		private void OnCollisionEnter2D(Collision2D other)
		{
			//Ship destroyed, so game ended
			//Destroy(gameObject);
			FindObjectOfType<GenManager>().OnShipCollision();
		}

		public void ResetPos()
		{
			transform.position = Vector3.zero;
			isWrappingOnX = false;
			isWrappingOnY = false;
		}
	}
}
