using UnityEngine;

namespace Baldwin
{
	public class DDOL : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}
