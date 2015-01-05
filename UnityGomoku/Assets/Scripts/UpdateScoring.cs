using UnityEngine;
using System.Collections;
using Gomoku;
using UnityEngine.UI;

public class UpdateScoring : MonoBehaviour
{
	
		void Start ()
		{

		}
	
		void Update ()
		{
				int val;
				MapComponent map = GameObject.Find ("Map").GetComponent<MapComponent> ();
				Text c = GameObject.Find ("score white").GetComponent<Text> ();
				Text cf = GameObject.Find ("score whiteE").GetComponent<Text> ();
				val = map.GetMap ().scores [(int)Gomoku.Color.White];
				c.text = val.ToString ();
				cf.text = val.ToString ();
				Text d = GameObject.Find ("score black").GetComponent<Text> ();
				Text df = GameObject.Find ("score blackE").GetComponent<Text> ();
				val = map.GetMap ().scores [(int)Gomoku.Color.Black];
				d.text = val.ToString ();
				df.text = val.ToString ();
		}
}