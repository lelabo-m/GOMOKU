using UnityEngine;
using System.Collections;

public class SettingsUser : MonoBehaviour
{

		public static string[] ia_lvls;
		public static UILabel statIA;
		public static UILabel statGameSett;

		public void OnChangeIa ()
		{
				if (statIA != null) {
						int j = 2;
						for (int i=0; i < 10; i++) {
								if (ia_lvls [i].Equals (statIA.text)) {
										j = i;
										break;
								}
						}
						PlayerPrefs.SetInt ("IaLevel", j + 1);
				}
		}

		public void OnChangeGameSett ()
		{
				if (statGameSett != null) {
						PlayerPrefs.SetInt ("double 3", 1);
						PlayerPrefs.SetInt ("5 cassables", 1);

						if (statGameSett.text.Equals ("Cinq cassable & double 3".ToString ())) {
								PlayerPrefs.SetInt ("double 3", 2);
								PlayerPrefs.SetInt ("5 cassables", 2);	
						}
						if (statGameSett.text.Equals ("Cinq cassable".ToString ()))
								PlayerPrefs.SetInt ("5 cassables", 2);
						if (statGameSett.text.Equals ("Double 3".ToString ()))
								PlayerPrefs.SetInt ("double 3", 2);
				}
		}
	
		void Start ()
		{
				ia_lvls = new string[] {
						"IA te donne la partie",
						"IA très facile",
						"IA facile",
						"IA ça va encore",
						"IA faut voir ton niveau",
						"IA ça commence à chatouiller hein ?!",
						"IA ah bah oui il faut s'entraîner aussi",
						"IA tu le retentes tu es sûr ?",
						"IA non mais abandonne !",
						"IA HAHAHAHAHAHA (rire démoniaque)"
				};
				GameObject tes = GameObject.Find ("Label_ia");
				statIA = tes.GetComponent<UILabel> ();
				UIPopupList opop = GameObject.Find ("ia").GetComponent<UIPopupList> ();
				if (PlayerPrefs.GetInt ("IaLevel") < 1) 
						PlayerPrefs.SetInt ("IaLevel", 1);
				opop.value = ia_lvls [PlayerPrefs.GetInt ("IaLevel") - 1];
		
				GameObject tes2 = GameObject.Find ("Label_opts");
				statGameSett = tes2.GetComponent<UILabel> ();
				UIPopupList opopo = GameObject.Find ("opts").GetComponent<UIPopupList> ();
				if (PlayerPrefs.GetInt ("double 3") == 2 && PlayerPrefs.GetInt ("5 cassables") == 2)
						opopo.value = "Cinq cassable & double 3".ToString ();
				else if (PlayerPrefs.GetInt ("5 cassables") == 2)
						opopo.value = "Cinq cassable".ToString ();
				else if (PlayerPrefs.GetInt ("double 3") == 2)
						opopo.value = "Double 3".ToString ();
				else
						opopo.value = "Ø".ToString ();
		}
}