using UnityEngine;
using System.Collections;

public class SoundAlltime : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void Awake (){
		DontDestroyOnLoad (this);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
