using UnityEngine;
using System.Collections;

public class ReturnMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		UILabel tm = (UILabel)GameObject.Find ("Label_victory").GetComponent<UILabel>();
		if (PlayerPrefs.GetInt("Winner") == 0)
			tm.text = "WHITE WINS";
		else if (PlayerPrefs.GetInt ("Winner") == 1)
			tm.text = "BLACK WINS";
		else
			tm.text = "DRAW";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void GoMenu () {
		PlayerPrefs.SetInt ("Winner", -1);
		Application.LoadLevel(1);
	}

	public void GoQuit (){
		Application.Quit();
	}

}