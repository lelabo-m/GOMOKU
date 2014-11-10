using UnityEngine;
using System.Collections;

public class MenuInGame : MonoBehaviour {

	public static GameObject menu;
	public static bool menuOn;
	// Use this for initialization
	void Start () {
		GameObject tm = GameObject.Find("UI Root");
		menu = tm;
		menu.SetActive (false);
		menuOn = false;
	}

	public void GoMenu() {
		Application.LoadLevel(1);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("tab")) {
			if (menuOn == false){
				menuOn = true;
				menu.SetActive (true);
			}
			else{
				menuOn = false;
				menu.SetActive (false);
			}
		}
	}
}
