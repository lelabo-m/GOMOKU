using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// Component Arbiter
public class Rules : MonoBehaviour {

	public System.Collections.Generic.Dictionary<PlayerComponent.Color, int> scores; 

	// Use this for initialization
	void Start () 
	{
		scores = new System.Collections.Generic.Dictionary<PlayerComponent.Color, int> ();
		scores.Add (PlayerComponent.Color.White, 0);
		scores.Add (PlayerComponent.Color.Black, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool putPawn(MapComponent.BitsMap map, int x, int y) {
				return true;
	}

	public void removePawn(PlayerComponent.Color remover) {
				scores [remover]++;
	}

}
