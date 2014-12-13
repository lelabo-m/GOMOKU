using UnityEngine;
using System.Collections;

public class MenuInGame : MonoBehaviour {

	public static GameObject menu;
	public static bool menuOn;
	// Use this for initialization
	void Start () {

	}

	public void GoMenu() {
		Application.LoadLevel(1);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
