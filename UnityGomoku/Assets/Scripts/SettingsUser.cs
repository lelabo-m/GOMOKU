	using UnityEngine;
	using System.Collections;

	public class SettingsUser : MonoBehaviour {

		public static GameObject statm;
		public static GameObject statm2;
		public static GameObject statmson;
		public static GameObject sooon;

		public void OnChangeCinq (){
			if (PlayerPrefs.GetInt ("5 cassables") == 1) {
					PlayerPrefs.SetInt ("5 cassables", 2);
					statm2.SetActive (true);
				} 
				else {
					PlayerPrefs.SetInt ("5 cassables", 1);
					statm2.SetActive (false);
				}
		}

		public void OnChangeTrois (){
			if (PlayerPrefs.GetInt ("double 3") == 1) {
				PlayerPrefs.SetInt ("double 3", 2);
				statm.SetActive (true);
			} 
			else {
				PlayerPrefs.SetInt ("double 3", 1);
				statm.SetActive (false);
			}
		}

		public void OnChangeSon(){
			if (PlayerPrefs.GetInt ("son") == 1) {
					PlayerPrefs.SetInt ("son", 0);
					statmson.SetActive (false);
			AudioSource truc = sooon.GetComponent<AudioSource> ();
				truc.mute = true;
				} else {
					PlayerPrefs.SetInt ("son", 1);
					statmson.SetActive (true);
			AudioSource truc = sooon.GetComponent<AudioSource> ();
					truc.mute = false;
				}
		}
	
		void Start () {
			GameObject tm = GameObject.Find("trois2");
			GameObject tm2 = GameObject.Find("cinq2");
			GameObject tmson = GameObject.Find("son2");
			GameObject soundd = GameObject.Find("Sound");

			statm = tm;
			statm2 = tm2;
			statmson = tmson;
			sooon = soundd;
			if (PlayerPrefs.GetInt ("5 cassables") == 1)
				statm2.SetActive (false);
			else
				statm2.SetActive (true);
			if (PlayerPrefs.GetInt ("double 3") == 1)
				statm.SetActive (false);
			else
				statm.SetActive (true);
			if (PlayerPrefs.GetInt ("son") == 0)
				statmson.SetActive (false);
			else
				statmson.SetActive (true);
	}
}