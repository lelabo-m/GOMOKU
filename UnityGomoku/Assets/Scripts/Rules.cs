using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gomoku;

// Component Arbiter
public class Rules : MonoBehaviour
{

		public bool FiveBreakable = false;
		public bool DoubleThree = false;
		public const int MAX_SCORE = 10;
		public System.Collections.Generic.Dictionary<Gomoku.Color, int> scores; 
		private List<int[]> masks;

		// Use this for initialization
		void Start ()
		{
				if (PlayerPrefs.GetInt ("5 cassables") > 1) {
						FiveBreakable = true;
						print ("five breakable active");
				}
				if (PlayerPrefs.GetInt ("double 3") > 1) {
					 	DoubleThree = true;
						print ("double 3 active");
				}
				masks = new List<int[]> ();
				masks.Add (new int[] { -1, 0, 2, 2, 1, 0, -1, -1, -1});
				masks.Add (new int[] { 0, 2, 2, 0, 1, 0, -1, -1, -1});
				masks.Add (new int[] { 0, 2, 0, 2, 1, 0, -1, -1, -1});
				masks.Add (new int[] { -1, -1, 0, 2, 1, 2, 0, -1, -1});
				masks.Add (new int[] { -1, 0, 2, 0, 1, 2, 0, -1, -1});
				masks.Add (new int[] { -1, -1, 0, 2, 1, 0, 2, 0, -1});
				masks.Add (new int[] { -1, -1, -1, 0, 1, 2, 2, 0, -1});
				masks.Add (new int[] { -1, -1, -1, 0, 1, 2, 0, 2, 0});
				masks.Add (new int[] { -1, -1, -1, 0, 1, 0, 2, 2, 0});
				scores = new System.Collections.Generic.Dictionary<Gomoku.Color, int> ();
				scores.Add (Gomoku.Color.White, 0);
				scores.Add (Gomoku.Color.Black, 0);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	

		// true si possible de poser
		public bool PutPawn (Gomoku.Map map, int x, int y)
		{
				if (map.GetColor(x, y) != Gomoku.Color.Empty)
						return false;
				return true;
		}


		public bool UpdateMap(Gomoku.Map map, int x, int y)
		{
			Gomoku.Coord currentCell = new Gomoku.Coord ();
			currentCell.x = x;
			currentCell.y = y;
			Gomoku.Color color = map.GetColor (x, y);

			UpdateCellData (Orientation.EAST, currentCell, map);
			UpdateCellData (Orientation.NORTH, currentCell, map);
			UpdateCellData (Orientation.NORTHEAST, currentCell, map);
			UpdateCellData (Orientation.NORTHWEST, currentCell, map);

			SetIsTaking (map, currentCell, color);
			
			UpdateAround (map, currentCell);	

			return true;
		}

	private void UpdateAround(Gomoku.Map map, Gomoku.Coord baseCell)
	{
		Gomoku.Color color = map.GetColor (baseCell.x, baseCell.y);
		Gomoku.Color enemy = (color == Gomoku.Color.White) ? Gomoku.Color.Black : Gomoku.Color.White;
		Gomoku.Coord currentCell = new Gomoku.Coord ();

		foreach (KeyValuePair<Gomoku.Orientation, int[]> entry in MapComponent.ORIENTATION) {
			currentCell.x = baseCell.x + entry.Value [0];
			currentCell.y = baseCell.y + entry.Value [1];

			if (currentCell.x >= 0 && currentCell.x < Gomoku.Map.GetSizeMap() && 
			    currentCell.y >= 0 && currentCell.y < Gomoku.Map.GetSizeMap() && 
			    map.GetColor (currentCell.x, currentCell.y) == enemy) {
				SetIsTaking (map, currentCell, enemy);
			}
			
			currentCell.x = baseCell.x + 2 * entry.Value [0];
			currentCell.y = baseCell.y + 2 * entry.Value [1];
			if (currentCell.x >= 0 && currentCell.x < Gomoku.Map.GetSizeMap() && 
			    currentCell.y >= 0 && currentCell.y < Gomoku.Map.GetSizeMap() && 
			    map.GetColor (currentCell.x, currentCell.y) == enemy) {
				SetIsTaking (map, currentCell, enemy);
			}
			
			currentCell.x = baseCell.x + 3 * entry.Value [0];
			currentCell.y = baseCell.y + 3 * entry.Value [1];
			if (currentCell.x >= 0 && currentCell.x < Gomoku.Map.GetSizeMap() && 
			    currentCell.y >= 0 && currentCell.y < Gomoku.Map.GetSizeMap() && 
			    map.GetColor (currentCell.x, currentCell.y) == enemy) {
				SetIsTaking (map, currentCell, enemy);
			}
			
		}
	}

	private void SetIsTaking (Gomoku.Map map, Gomoku.Coord currentCell, Gomoku.Color color)
	{	
		map.SetIsTaking (currentCell.x, currentCell.y, Orientation.EAST, IsTaking (map, MapComponent.ORIENTATION [Orientation.EAST] [0], MapComponent.ORIENTATION [Orientation.EAST] [1], currentCell, color));
		map.SetIsTaking (currentCell.x, currentCell.y, Orientation.WEST, IsTaking (map, MapComponent.ORIENTATION [Orientation.WEST] [0], MapComponent.ORIENTATION [Orientation.WEST] [1], currentCell, color));
		map.SetIsTaking (currentCell.x, currentCell.y, Orientation.NORTH, IsTaking (map, MapComponent.ORIENTATION [Orientation.NORTH] [0], MapComponent.ORIENTATION [Orientation.NORTH] [1], currentCell, color));
		map.SetIsTaking (currentCell.x, currentCell.y, Orientation.SOUTH, IsTaking (map, MapComponent.ORIENTATION [Orientation.SOUTH] [0], MapComponent.ORIENTATION [Orientation.SOUTH] [1], currentCell, color));
		
		map.SetIsTaking (currentCell.x, currentCell.y, Orientation.NORTHEAST, IsTaking (map, MapComponent.ORIENTATION [Orientation.NORTHEAST] [0], MapComponent.ORIENTATION [Orientation.NORTHEAST] [1], currentCell, color));
		map.SetIsTaking (currentCell.x, currentCell.y, Orientation.SOUTHEAST, IsTaking (map, MapComponent.ORIENTATION [Orientation.SOUTHEAST] [0], MapComponent.ORIENTATION [Orientation.SOUTHEAST] [1], currentCell, color));
		map.SetIsTaking (currentCell.x, currentCell.y, Orientation.NORTHWEST, IsTaking (map, MapComponent.ORIENTATION [Orientation.NORTHWEST] [0], MapComponent.ORIENTATION [Orientation.NORTHWEST] [1], currentCell, color));
		map.SetIsTaking (currentCell.x, currentCell.y, Orientation.SOUTHWEST, IsTaking (map, MapComponent.ORIENTATION [Orientation.SOUTHWEST] [0], MapComponent.ORIENTATION [Orientation.SOUTHWEST] [1], currentCell, color));
		
	}

	private bool IsTaking (Gomoku.Map map, int wayX, int wayY, Gomoku.Coord currentCell, Gomoku.Color color)
	{
		Gomoku.Color otherColor = (color == Gomoku.Color.White) ? Gomoku.Color.Black : Gomoku.Color.White;
		int x = currentCell.x + wayX;
		int y = currentCell.y + wayY;
		int nbOtherColor = 0;
		
		while (x >= 0 && y >= 0 &&
		       x < Gomoku.Map.GetSizeMap() && y < Gomoku.Map.GetSizeMap()) {
			if (map.GetColor (x, y) != otherColor && nbOtherColor == 2) {
				map.SetIsTakeable (x - wayX, y - wayY, true);
				map.SetIsTakeable (x - 2 * wayX, y - 2 * wayY, true);
				return true;
			}
			if (map.GetColor (x, y) != otherColor)
				return false;
			map.SetIsTakeable (x, y, false);
			nbOtherColor++;
			x += wayX;
			y += wayY;
		}
		return false;
	}


	// voir pour passer en recursif
	private void UpdateCellData (Gomoku.Orientation orientation, Gomoku.Coord currentCell, Gomoku.Map map)
	{
		Gomoku.Color color = map.GetColor (currentCell.x, currentCell.y);
		int x;
		int y;
		int weight = 1;
		
		x = currentCell.x + MapComponent.ORIENTATION [orientation] [0];
		y = currentCell.y + MapComponent.ORIENTATION [orientation] [1];
		while (x >= 0 && y >= 0 &&
		       x < Gomoku.Map.GetSizeMap() && y < Gomoku.Map.GetSizeMap() && map.GetColor(x, y) == color) {
			weight++;
			x += MapComponent.ORIENTATION [orientation] [0];
			y += MapComponent.ORIENTATION [orientation] [1];
		}

		x = currentCell.x - MapComponent.ORIENTATION [orientation] [0];
		y = currentCell.y - MapComponent.ORIENTATION [orientation] [1];
		while (x >= 0 && y >= 0 &&
		       x < Gomoku.Map.GetSizeMap() && y < Gomoku.Map.GetSizeMap() && map.GetColor(x, y) == color) {
			weight++;
			x -= MapComponent.ORIENTATION [orientation] [0];
			y -= MapComponent.ORIENTATION [orientation] [1];
		}


		x = currentCell.x + MapComponent.ORIENTATION [orientation] [0];
		y = currentCell.y + MapComponent.ORIENTATION [orientation] [1];
		while (x >= 0 && y >= 0 &&
		       x < Gomoku.Map.GetSizeMap() && y < Gomoku.Map.GetSizeMap() && map.GetColor(x, y) == color) {
			if (map.GetWeight (x, y, color) < weight)
				map.SetWeight (x, y, weight, color);
			x += MapComponent.ORIENTATION [orientation] [0];
			y += MapComponent.ORIENTATION [orientation] [1];
		}
		if (x < Gomoku.Map.GetSizeMap() && x >= 0 &&
		    y < Gomoku.Map.GetSizeMap() && y >= 0 && 
		    map.GetWeight (x, y, color) < weight)
			map.SetWeight (x, y, weight, color);


		x = currentCell.x - MapComponent.ORIENTATION [orientation] [0];
		y = currentCell.y - MapComponent.ORIENTATION [orientation] [1];
		while (x >= 0 && y >= 0 &&
		       x < Gomoku.Map.GetSizeMap() && y < Gomoku.Map.GetSizeMap() && map.GetColor(x, y) == color) {
			if (map.GetWeight (x, y, color) < weight)
				map.SetWeight (x, y, weight, color);
			x -= MapComponent.ORIENTATION [orientation] [0];
			y -= MapComponent.ORIENTATION [orientation] [1];
		}
		if (x < Gomoku.Map.GetSizeMap() && x >= 0 &&
		    y < Gomoku.Map.GetSizeMap() && y >= 0 && 
		    map.GetWeight (x, y, color) < weight)
			map.SetWeight (x, y, weight, color);

		if (map.GetWeight (currentCell.x, currentCell.y, color) < weight)
			map.SetWeight (currentCell.x, currentCell.y, weight, color);
	}


		public void Scoring (Gomoku.Color remover, int score)
		{
				scores [remover] += score;
		}

		public bool CanTakePawns (MapComponent map, int x, int y, Gomoku.Orientation orientation)
		{
				return (map.GetMap ().IsTaking (x, y, orientation) && 
						map.GetMap ().GetColor (x + 3 * MapComponent.ORIENTATION [orientation] [0], y + 3 * MapComponent.ORIENTATION [orientation] [1]) == map.GetMap ().GetColor (x, y));
		}

		public void TakePawns (MapComponent map, int x, int y, Gomoku.Orientation orientation)
		{
				Scoring ((Gomoku.Color)map.GetMap ().GetColor (x, y), 2);
				map.GetMap ().SetIsTaking (x, y, orientation, false);
				map.GetMap ().SetIsTaking (x + 3 * MapComponent.ORIENTATION [orientation] [0], y + 3 * MapComponent.ORIENTATION [orientation] [1], orientation, false);
				map.removePawn (x + MapComponent.ORIENTATION [orientation] [0], y + MapComponent.ORIENTATION [orientation] [1]);
				map.removePawn (x + 2 * MapComponent.ORIENTATION [orientation] [0], y + 2 * MapComponent.ORIENTATION [orientation] [1]);
		}

		//regarde si alignement de 5
		public Gomoku.Color IsWinner (Gomoku.Map map)
		{
				Gomoku.Color win = Gomoku.Color.Empty;

				for (int x = 0; x < Gomoku.Map.GetSizeMap(); ++x) {
						for (int y = 0; y < Gomoku.Map.GetSizeMap(); ++y) {
								win = WeightToFive (x, y, map);
									if (win != Gomoku.Color.Empty) {
										print ("x = " + x + " y = " + y);
										print ("weight = " + map.GetWeight(x, y, win));
										if (!FiveBreakable || !CheckIsBreakable (map, x, y))
												return win;
								}
				
						}
				}
				return  Gomoku.Color.Empty;
	}

		private bool CheckIsBreakable (Gomoku.Map map, int x, int y)
		{
				print ("checkIsBreakable EAST");
				bool right = IsBreakable (map, x, y, Gomoku.Orientation.EAST);
				print ("checkIsBreakable NORTH");
				bool up = IsBreakable (map, x, y, Gomoku.Orientation.NORTH);
				bool rightUp = IsBreakable (map, x, y, Gomoku.Orientation.NORTHEAST);
				print ("checkIsBreakable NORTHEAST");
				bool leftUp = IsBreakable (map, x, y, Gomoku.Orientation.NORTHWEST);
				print ("checkIsBreakable NORTHWEST");

				if (!right || !up || !rightUp || !leftUp)
						return false;
				return true;
		}

		private bool IsBreakable (Gomoku.Map map, int x, int y, Gomoku.Orientation orientation)
		{
				int countPawn = 0;
				Gomoku.Color color = map.GetColor (x, y);

				while (x >= 0 && y >= 0 &&
		              y < Gomoku.Map.GetSizeMap() && x < Gomoku.Map.GetSizeMap()
		              && map.GetColor(x, y) == color) {
						x -= MapComponent.ORIENTATION [orientation] [0];
						y -= MapComponent.ORIENTATION [orientation] [1];
				}

		if (!(x < Gomoku.Map.GetSizeMap() && x >= 0 &&
		      y < Gomoku.Map.GetSizeMap() && y >= 0) || map.GetColor (x, y) != color) {
			x += MapComponent.ORIENTATION [orientation] [0];
						y += MapComponent.ORIENTATION [orientation] [1];		
				}
				
				while (x >= 0 && y >= 0 &&
		       		y < Gomoku.Map.GetSizeMap() && x < Gomoku.Map.GetSizeMap()
		      	 	&& map.GetColor(x, y) == color && countPawn < 5) {
					//	print ("x = " + x + " y = " + y + " countPawn = " + countPawn);
						//if (IsTakingAroundMe (map, x, y))
						if (map.IsTakeable(x, y))
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
						if (x - entry.Value [0] >= 0 && x - entry.Value [0] < Gomoku.Map.GetSizeMap() && 
								y - entry.Value [1] >= 0 && y - entry.Value [1] < Gomoku.Map.GetSizeMap() && 
								map.GetMap ().GetColor (x - entry.Value [0], y - entry.Value [1]) == enemy) {
								if (map.GetMap ().IsTaking (x - entry.Value [0], y - entry.Value [1], entry.Key)) {
										return true;
								}	
						}
				}
				return false;
		}

		private Gomoku.Color WeightToFive (int x, int y, Gomoku.Map map)
		{
				if (map.GetColor(x, y) == Gomoku.Color.Black && map.GetWeight (x, y, Gomoku.Color.Black) >= 5) 
						return Gomoku.Color.Black;
				else if (map.GetColor(x, y) == Gomoku.Color.White && map.GetWeight (x, y, Gomoku.Color.White) >= 5)
						return Gomoku.Color.White;
		return Gomoku.Color.Empty;
		}

		public Gomoku.Color IsScoringWinner ()
		{
				if (scores [Gomoku.Color.White] == MAX_SCORE)
						return Gomoku.Color.White;
				if (scores [Gomoku.Color.Black] == MAX_SCORE)
						return Gomoku.Color.Black;
				return Gomoku.Color.Empty;
		}

	private int GetColorMod (Gomoku.Map map, int x, int y, Gomoku.Color color)
	{
		
		if (!(x >= 0 && y >= 0 && x < Gomoku.Map.GetSizeMap() && y < Gomoku.Map.GetSizeMap()))
			return -1;
		if (map.GetColor (x, y) == color)
			return 2;
		else if (map.GetColor (x, y) == Gomoku.Color.Empty)
			return 0;
		else
			return -1;
	}
	
	private bool TestMask (Gomoku.Map map, int[] mask, Gomoku.Color color, Gomoku.Coord cell, Gomoku.Orientation orientation)
	{
		
		if (mask [0] != -1 && GetColorMod (map, cell.x + -4 * MapComponent.ORIENTATION [orientation] [0], cell.y + -4 * MapComponent.ORIENTATION [orientation] [1], color) != mask [0])
			return false;
		if (mask [1] != -1 && GetColorMod (map, cell.x + -3 * MapComponent.ORIENTATION [orientation] [0], cell.y + -3 * MapComponent.ORIENTATION [orientation] [1], color) != mask [1])
			return false;
		if (mask [2] != -1 && GetColorMod (map, cell.x + -2 * MapComponent.ORIENTATION [orientation] [0], cell.y + -2 * MapComponent.ORIENTATION [orientation] [1], color) != mask [2])
			return false;
		if (mask [3] != -1 && GetColorMod (map, cell.x + -1 * MapComponent.ORIENTATION [orientation] [0], cell.y + -1 * MapComponent.ORIENTATION [orientation] [1], color) != mask [3])
			return false;
		if (mask [5] != -1 && GetColorMod (map, cell.x + 1 * MapComponent.ORIENTATION [orientation] [0], cell.y + 1 * MapComponent.ORIENTATION [orientation] [1], color) != mask [5])
			return false;
		if (mask [6] != -1 && GetColorMod (map, cell.x + 2 * MapComponent.ORIENTATION [orientation] [0], cell.y + 2 * MapComponent.ORIENTATION [orientation] [1], color) != mask [6])
			return false;
		if (mask [7] != -1 && GetColorMod (map, cell.x + 3 * MapComponent.ORIENTATION [orientation] [0], cell.y + 3 * MapComponent.ORIENTATION [orientation] [1], color) != mask [7])
			return false;
		if (mask [8] != -1 && GetColorMod (map, cell.x + 4 * MapComponent.ORIENTATION [orientation] [0], cell.y + 4 * MapComponent.ORIENTATION [orientation] [1], color) != mask [8])
			return false;
		return true;
	}
	
	public bool FindDoubleThree (Gomoku.Map map, Gomoku.Coord currentCell, Gomoku.Color color)
	{
		int j;
		bool findMask = false;
		bool breako = false;
		List<int []> threeFree = new List<int[]> ();
		List<int []> threeFreeNext = new List<int[]> ();

		
		foreach (int[] mask in this.masks) {
			foreach (KeyValuePair<Gomoku.Orientation, int[]> orientation in MapComponent.ORIENTATION) {
				if (TestMask (map, mask, color, currentCell, orientation.Key)) {
					findMask = true;
					for (j=0; j < mask.Length; j++) {
						if (mask [j] == 2) {
							threeFree.Add (new int[] {
								currentCell.x + (j - 4) * MapComponent.ORIENTATION [orientation.Key] [0],
								currentCell.y + (j - 4) * MapComponent.ORIENTATION [orientation.Key] [1]
							});
						}
						if (mask [j] == 1) {
							threeFree.Add (new int[] { currentCell.x, currentCell.y });
						}
					}
					/*
						print ("==========");
						print ("find three !! 1");
						print (orientation.Key);
						foreach (int [] cell in threeFree) {
							print ("x = " + cell [0] + " y = " + cell [1]);
						}
						print ("==========");
						*/
					breako = true;
					break;
				}
				threeFree.Clear();
			}
			if (breako)
				break;
			threeFree.Clear();
		}
		if (!findMask) 
			return false;
		
		foreach (int [] cell in threeFree) {
			foreach (int[] mask in this.masks) {
				foreach (KeyValuePair<Gomoku.Orientation, int[]> orientation in MapComponent.ORIENTATION) {
					Gomoku.Coord tmpCoord = new Gomoku.Coord();
					tmpCoord.x = cell[0];
					tmpCoord.y = cell[1];

					if (TestMask (map, mask, color, tmpCoord, orientation.Key)) {
						for (j=0; j < mask.Length; j++) {
							if (mask [j] == 2) {
								threeFreeNext.Add (new int[] {
									tmpCoord.x + (j - 4) * MapComponent.ORIENTATION [orientation.Key] [0],
									tmpCoord.y + (j - 4) * MapComponent.ORIENTATION [orientation.Key] [1]
								});
							}
							if (mask [j] == 1) {
								threeFreeNext.Add (new int[] { tmpCoord.x, tmpCoord.y });
							}
						}
						if (TestPrecThree (threeFree, threeFreeNext)) {
							/*
								print ("==========");
								print ("x = " + cell [0] + " y = " + cell [1]);
								print ("find three !! 2");
								print (orientation.Key);
								foreach (int [] cell2 in threeFreeNext) {
									print ("x = " + cell2 [0] + " y = " + cell2 [1]);
									print (map.GetColor(cell2 [0], cell2 [1]));
								}
								print ("==========");
								*/
							return true;
						}
					}
					threeFreeNext.Clear();
				}
				threeFreeNext.Clear();
			}
			threeFreeNext.Clear();
		}
		
		return false;
	}
	
	public bool TestPrecThree (List<int[]> a, List<int[]> b)
	{
		for (int j=0; j < a.Count; j++) {
			if (!((a [j] [0] == b [j] [0] && a [j] [1] == b [j] [1]) || (a [j] [0] == b [a.Count - j - 1] [0] && a [j] [1] == b [a.Count - j - 1] [1])))
				return true;
		}
		return false;
	}
	
	public bool IsDoubleThree (Gomoku.Map map, int x, int y, Gomoku.Color color)
	{
		map.PutPawn (x, y, color);
		Gomoku.Coord cell = new Gomoku.Coord ();
		cell.x = x;
		cell.y = y;

		bool ret = FindDoubleThree (map, cell, color);
		map.RemovePawn (x, y);
		
		return ret;
	}
}
