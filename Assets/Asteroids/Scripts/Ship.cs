using Baldwin.AI;
using UnityEngine;

namespace Asteroids
{
	public class Ship : MonoBehaviour
	{
		[Header("Prefabs & Refs")]
		public GameObject bulletPrefab;
		public GameObject thrustGobject;

		[Header("Config")]
		public float moveSpeed = 3.0f;
		public float rotationSpeed = 100.0f;
		public float reloadSpeed = 1.0f;
		private float lastShotTime;
		private Vector3 lastPos;

		private void Update()
		{
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

		private void OnBecameInvisible()
		{
			Vector2 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

			if(viewportPos.x < 0 || viewportPos.x > 1)
			{
				transform.position = new Vector2(-transform.position.x, transform.position.y);
			}

			if(viewportPos.y < 0 || viewportPos.y > 1)
			{
				transform.position = new Vector2(transform.position.x, -transform.position.y);
			}
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			GenManager.Instance.OnShipCollision();
			transform.position = Vector3.zero;
		}
	}
}
