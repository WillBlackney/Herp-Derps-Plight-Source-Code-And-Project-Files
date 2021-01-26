using System;
using UnityEngine;

namespace Assets.HeroEditor.Common.EditorScripts
{
	/// <summary>
	/// Take a screenshot in play mode [S].
	/// </summary>
	public class Screenshot : MonoBehaviour
	{
		public int SuperSize = 1;
		public string Directory = "Screenshots";

		public string GetPath()
		{
			return $"{Directory}/Screenshot_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				var fileName = GetPath();

				System.IO.Directory.CreateDirectory(Directory);
				ScreenCapture.CaptureScreenshot(fileName, SuperSize);
				Debug.Log($"Screenshot saved: {fileName}");
			}
		}
	}
}