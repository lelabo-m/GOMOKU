using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// Component Arbiter
public class Rules : MonoBehaviour {

	public bool fiveBreakable = false;
	public bool doubleThree = false;
	public const int MAX_SCORE = 10;
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


	// true si possible de poser
	public bool putPawn(MapComponent.BitsMap map, int x, int y) {
				return true;
	}

	// true si possible de poser
	public bool putPawn(char[] map, int x, int y) {
		if (map [x * MapComponent.SIZE_MAP + y] != (char) MapComponent.Color.Empty)
						return false;
		return true;
	}

	public void removePawn(PlayerComponent.Color remover) {
				scores [remover]++;
	}

	//regarde si alignement de 5
	public PlayerComponent.Color isWinner(MapComponent map) 
	{
		return PlayerComponent.Color.Empty; 
	}

	public PlayerComponent.Color isScoringWinner()
	{
		if (scores [PlayerComponent.Color.White] == MAX_SCORE)
			return PlayerComponent.Color.White;
		if (scores [PlayerComponent.Color.Black] == MAX_SCORE)
			return PlayerComponent.Color.Black;
		return PlayerComponent.Color.Empty;
	}
}
