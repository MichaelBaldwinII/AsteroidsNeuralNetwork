using UnityEngine;
using UnityEngine.UI;

namespace Asteroids
{
	/// <summary>
	/// The FPSCounter class when added to a class, calculates and displays the current frames per second.
	/// </summary>
	[RequireComponent(typeof(Text))]
	public sealed class FpsDisplay : MonoBehaviour
	{
		private const int DEFAULT_LOW_FPS = 15;
		private const int DEFAULT_HIGH_FPS = 60;
		private static readonly Color32 DefaultLowFpsColor = new Color32(255, 0, 0, 255);
		private static readonly Color32 DefaultMidFpsColor = new Color32(255, 192, 0, 255);
		private static readonly Color32 DefaultHighFpsColor = new Color32(0, 255, 0, 255);

		[Range(0, 60)]
		[Tooltip("If the FPS drops below this, then the text will be colored the low fps color")]
		public int lowFps = DEFAULT_LOW_FPS;
		[Range(0, 60)]
		[Tooltip("If the FPS rises above this, then the text will be colored the high fps color")]
		public int highFps = DEFAULT_HIGH_FPS;
		[Tooltip("The color the FPS text should be when below the lowFPS value")]
		public Color32 lowFpsColor = DefaultLowFpsColor;
		[Tooltip("The color the FPS text should be when above the lowFPS value but below the highFPS value")]
		public Color32 midFpsColor = DefaultMidFpsColor;
		[Tooltip("The color the FPS text should be when above the highFPS value")]
		public Color32 highFpsColor = DefaultHighFpsColor;
		private int frameCount;
		private float previousTicks;
		private Text textComponent;

		private void Awake()
		{
			QualitySettings.vSyncCount = 0;
			textComponent = GetComponent<Text>();
		}

		private void LateUpdate()
		{
			if (lowFps > highFps)
			{
				var temp = lowFps;
				lowFps = highFps;
				highFps = temp;
			}

			frameCount++;
			if (Time.time - previousTicks >= 1.0f)
			{
				textComponent.text = "FPS(" + frameCount + ")";
				if (frameCount <= lowFps)
					textComponent.color = lowFpsColor;
				else if (frameCount >= highFps)
					textComponent.color = highFpsColor;
				else
					textComponent.color = midFpsColor;

				frameCount = 0;
				previousTicks = Time.time;
			}
		}
	}
}
