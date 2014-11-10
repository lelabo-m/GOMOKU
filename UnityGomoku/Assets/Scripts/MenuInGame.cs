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
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("tab")) {
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
