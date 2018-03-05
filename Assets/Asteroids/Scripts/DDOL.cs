using UnityEngine;

namespace Asteroids
{
	public class DDOL : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}
