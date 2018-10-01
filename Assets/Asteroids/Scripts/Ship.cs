using Baldwin;
using Baldwin.AI;
using UnityEngine;
using UnityEngine.Events;

namespace Asteroids
{
	public class Ship : MonoBehaviour, Pauseable
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

		[Header("Events")]
		public UnityEvent onShipCollisionEvent;

		[Header("Runtime")]
		public bool isPaused = false;

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
			if(!isPaused)
			{
				thrustGobject.SetActive(!transform.position.Equals(lastPos));
				lastPos = transform.position;
			}
		}

		public void Thrust()
		{
			if(!isPaused)
			{
				transform.Translate(transform.up * moveSpeed * Time.deltaTime, Space.World);
				GenManager.Instance.AddFitness(Time.deltaTime);
			}
		}

		public void Rotate(bool inPosDir)
		{
			if(!isPaused)
			{
				transform.Rotate(0, 0, rotationSpeed * (inPosDir ? 1 : -1) * Time.deltaTime, Space.World);
			}
		}

		public void Fire()
		{
			if(!isPaused && Time.time - lastShotTime >= reloadSpeed)
			{
				GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
				bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * 300);
				lastShotTime = Time.time;
				GenManager.Instance.AddFitness(-1);
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
			ResetShip();
			onShipCollisionEvent.Invoke();
		}

		public void ResetShip()
		{
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
			OnUnpause();
		}

		//Have to do this to prevent errors when stopping play in the editor
		private void OnApplicationQuit()
		{
			gameObject.Disable();
		}

		#region Pauseable

		public void OnPause()
		{
			isPaused = true;
		}

		public void OnUnpause()
		{
			isPaused = false;
		}

		#endregion
	}
}
