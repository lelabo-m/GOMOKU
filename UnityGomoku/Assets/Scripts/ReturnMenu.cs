using UnityEngine;
using System.Collections;

public class ReturnMenu : MonoBehaviour {
	UILabel tm;
	// Use this for initialization
	void Start () {
		tm = (UILabel)GameObject.Find ("Label_victory").GetComponent<UILabel>();

	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerPrefs.GetInt("Winner") == 0)
			tm.text = "WHITE WINS";
		else if (PlayerPrefs.GetInt ("Winner") == 1)
			tm.text = "BLACK WINS";
		else
			tm.text = "DRAW";	}
	public void GoMenu () {
		PlayerPrefs.SetInt ("Winner", -1);
		Application.LoadLevel(1);
	}

	public void restart () {
		PlayerPrefs.SetInt ("Winner", -1);
		Application.LoadLevel(2);
	}


	public void GoQuit (){
		Application.Quit();
	}

}