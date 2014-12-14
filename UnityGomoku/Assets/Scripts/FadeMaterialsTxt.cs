using UnityEngine;
using System.Collections;
using Gomoku;
using UnityEngine.UI;

class FadeMaterialsTxt : MonoBehaviour {
	
	public Text _myTexture;    //The guiTexture you want to manipulate. Set this up in the inspector.
	public float stayTime = 2f;   // Time before fading starts
	public float fadeTime = 2f;   // How long we want to fade
	public float startTime = 2f;
	private float lastStartTime = -100f;
	
	void Update(){
		if (_myTexture.enabled == true) AnimateImage(); //Animate the texture if it is enabled
	}
	void AnimateImage(){
		float t = (Time.time - lastStartTime - stayTime)/fadeTime;
		if(t <= 0f){
			UnityEngine.Color fullColor = UnityEngine.Color.white;
			UnityEngine.Color noColor = new UnityEngine.Color(1f,1f,1f,0f);
			_myTexture.color = UnityEngine.Color.Lerp(noColor, fullColor, 1 + t) ;
		}
		else if(t <= 1f){
			UnityEngine.Color fullColor = UnityEngine.Color.white;
			UnityEngine.Color noColor = new UnityEngine.Color(1f,1f,1f,0f);
			_myTexture.color = UnityEngine.Color.Lerp(fullColor, noColor, t) ;
		} else {    //Once we faded out we shut the texture off, in order to save memory
			_myTexture.enabled = false;
		}
	}
	public void ShowImage(){    //Call this to start the effect
		_myTexture.enabled = true;
		_myTexture.color = new UnityEngine.Color(1f,1f,1f,0f);
		lastStartTime = Time.time;
	}  
	
}

//220 - 200 - 40 / 20

