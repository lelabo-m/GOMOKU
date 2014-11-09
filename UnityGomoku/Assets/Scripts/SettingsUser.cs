	using UnityEngine;
	using System.Collections;

	public class SettingsUser : MonoBehaviour {

		public void OnChangeCinq (){
			if (PlayerPrefs.GetInt ("5 cassables") == 1) {
					PlayerPrefs.SetInt ("5 cassables", 2);
				} 
				else {
					PlayerPrefs.SetInt ("5 cassables", 1);
				}
		}

		public void OnChangeTrois (){
			if (PlayerPrefs.GetInt ("double 3") == 1) {
				PlayerPrefs.SetInt ("double 3", 2);
			} 
			else {
				PlayerPrefs.SetInt ("double 3", 1);
			}
		}
	
		void Start () {
		/*
		GameObject tm = GameObject.Find("Settings");
		UIToggle[] test = tm.GetComponentsInChildren<UIToggle> ();

		foreach (UIToggle value in test){
			//value.startsActive = false;
		}

				if (PlayerPrefs.GetInt ("5 cassables") != 0) {
						if (PlayerPrefs.GetInt ("5 cassables") == 1) {
							
								//tm.startsActive = false;
						} 
						if (PlayerPrefs.GetInt ("double 3") == 1) {
						}
				} else {
			*/
				PlayerPrefs.SetInt ("5 cassables", 2);
				PlayerPrefs.SetInt ("double 3", 2);
			//}
		}

	}