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
		MapComponent map = GameObject.Find("Map").GetComponent<MapComponent>();
		UILabel c = GameObject.Find("score white").GetComponent<UILabel>();
		val = map.GetMap().scores[(int) Gomoku.Color.White];
		c.text = val.ToString();
		UILabel d = GameObject.Find("score black").GetComponent<UILabel>();
		val = map.GetMap().scores[(int) Gomoku.Color.Black];
		d.text = val.ToString();
	}
}
