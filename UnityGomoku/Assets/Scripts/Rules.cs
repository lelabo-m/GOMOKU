using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gomoku;

// Component Arbiter
public class Rules : MonoBehaviour
{

		public bool fiveBreakable = false;
		public bool doubleThree = false;
		public const int MAX_SCORE = 10;
		public System.Collections.Generic.Dictionary<Gomoku.Color, int> scores; 

		// Use this for initialization
		void Start ()
		{
				if (PlayerPrefs.GetInt ("5 cassables") > 1) {
						fiveBreakable = true;
				}
				if (PlayerPrefs.GetInt ("double 3") > 1) {
						doubleThree = true;
				}
				scores = new System.Collections.Generic.Dictionary<Gomoku.Color, int> ();
				scores.Add (Gomoku.Color.White, 0);
				scores.Add (Gomoku.Color.Black, 0);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	

		// true si possible de poser
		public bool putPawn (Gomoku.Map map, int x, int y)
		{
				if (map.GetColor(x, y) != Gomoku.Color.Empty)
						return false;
				return true;
		}

		public void scoring (Gomoku.Color remover, int score)
		{
				scores [remover] += score;
		}

		public bool canTakePawns (MapComponent map, int x, int y, Gomoku.Orientation orientation)
		{
				return (map.GetMap ().IsTaking (x, y, orientation) && 
						map.GetMap ().GetColor (x + 3 * MapComponent.ORIENTATION [orientation] [0], y + 3 * MapComponent.ORIENTATION [orientation] [1]) == map.GetMap ().GetColor (x, y));
		}

		public void takePawns (MapComponent map, int x, int y, Gomoku.Orientation orientation)
		{
				scoring ((Gomoku.Color)map.GetMap ().GetColor (x, y), 2);
				map.GetMap ().SetIsTaking (x, y, orientation, false);
				map.GetMap ().SetIsTaking (x + 3 * MapComponent.ORIENTATION [orientation] [0], y + 3 * MapComponent.ORIENTATION [orientation] [1], orientation, false);
				map.removePawn (x + MapComponent.ORIENTATION [orientation] [0], y + MapComponent.ORIENTATION [orientation] [1]);
				map.removePawn (x + 2 * MapComponent.ORIENTATION [orientation] [0], y + 2 * MapComponent.ORIENTATION [orientation] [1]);
		}

		//regarde si alignement de 5
		public Gomoku.Color isWinner (MapComponent map)
		{
				Gomoku.Color win = Gomoku.Color.Empty;

				for (int x = 0; x < MapComponent.SIZE_MAP; ++x) {
						for (int y = 0; y < MapComponent.SIZE_MAP; ++y) {
								if ((win = weightToFive (x, y, map)) != Gomoku.Color.Empty) {
										if (!fiveBreakable || !checkIsBreakable (map, x, y))
												return win;
								}
				
						}
				}
				return  win;
		}

		private bool checkIsBreakable (MapComponent map, int x, int y)
		{
				bool right = isBreakable (map, x, y, Gomoku.Orientation.EAST);
				bool up = isBreakable (map, x, y, Gomoku.Orientation.NORTH);
				bool rightUp = isBreakable (map, x, y, Gomoku.Orientation.NORTHEAST);
				bool leftUp = isBreakable (map, x, y, Gomoku.Orientation.NORTHWEST);

				if (!right || !up || !rightUp || !leftUp)
						return false;
				return true;
		}

		private bool isBreakable (MapComponent map, int x, int y, Gomoku.Orientation orientation)
		{
				int countPawn = 0;
				Gomoku.Color color = map.GetMap ().GetColor (x, y);

				while (x > 0 && y > 0 &&
		              y < MapComponent.SIZE_MAP - 1 && x < MapComponent.SIZE_MAP - 1
		              && map.GetMap().GetColor(x, y) == color) {
						x -= MapComponent.ORIENTATION [orientation] [0];
						y -= MapComponent.ORIENTATION [orientation] [1];
				}

				if (map.GetMap ().GetColor (x, y) != color) {
						x += MapComponent.ORIENTATION [orientation] [0];
						y += MapComponent.ORIENTATION [orientation] [1];		
				}
				
				while (x >= 0 && y >= 0 &&
		       	y < MapComponent.SIZE_MAP && x < MapComponent.SIZE_MAP
		       	&& map.GetMap().GetColor(x, y) == color && countPawn < 5) {
					//	print ("x = " + x + " y = " + y + " countPawn = " + countPawn);
						//if (IsTakingAroundMe (map, x, y))
						if (map.GetMap().IsTakeable(x, y))
							countPawn = 0;
						else 
								countPawn++;
						x += MapComponent.ORIENTATION [orientation] [0];
						y += MapComponent.ORIENTATION [orientation] [1];
				}
				if (countPawn < 5)
						return true;
				return false;
		}

		private bool IsTakingAroundMe (MapComponent map, int x, int y)
		{
				Gomoku.Color color = map.GetMap ().GetColor (x, y);
				Gomoku.Color enemy = (color == Gomoku.Color.Black) ? Gomoku.Color.White : Gomoku.Color.Black;

				foreach (KeyValuePair<Gomoku.Orientation, int[]> entry in MapComponent.ORIENTATION) {
						if (x - entry.Value [0] >= 0 && x - entry.Value [0] < MapComponent.SIZE_MAP && 
								y - entry.Value [1] >= 0 && y - entry.Value [1] < MapComponent.SIZE_MAP && 
								map.GetMap ().GetColor (x - entry.Value [0], y - entry.Value [1]) == enemy) {
								if (map.GetMap ().IsTaking (x - entry.Value [0], y - entry.Value [1], entry.Key)) {
										return true;
								}	
						}
				}
				return false;
		}

		private Gomoku.Color weightToFive (int x, int y, MapComponent map)
		{
				if (map.GetMap ().GetWeight (x, y, Gomoku.Color.Black) >= 5) 
						return Gomoku.Color.Black;
				if (map.GetMap ().GetWeight (x, y, Gomoku.Color.White) >= 5)
						return Gomoku.Color.White;
				return Gomoku.Color.Empty;
		}

		public Gomoku.Color isScoringWinner ()
		{
				if (scores [Gomoku.Color.White] == MAX_SCORE)
						return Gomoku.Color.White;
				if (scores [Gomoku.Color.Black] == MAX_SCORE)
						return Gomoku.Color.Black;
				return Gomoku.Color.Empty;
		}
}
