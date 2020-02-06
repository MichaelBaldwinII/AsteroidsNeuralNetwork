using UnityEngine;

namespace Asteroids
{
	public class MainMenu : MonoBehaviour
	{
		public void OnExitButton()
		{
			Debug.Log("Exit button pressed.");
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
			#endif
		}
	}
}
