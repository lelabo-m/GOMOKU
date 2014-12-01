using UnityEngine;
using System.Collections;
using Gomoku;

public class UpdateScoring : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		int val;
		Rules rules = GameObject.Find("Arbiter").GetComponent<Rules>();
		UILabel c = GameObject.Find("score white").GetComponent<UILabel>();
		rules.scores.TryGetValue (Gomoku.Color.White, out val);
		c.text = val.ToString();
		UILabel d = GameObject.Find("score black").GetComponent<UILabel>();
		rules.scores.TryGetValue (Gomoku.Color.Black, out val);
		d.text = val.ToString();
	}
}
