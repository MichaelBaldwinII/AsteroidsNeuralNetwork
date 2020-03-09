using UnityEngine;
using UnityEngine.Events;

namespace Asteroids
{
	public class Ship : MonoBehaviour
	{
		[Header("Prefabs & Refs")]
		public GameObject bulletPrefab;
		public GameObject thrustGameObject;

		[Header("Config")]
		public float moveSpeed = 3.0f;
		public float rotationSpeed = 100.0f;
		public float reloadSpeed = 1.0f;

		[Header("Events")]
		public UnityEvent onShipCollisionEvent;
		
		private float lastShotTime;
		private Vector3 lastPos;

		public void SetShipMoveSpeed(string value)
		{
			moveSpeed = float.Parse(value);
		}

		public void SetShipRotationSpeed(string value)
		{
			rotationSpeed = float.Parse(value);
		}

		public void SetShipReloadSpeed(string value)
		{
			reloadSpeed = float.Parse(value);
		}

		public void SetBulletPrefabLifeTime(string value)
		{
			if(bulletPrefab != null)
				bulletPrefab.GetComponent<Bullet>().lifeSpan = float.Parse(value);
		}

		private void Update()
		{
			thrustGameObject.SetActive(!transform.position.Equals(lastPos));
			lastPos = transform.position;
		}

		private void LateUpdate()
		{
			GenManager.Instance.AddFitness(1);
		}

		public void Thrust()
		{
			transform.Translate(transform.up * (moveSpeed * Time.deltaTime), Space.World);
		}

		public void Rotate(bool inPosDir)
		{
			transform.Rotate(0, 0, rotationSpeed * (inPosDir ? 1 : -1) * Time.deltaTime, Space.World);
		}

		public void Fire()
		{
			if(Time.time - lastShotTime >= reloadSpeed)
			{
				var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
				bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * 300);
				lastShotTime = Time.time;
			}
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			ResetShip();
			onShipCollisionEvent.Invoke();
		}

		private void ResetShip()
		{
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
		}
	}
}
