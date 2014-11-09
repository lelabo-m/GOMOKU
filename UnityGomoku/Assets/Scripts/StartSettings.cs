using UnityEngine;
using System.Collections;

public class StartSettings : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt ("5 cassables", 0);
		PlayerPrefs.SetInt ("double 3", 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
