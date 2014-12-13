using UnityEngine;
using System.Collections;
using Gomoku;
using UnityEngine.UI;

public class UpdateScoring : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		int val;
		MapComponent map = GameObject.Find("Map").GetComponent<MapComponent>();
		Text c = GameObject.Find("score white").GetComponent<Text>();
		val = map.GetMap().scores[(int) Gomoku.Color.White];
		c.text = "White " + val.ToString();
		Text d = GameObject.Find("score black").GetComponent<Text>();
		val = map.GetMap().scores[(int) Gomoku.Color.Black];
		d.text = "Black " + val.ToString();
	}
}