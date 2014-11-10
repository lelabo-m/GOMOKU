using UnityEngine;
using System.Collections;

public class StartSettings : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt ("5 cassables", 2);
		PlayerPrefs.SetInt ("double 3", 2);
		PlayerPrefs.SetInt ("son", 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
