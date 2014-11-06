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
		 if (PlayerPrefs.GetInt ("5 cassables") > 0) {
			fiveBreakable = true;
		}
		if (PlayerPrefs.GetInt ("double 3") > 0) {
			doubleThree = true;
		}
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

	public void scoring(PlayerComponent.Color remover, int score) {
				scores [remover] += score;
	}

	//regarde si alignement de 5
	public PlayerComponent.Color isWinner(MapComponent map) 
	{
		PlayerComponent.Color win = PlayerComponent.Color.Empty;

		for (int x = 0; x < MapComponent.SIZE_MAP; ++x) {
			for (int y = 0; y < MapComponent.SIZE_MAP; ++y) {
				if ((win = weightToFive(x, y, map)) != PlayerComponent.Color.Empty) {
					if (!fiveBreakable)
						return win;
				}
				
			}
		}
		return  win;
	}

	private PlayerComponent.Color weightToFive(int x, int y, MapComponent map)
	{
		if (map.getBitsMap ().getWeight (x, y, MapComponent.Color.Black) >= 5) 
						return PlayerComponent.Color.Black;
		if (map.getBitsMap ().getWeight (x, y, MapComponent.Color.White) >= 5)
						return PlayerComponent.Color.White;
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
