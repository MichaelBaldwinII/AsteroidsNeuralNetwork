using UnityEngine;
using UnityEngine.UI;

namespace Baldwin
{
	/// <summary>
	/// The FpsDisplay class is a script that, which when added to a class, calculates and displays the current frames per second.
	/// </summary>
	[RequireComponent(typeof(Text))]
	public sealed class FpsDisplay : MonoBehaviour
	{
		#region Fields

		private const int defaultLowFps = 15;
		private const int defaultHighFps = 60;
		private static readonly Color32 defaultLowFpsColor = new Color32(255, 0, 0, 255);
		private static readonly Color32 defaultMidFpsColor = new Color32(255, 192, 0, 255);
		private static readonly Color32 defaultHighFpsColor = new Color32(0, 255, 0, 255);

		[Tooltip("The frame rate that Unity should try to render at")]
		public int targetFrameRate = 60;
		[Tooltip("Should the FPS be locked at the set framerate")]
		public bool lockFps;
		[Tooltip("If the FPS drops below this, then the text will be colored the low fps color")]
		public int lowFps = defaultLowFps;
		[Tooltip("If the FPS rises above this, then the text will be colored the high fps color")]
		public int highFps = defaultHighFps;
		[Tooltip("The color the FPS text should be when below the lowFPS value")]
		public Color32 lowFpsColor = defaultLowFpsColor;
		[Tooltip("The color the FPS text should be when above the lowFPS value but below the highFPS value")]
		public Color32 midFpsColor = defaultMidFpsColor;
		[Tooltip("The color the FPS text should be when above the highFPS value")]
		public Color32 highFpsColor = defaultHighFpsColor;
		private int frameCount;
		private float previousTicks;

		#endregion

		#region Methods

		private void Awake()
		{
			QualitySettings.vSyncCount = 0;
		}

		private void LateUpdate()
		{
			/*
			 * Increment the frame count, check to see if one second has passed (the FPS is only updates every second).
			 * If enough time has passed, determine the Text color based upon how many frames have been counted.
			 * Finally reset the frame count for the next second, and record the current time.
			 */

			frameCount++;

			if(Time.time - previousTicks >= 1.0f)
			{
				GetComponent<Text>().text = "FPS(" + frameCount + ")";

				//Color the text based upon current FPS.
				if(frameCount <= lowFps)
				{
					GetComponent<Text>().color = lowFpsColor;
				}
				else if(frameCount >= highFps)
				{
					GetComponent<Text>().color = highFpsColor;
				}
				else
				{
					GetComponent<Text>().color = midFpsColor;
				}

				frameCount = 0;
				previousTicks = Time.time;
			}
		}

		private void OnValidate()
		{
			/*
			 * This method is called when ever a variable is changed in the Inspector.
			 */

			targetFrameRate = targetFrameRate < 1 ? 1 : targetFrameRate;

			if(lockFps)
			{
				Application.targetFrameRate = targetFrameRate;
			}
			else
			{
				Application.targetFrameRate = -1;
			}
		}

		#endregion
	}
}
