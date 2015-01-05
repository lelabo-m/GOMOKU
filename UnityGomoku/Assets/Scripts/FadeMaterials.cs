using UnityEngine;
using System.Collections;
using Gomoku;
using UnityEngine.UI;

class FadeMaterials : MonoBehaviour
{
 
		public UILabel _myTexture;
		public float stayTime = 2f;
		public float fadeTime = 2f;
		public float startTime = 2f;
		private float lastStartTime = -100f;
	
		void Update ()
		{
				if (_myTexture.enabled == true)
						AnimateImage ();
		}

		void AnimateImage ()
		{
				float t = (Time.time - lastStartTime - stayTime) / fadeTime;
				if (t <= 0f) {
						UnityEngine.Color fullColor = UnityEngine.Color.black;
						UnityEngine.Color noColor = new UnityEngine.Color (0f, 0f, 0f, 0f);
						_myTexture.color = UnityEngine.Color.Lerp (noColor, fullColor, 1 + t);
				} else if (t <= 1f) {
						UnityEngine.Color fullColor = UnityEngine.Color.black;
						UnityEngine.Color noColor = new UnityEngine.Color (0f, 0f, 0f, 0f);
						_myTexture.color = UnityEngine.Color.Lerp (fullColor, noColor, t);
				} else {
						_myTexture.enabled = false;
				}
		}

		public void ShowImage ()
		{ 
				_myTexture.enabled = true;
				_myTexture.color = new UnityEngine.Color (0f, 0f, 0f, 0f);
				lastStartTime = Time.time;
		}  
 
}

