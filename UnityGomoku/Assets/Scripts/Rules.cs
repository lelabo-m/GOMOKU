using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Component Arbiter
public class Rules : MonoBehaviour
{

		public bool fiveBreakable = false;
		public bool doubleThree = false;
		public const int MAX_SCORE = 10;
		public System.Collections.Generic.Dictionary<PlayerComponent.Color, int> scores; 

		// Use this for initialization
		void Start ()
		{
				if (PlayerPrefs.GetInt ("5 cassables") > 1) {
						fiveBreakable = true;
				}
				if (PlayerPrefs.GetInt ("double 3") > 1) {
						doubleThree = true;
				}
				scores = new System.Collections.Generic.Dictionary<PlayerComponent.Color, int> ();
				scores.Add (PlayerComponent.Color.White, 0);
				scores.Add (PlayerComponent.Color.Black, 0);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		// true si possible de poser
		public bool putPawn (MapComponent.BitsMap map, int x, int y)
		{
				return true;
		}

		// true si possible de poser
		public bool putPawn (char[] map, int x, int y)
		{
				if (map [x * MapComponent.SIZE_MAP + y] != (char)MapComponent.Color.Empty)
						return false;
				return true;
		}

		public void scoring (PlayerComponent.Color remover, int score)
		{
				scores [remover] += score;
		}

		public bool canTakePawns (MapComponent map, int x, int y, int orientation)
		{
				return (map.getBitsMap ().isTaking (x, y, orientation) && 
						map.getBitsMap ().getColor (x + 3 * MapComponent.ORIENTATION [orientation] [0], y + 3 * MapComponent.ORIENTATION [orientation] [1]) == map.getBitsMap ().getColor (x, y));
		}

		public void takePawns (MapComponent map, int x, int y, int orientation)
		{
				scoring ((PlayerComponent.Color)map.getBitsMap ().getColor (x, y), 2);
				map.getBitsMap ().setIsTaking (x, y, orientation, (char)0);
				map.getBitsMap ().setIsTaking (x + 3 * MapComponent.ORIENTATION [orientation] [0], y + 3 * MapComponent.ORIENTATION [orientation] [1], orientation, (char)0);
				map.removePawn (x + MapComponent.ORIENTATION [orientation] [0], y + MapComponent.ORIENTATION [orientation] [1]);
				map.removePawn (x + 2 * MapComponent.ORIENTATION [orientation] [0], y + 2 * MapComponent.ORIENTATION [orientation] [1]);
		}

		//regarde si alignement de 5
		public PlayerComponent.Color isWinner (MapComponent map)
		{
				PlayerComponent.Color win = PlayerComponent.Color.Empty;

				for (int x = 0; x < MapComponent.SIZE_MAP; ++x) {
						for (int y = 0; y < MapComponent.SIZE_MAP; ++y) {
								if ((win = weightToFive (x, y, map)) != PlayerComponent.Color.Empty) {
									print ("x = " + x + " y = " + y);
										if (!fiveBreakable || !checkIsBreakable (map, x, y))
												return win;
										return PlayerComponent.Color.Empty;
								}
				
						}
				}
				return  win;
		}

		private bool checkIsBreakable (MapComponent map, int x, int y)
		{
				bool right = isBreakable (map, x, y, MapComponent.O_RIGHT);
				bool up = isBreakable (map, x, y, MapComponent.O_UP);
				bool rightUp = isBreakable (map, x, y, MapComponent.O_RIGHT_UP);
				bool leftUp = isBreakable (map, x, y, MapComponent.O_LEFT_UP);

				if (!right || !up || !rightUp || !leftUp)
						return false;
				return true;
		}

		private bool isBreakable (MapComponent map, int x, int y, int orientation)
		{
				int countPawn = 0;
				MapComponent.Color color = map.getBitsMap ().getColor (x, y);

				//print ("isBreakable: x = " + x + " y = " + y + " color = " + color + " orientation = " + orientation);
				while (x > 0 && y > 0 &&
		              y < MapComponent.SIZE_MAP - 1 && x < MapComponent.SIZE_MAP - 1
		              && map.getBitsMap().getColor(x, y) == color) {
						x -= MapComponent.ORIENTATION [orientation] [0];
						y -= MapComponent.ORIENTATION [orientation] [1];
				}

				if (map.getBitsMap ().getColor (x, y) != color) {
						x += MapComponent.ORIENTATION [orientation] [0];
						y += MapComponent.ORIENTATION [orientation] [1];		
				}
				
				//	print ("x = " + x + " y = " + y + " countPawn = " + countPawn);
				//print ("color = " + map.getBitsMap ().getColor (x, y));
				while (x >= 0 && y >= 0 &&
		       	y < MapComponent.SIZE_MAP && x < MapComponent.SIZE_MAP
		       	&& map.getBitsMap().getColor(x, y) == color && countPawn < 5) {
						//print ("x = " + x + " y = " + y + " countPawn = " + countPawn);
						if (isTakingAroundMe (map, x, y))
								countPawn = 0;
						else 
								countPawn++;
						x += MapComponent.ORIENTATION [orientation] [0];
						y += MapComponent.ORIENTATION [orientation] [1];
				}
			//	print ("countPawn = " + countPawn);
				if (countPawn < 5)
						return true;
				return false;
		}

		private bool isTakingAroundMe (MapComponent map, int x, int y)
		{
				MapComponent.Color color = map.getBitsMap ().getColor (x, y);
				MapComponent.Color enemy = (color == MapComponent.Color.Black) ? MapComponent.Color.White : MapComponent.Color.Black;

				foreach (KeyValuePair<int, int[]> entry in MapComponent.ORIENTATION) {
						if (x - entry.Value [0] >= 0 && x - entry.Value [0] < MapComponent.SIZE_MAP && 
								y - entry.Value [1] >= 0 && y - entry.Value [1] < MapComponent.SIZE_MAP && 
								map.getBitsMap ().getColor (x - entry.Value [0], y - entry.Value [1]) == enemy) {
								//print ("enemy found");
								//print ("isTaking = " + map.getBitsMap ().isTaking (x - entry.Value [0], y - entry.Value [1], entry.Key));
								for (int i = 0; i < 8; i++) {
										print (map.getBitsMap ().isTaking (x - entry.Value [0], y - entry.Value [1], i));
								}
								if (map.getBitsMap ().isTaking (x - entry.Value [0], y - entry.Value [1], entry.Key)) {
										return true;
								}	
						}
				}
				return false;
		}

		private PlayerComponent.Color weightToFive (int x, int y, MapComponent map)
		{
				if (map.getBitsMap ().getWeight (x, y, MapComponent.Color.Black) >= 5) 
						return PlayerComponent.Color.Black;
				if (map.getBitsMap ().getWeight (x, y, MapComponent.Color.White) >= 5)
						return PlayerComponent.Color.White;
				return PlayerComponent.Color.Empty;
		}

		public PlayerComponent.Color isScoringWinner ()
		{
				if (scores [PlayerComponent.Color.White] == MAX_SCORE)
						return PlayerComponent.Color.White;
				if (scores [PlayerComponent.Color.Black] == MAX_SCORE)
						return PlayerComponent.Color.Black;
				return PlayerComponent.Color.Empty;
		}
}
