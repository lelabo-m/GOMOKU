	using UnityEngine;
	using System.Collections;

	public class SettingsUser : MonoBehaviour {

		public void OnChangeCinq (){
		if (PlayerPrefs.GetInt ("5 cassables") == 0 || PlayerPrefs.GetInt ("5 cassables") == 2) {
				PlayerPrefs.SetInt ("5 cassables", 1);
			} 
			else {
				PlayerPrefs.SetInt ("5 cassables", 0);
			}
		}

		public void OnChangeTrois (){
		if (PlayerPrefs.GetInt ("double 3") == 0 || PlayerPrefs.GetInt ("double 3") == 2) {
			PlayerPrefs.SetInt ("double 3", 1);
		} 
		else {
			PlayerPrefs.SetInt ("double 3", 0);
		}
	}
	
	void Start () {
	
	}

	}