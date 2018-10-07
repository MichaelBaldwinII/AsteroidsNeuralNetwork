using Baldwin;
using Baldwin.AI;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids
{
	public class InGameGUI : MonoBehaviour
	{
		public Text genText;
		public GameObject currentGenTextPrefab;
		public GameObject scrollViewContent;
		private Text currentRunText;

		private void Start()
		{
			currentRunText = Instantiate(currentGenTextPrefab, scrollViewContent.transform).GetOrAddComponent<Text>();
		}

		private void Update()
		{
			if(currentRunText != null)
			{
				currentRunText.text = (GenManager.Instance.Index + 1) + ": " + GenManager.Instance.CurrentFitness;
				currentRunText.color = Color.magenta;
				genText.text = "Gen: " + GenManager.Instance.CurrentGenNumber;
			}
		}

		public void OnNextButton()
		{
			GenManager.Instance.Next();
		}

		public void OnNextNetwork()
		{
			currentRunText.color = Color.white;
			currentRunText = Instantiate(currentGenTextPrefab, scrollViewContent.transform).GetOrAddComponent<Text>();
		}

		public void OnResumeButton()
		{
			Time.timeScale = 1;
		}

		public void OnMenuButton()
		{
			Time.timeScale = 0;
		}

		public void OnNextGeneration()
		{
			foreach(Transform iChild in scrollViewContent.transform)
			{
				Destroy(iChild.gameObject);
			}

			currentRunText.color = Color.white;
			currentRunText = Instantiate(currentGenTextPrefab, scrollViewContent.transform).GetOrAddComponent<Text>();
		}
	}
}
