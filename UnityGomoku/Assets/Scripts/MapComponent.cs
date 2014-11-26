using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;

public class MapComponent : MonoBehaviour
{
		public GameObject TilePrefab;
		private List <List <Tile>> graphicMap;
		public const int SIZE_MAP = 19;
		public enum Color
		{
				Empty,
				White,
				Black }
		;
		private GameObject arbiter;
		private GameManager gameManager;
		private Rules rules;
		private BitsMap bitsMap;
		private int currentX;
		private int currentY;
		public const int O_RIGHT = 0;
		public const int O_LEFT = 1;
		public const int O_UP = 2;
		public const int O_DOWN = 3;
		public const int O_RIGHT_UP = 4;
		public const int O_RIGHT_DOWN = 5;
		public const int O_LEFT_UP = 6;
		public const int O_LEFT_DOWN = 7;
		public static Dictionary<int , int[]> ORIENTATION ;

	
		// Use this for initialization
		void Start ()
		{
				ORIENTATION = new Dictionary<int, int[]> ()
		{
			{ O_RIGHT , new int[] { 1 , 0 } },
			{ O_LEFT , new int[] { -1 , 0 } },
			{ O_UP , new int[] { 0 , -1 } },
			{ O_DOWN , new int[] { 0 , 1 } },
			{ O_RIGHT_UP , new int[] { 1 , -1 } },
			{ O_RIGHT_DOWN , new int[] { 1 , 1 } },
			{ O_LEFT_UP , new int[] { -1 , -1 } },
			{ O_LEFT_DOWN , new int[] { -1 , 1 } }
		};
				if (PlayerPrefs.GetInt ("5 cassables") > 1) {
						print ("Regle des 5 cassables active !");
				}
				if (PlayerPrefs.GetInt ("double 3") > 1) {
						print ("Regle des double 3 active !");
				}
				bitsMap = new BitsMap ();
				arbiter = GameObject.Find ("Arbiter");
				rules = arbiter.GetComponent<Rules> ();
				gameManager = arbiter.GetComponent<GameManager> ();

				generateGraphicMap ();
		}

		// Update is called once per frame
		void Update ()
		{
	
		}

		private void generateGraphicMap ()
		{
				graphicMap = new List<List<Tile>> ();
				for (int i = 0; i < SIZE_MAP; ++i) {
						List <Tile> row = new List<Tile> ();
						for (int a = 0; a < SIZE_MAP; ++a) {
								Tile tile = ((GameObject)Instantiate (TilePrefab, 
				                                      new Vector3 (i - Mathf.Floor (SIZE_MAP / 2), 0, -a + Mathf.Floor (SIZE_MAP / 2)),
				                                      Quaternion.Euler (new Vector3 ()))).GetComponent<Tile> ();
								tile.name = "Tile_" + (i * SIZE_MAP + a).ToString ();
								tile.setGridPosition (new Vector2 (i, a));
								row.Add (tile);
						}
						graphicMap.Add (row);
				}
		}

		public int getColorMod(int x, int y, MapComponent.Color color){
			
		if (!(x >= 0 && y >= 0 && x < SIZE_MAP && y < SIZE_MAP))
			return -1;
		if (bitsMap.getColor (x, y) == color)
			return 2;
		else if (bitsMap.getColor (x, y) == MapComponent.Color.Empty)
			return 0;
		else
			return -1;
		}

		public bool testMask(int[] mask, MapComponent.Color color, int x, int y, int orientation){
			
			if (mask [0] != -1 && getColorMod (x + -4 * ORIENTATION [orientation] [0], y + -4 * ORIENTATION [orientation] [1], color) != mask [0])
				return false;
			if (mask [1] != -1 && getColorMod (x + -3 * ORIENTATION [orientation] [0], y + -3 * ORIENTATION [orientation] [1], color) != mask [1])
					return false;
			if (mask [2] != -1 && getColorMod (x + -2 * ORIENTATION [orientation] [0], y + -2 * ORIENTATION [orientation] [1], color) != mask [2])
					return false;
			if (mask [3] != -1 && getColorMod (x + -1 * ORIENTATION [orientation] [0], y + -1 * ORIENTATION [orientation] [1], color) != mask [3])
					return false;
			if (mask [5] != -1 && getColorMod (x + 1 * ORIENTATION [orientation] [0], y + 1 * ORIENTATION [orientation] [1], color) != mask [5])
					return false;
			if (mask [6] != -1 && getColorMod (x + 2 * ORIENTATION [orientation] [0], y + 2 * ORIENTATION [orientation] [1], color) != mask [6])
					return false;
			if (mask [7] != -1 && getColorMod (x + 3 * ORIENTATION [orientation] [0], y + 3 * ORIENTATION [orientation] [1], color) != mask [7])
					return false;
			if (mask [8] != -1 && getColorMod (x + 4 * ORIENTATION [orientation] [0], y + 4 * ORIENTATION [orientation] [1], color) != mask [8])
					return false;
			return true;
		}

		public bool findDoubleThree (int x, int y, MapComponent.Color color)
		{
			int j;
			bool findMask = false;
			bool breako = false;
			List<int []> masks = new List<int[]> ();
			List<int []> threeFree = new List<int[]> ();
			List<int []> threeFreeNext = new List<int[]> ();
			

			masks.Add(new int[] { -1, 0, 2, 2, 1, 0, -1, -1, -1});
			masks.Add(new int[] { 0, 2, 2, 0, 1, 0, -1, -1, -1});
			masks.Add(new int[] { 0, 2, 0, 2, 1, 0, -1, -1, -1});
			masks.Add(new int[] { -1, -1, 0, 2, 1, 2, 0, -1, -1});
			masks.Add(new int[] { -1, 0, 2, 0, 1, 2, 0, -1, -1});
			masks.Add(new int[] { -1, -1, 0, 2, 1, 0, 2, 0, -1});
			masks.Add(new int[] { -1, -1, -1, 0, 1, 2, 2, 0, -1});
			masks.Add(new int[] { -1, -1, -1, 0, 1, 2, 0, 2, 0});
			masks.Add(new int[] { -1, -1, -1, 0, 1, 0, 2, 2, 0});

			foreach (int[] mask in masks) {
				foreach (KeyValuePair<int, int[]> orientation in ORIENTATION){
					if (testMask (mask, color, x, y, orientation.Key)){
						findMask = true;
						for(j=0; j < mask.Length; j++)
						{
							if(mask[j]==2){
							threeFree.Add(new int[] { x + (j - 4) * ORIENTATION [orientation.Key] [0], y + (j - 4) * ORIENTATION [orientation.Key] [1] });
							}
							if (mask[j] == 1){
								threeFree.Add(new int[] { x, y });
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
					threeFree = new List<int[]> ();
				}
				if (breako)
					break;
				threeFree = new List<int[]> ();
			}
			if (!findMask) 
				return false;

			foreach (int [] cell in threeFree) {
				foreach (int[] mask in masks) {
					foreach (KeyValuePair<int, int[]> orientation in ORIENTATION){
						if (testMask (mask, color, cell [0], cell [1], orientation.Key)){
							for(j=0; j < mask.Length; j++)
							{
								if(mask[j]==2){
									threeFreeNext.Add(new int[] { cell [0] + (j - 4) * ORIENTATION [orientation.Key] [0], cell [1] + (j - 4) * ORIENTATION [orientation.Key] [1] });
								}
								if (mask[j] == 1){
									threeFreeNext.Add(new int[] { cell[0], cell[1] });
								}
							}
							if (testPrecThree(threeFree, threeFreeNext)){
								/*
								print ("==========");
								print ("x = " + cell [0] + " y = " + cell [1]);
								print ("find three !! 2");
								print (orientation.Key);
								foreach (int [] cell2 in threeFreeNext) {
									print ("x = " + cell2 [0] + " y = " + cell2 [1]);
									print (bitsMap.getColor(cell2 [0], cell2 [1]));
								}
								print ("==========");
								*/
								return true;
							}
						}
						threeFreeNext = new List<int[]> ();
					}
					threeFreeNext = new List<int[]> ();
				}
				threeFreeNext = new List<int[]> ();
			}

		return false;
	}

		public bool testPrecThree(List<int []> a, List<int []> b){
			for (int j=0; j < a.Count; j++) {
			if (!((a[j][0] == b[j][0] && a[j][1] == b[j][1]) || (a[j][0] == b[a.Count - j - 1][0] && a[j][1] == b[a.Count - j - 1][1])))
				return true;
			}
			return false;
		}
	
		public bool isDoubleThree (int x, int y, MapComponent.Color color)
		{
				bitsMap.putPawn (x, y, color);
				
				bool ret =  findDoubleThree (x, y, color);
				bitsMap.removePawn (x, y);
		        
				return ret;
		}
	
		public bool putPawn (int x, int y, MapComponent.Color color)
		{
				if (!rules.putPawn (bitsMap, x, y) || (rules.doubleThree && isDoubleThree (x, y, color)))
						return false;
				bitsMap.putPawn (x, y, color);

				currentX = x;
				currentY = y;

				updateCellData (O_RIGHT, color);
				updateCellData (O_UP, color);
				updateCellData (O_RIGHT_UP, color);
				updateCellData (O_LEFT_UP, color);
				
				setIsTaking (color);

				MapComponent.Color enemy = (color == MapComponent.Color.White) ? MapComponent.Color.Black : MapComponent.Color.White;
				foreach (KeyValuePair<int, int[]> entry in MapComponent.ORIENTATION) {
						currentX = x + entry.Value [0];
						currentY = y + entry.Value [1];
						if (currentX >= 0 && currentX < MapComponent.SIZE_MAP && 
								currentY >= 0 && currentY < MapComponent.SIZE_MAP && 
								bitsMap.getColor (currentX, currentY) == enemy) {
								setIsTaking (enemy);
						}

						currentX = x + 2 * entry.Value [0];
						currentY = y + 2 * entry.Value [1];
						if (currentX >= 0 && currentX < MapComponent.SIZE_MAP && 
								currentY >= 0 && currentY < MapComponent.SIZE_MAP && 
								bitsMap.getColor (currentX, currentY) == enemy) {
								setIsTaking (enemy);
						}

						currentX = x + 3 * entry.Value [0];
						currentY = y + 3 * entry.Value [1];
						if (currentX >= 0 && currentX < MapComponent.SIZE_MAP && 
						    currentY >= 0 && currentY < MapComponent.SIZE_MAP && 
						    bitsMap.getColor (currentX, currentY) == enemy) {
							setIsTaking (enemy);
						}

				}

				gameManager.setLastPawn (color, x, y);
				return true;
		}

		private void setIsTaking (MapComponent.Color color)
		{
				MapComponent.Color otherColor = (color == MapComponent.Color.White) ? MapComponent.Color.Black : MapComponent.Color.White;
			
				bitsMap.setIsTaking (currentX, currentY, O_RIGHT, IsTaking (ORIENTATION [O_RIGHT] [0], ORIENTATION [O_RIGHT] [1], color, otherColor));
				bitsMap.setIsTaking (currentX, currentY, O_LEFT, IsTaking (ORIENTATION [O_LEFT] [0], ORIENTATION [O_LEFT] [1], color, otherColor));
				bitsMap.setIsTaking (currentX, currentY, O_UP, IsTaking (ORIENTATION [O_UP] [0], ORIENTATION [O_UP] [1], color, otherColor));
				bitsMap.setIsTaking (currentX, currentY, O_DOWN, IsTaking (ORIENTATION [O_DOWN] [0], ORIENTATION [O_DOWN] [1], color, otherColor));

				bitsMap.setIsTaking (currentX, currentY, O_RIGHT_UP, IsTaking (ORIENTATION [O_RIGHT_UP] [0], ORIENTATION [O_RIGHT_UP] [1], color, otherColor));
				bitsMap.setIsTaking (currentX, currentY, O_RIGHT_DOWN, IsTaking (ORIENTATION [O_RIGHT_DOWN] [0], ORIENTATION [O_RIGHT_DOWN] [1], color, otherColor));
				bitsMap.setIsTaking (currentX, currentY, O_LEFT_UP, IsTaking (ORIENTATION [O_LEFT_UP] [0], ORIENTATION [O_LEFT_UP] [1], color, otherColor));
				bitsMap.setIsTaking (currentX, currentY, O_LEFT_DOWN, IsTaking (ORIENTATION [O_LEFT_DOWN] [0], ORIENTATION [O_LEFT_DOWN] [1], color, otherColor));

		}

		private char IsTaking (int wayX, int wayY, MapComponent.Color color, MapComponent.Color otherColor)
		{
				int x = currentX + wayX;
				int y = currentY + wayY;
				int nbOtherColor = 0;

				while (x >= 0 && y >= 0 &&
		     		  x < SIZE_MAP && y < SIZE_MAP) {
						if (bitsMap.getColor (x, y) != otherColor && nbOtherColor == 2) {
								bitsMap.setIsTakeable(x - wayX, y - wayY, true);
								bitsMap.setIsTakeable(x - 2 * wayX, y - 2 * wayY, true);
								return (char)1;
						}
						if (bitsMap.getColor (x, y) != otherColor)
								return (char)0;
						bitsMap.setIsTakeable(x, y, false);
						nbOtherColor++;
						x += wayX;
						y += wayY;
				}
				return (char)0;
		}

		// voir pour passer en recursif
		private void updateCellData (int orientation, MapComponent.Color color)
		{
				int x;
				int y;
				int weight = 1;

				x = currentX + ORIENTATION [orientation] [0];
				y = currentY + ORIENTATION [orientation] [1];
				while (x >= 0 && y >= 0 &&
		       	x < SIZE_MAP && y < SIZE_MAP && bitsMap.getColor(x, y) == color) {
						weight++;
						x += ORIENTATION [orientation] [0];
						y += ORIENTATION [orientation] [1];
				}
				
				x = currentX - ORIENTATION [orientation] [0];
				y = currentY - ORIENTATION [orientation] [1];
				while (x >= 0 && y >= 0 &&
			       x < SIZE_MAP && y < SIZE_MAP && bitsMap.getColor(x, y) == color) {
						weight++;
						x -= ORIENTATION [orientation] [0];
						y -= ORIENTATION [orientation] [1];
				}

				x = currentX + ORIENTATION [orientation] [0];
				y = currentY + ORIENTATION [orientation] [1];
				while (x >= 0 && y >= 0 &&
			       x < SIZE_MAP && y < SIZE_MAP && bitsMap.getColor(x, y) == color) {
					if (bitsMap.getWeight (x, y, color) < weight)
						bitsMap.setWeight (x, y, weight, color);
						x += ORIENTATION [orientation] [0];
						y += ORIENTATION [orientation] [1];
				}
			
				x = currentX - ORIENTATION [orientation] [0];
				y = currentY - ORIENTATION [orientation] [1];
				while (x >= 0 && y >= 0 &&
			       x < SIZE_MAP && y < SIZE_MAP && bitsMap.getColor(x, y) == color) {
					if (bitsMap.getWeight (x, y, color) < weight)
						bitsMap.setWeight (x, y, weight, color);
						x -= ORIENTATION [orientation] [0];
						y -= ORIENTATION [orientation] [1];
				}

				if (bitsMap.getWeight (currentX, currentY, color) < weight)
						bitsMap.setWeight (currentX, currentY, weight, color);
						
		}
	
		public bool removePawn (int x, int y)
		{
				bitsMap.removePawn (x, y);
				Destroy (GameObject.Find ("Pawn_" + (x * SIZE_MAP + y).ToString ()));
				return true;
		}
	
		public BitsMap getBitsMap ()
		{
				return bitsMap;
		}

		public static int FindOrientation (int wayX, int wayY)
		{
				foreach (KeyValuePair<int, int[]> entry in MapComponent.ORIENTATION) {
						if (entry.Value [0] == wayX && entry.Value [1] == wayY)
								return entry.Key;
				}
				return -1;
		}


		
		public class BitsMap
		{ 

				private class Cell
				{
						public MapComponent.Color color = MapComponent.Color.Empty;
						public System.Collections.Generic.Dictionary<MapComponent.Color, int> weight;
						public char[] takePawns = new char[8];
						public bool takeable;
			
						public Cell ()
						{		
								weight = new System.Collections.Generic.Dictionary<MapComponent.Color, int> ();
								weight.Add (MapComponent.Color.Black, 0);
								weight.Add (MapComponent.Color.White, 0);
								
						}
				}


				private int[] map = new int[SIZE_MAP * SIZE_MAP];
				private Cell[] _map = new Cell[SIZE_MAP * SIZE_MAP];

				public BitsMap ()
				{
						for (int i = 0; i < SIZE_MAP * SIZE_MAP; ++i) {
								_map [i] = new Cell ();
						}
				}

				// set Weight of a color 
				public void setWeight (int x, int y, int weight, MapComponent.Color color)
				{
						this._map [x * SIZE_MAP + y].weight [color] = weight;
						if (color != Color.Empty) {
								BitVector32 param = new BitVector32 (weight);
								BitVector32 cell = new BitVector32 (this.map [x * SIZE_MAP + y]);
				
								for (int cellC = (color == Color.Black) ? 3 : 0, cellE = cellC + 3, paramC = 0; cellC < cellE; cellC++, paramC++)
										cell [this.getMask (ref cell, cellC)] = param [this.getMask (ref param, paramC)];
								this.map [x * SIZE_MAP + y] = cell.Data;
						}
				}

				public void setIsTakeable(int x, int y, bool state)
				{
					this._map [x * SIZE_MAP + y].takeable = state;
				}

				public bool isTakeable(int x, int y)
				{
					return this._map [x * SIZE_MAP + y].takeable;
				}

				public void setIsTaking (int x, int y, int orientation, char state)
				{
						this._map [x * SIZE_MAP + y].takePawns [orientation] = state;
				}

				public bool isTaking (int x, int y, int orientation)
				{
						return  (this._map [x * SIZE_MAP + y].takePawns [orientation % 8] == 1);
				}

				public int getWeight (int x, int y, MapComponent.Color color)
				{
						return this._map [x * SIZE_MAP + y].weight [color];
						BitVector32 cell;
						BitVector32 value;
			
						if (color == Color.Empty)
								return -1;
						cell = new BitVector32 (this.map [x * SIZE_MAP + y]);
						value = new BitVector32 (0);
						for (int cellC = (color == Color.Black) ? 3 : 0, cellE = cellC + 3, valueC = 0; cellC < cellE; cellC++, valueC++)
								value [this.getMask (ref value, valueC)] = cell [this.getMask (ref cell, cellC)];
						return value.Data;
				}

				public bool putPawn (int x, int y, MapComponent.Color type)
				{
						this._map [x * SIZE_MAP + y].color = type;
						this.map [x * SIZE_MAP + y] = ((char)type << 6);
						return true;
				}

				public bool removePawn (int x, int y)
				{
						this._map [x * SIZE_MAP + y].color = 0;
						this._map [x * SIZE_MAP + y].weight [Color.Black] = 0;
						this._map [x * SIZE_MAP + y].weight [Color.White] = 0;
						this._map [x * SIZE_MAP + y].takeable = false;
						foreach (KeyValuePair<int, int[]> entry in MapComponent.ORIENTATION) {
								setIsTaking (x, y, entry.Key, (char)0);
						}
						this.map [x * SIZE_MAP + y] = 0;
						return true;
				}

				public int getCell (int x, int y)
				{
						return map [x * SIZE_MAP + y];
				}

				public MapComponent.Color getColor (int x, int y)
				{
						return this._map [x * SIZE_MAP + y].color;
						//return (MapComponent.Color)(this.map [x * SIZE_MAP + y] >> 6);
				}

				private int getMask (ref BitVector32 v, int index)
				{
						int i;
						int mask;

						for (mask = BitVector32.CreateMask(), i = 0; i < index; mask = BitVector32.CreateMask(mask), i++)
								;
						return mask;
				}
		}
}
