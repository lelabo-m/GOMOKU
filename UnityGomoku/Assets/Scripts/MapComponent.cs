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
		public char[] map;
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
			{ O_RIGHT , new int[] { 0 , 1 } },
			{ O_LEFT , new int[] { 0 , -1 } },
			{ O_UP , new int[] { 1 , 0 } },
			{ O_DOWN , new int[] { -1 , 0 } },
			{ O_RIGHT_UP , new int[] { 1 , 1 } },
			{ O_RIGHT_DOWN , new int[] { -1 , 1 } },
			{ O_LEFT_UP , new int[] { 1 , -1 } },
			{ O_LEFT_DOWN , new int[] { -1 , -1 } }
		};
				if (PlayerPrefs.GetInt ("5 cassables") > 0) {
						print ("Regle des 5 cassables active !");
				}
				if (PlayerPrefs.GetInt ("double 3") > 0) {
						print ("Regle des double 3 active !");
				}
				bitsMap = new BitsMap ();
				map = new char[SIZE_MAP * SIZE_MAP];
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

		public bool putPawn (int x, int y, MapComponent.Color color)
		{
				if (!rules.putPawn (map, x, y))
						return false;
				bitsMap.putPawn (x, y, color);

				currentX = x;
				currentY = y;

				weightPropagation (ORIENTATION [O_RIGHT] [0], ORIENTATION [O_RIGHT] [1], color);
				weightPropagation (ORIENTATION [O_UP] [0], ORIENTATION [O_UP] [1], color);
				weightPropagation (ORIENTATION [O_RIGHT_UP] [0], ORIENTATION [O_RIGHT_UP] [1], color);
				weightPropagation (ORIENTATION [O_LEFT_UP] [0], ORIENTATION [O_LEFT_UP] [1], color);

				setIsTakeable (color);

				map [x * SIZE_MAP + y] = (char)color;
				gameManager.setLastPawn(color, x, y);
				return true;
		}

		private void setIsTakeable (MapComponent.Color color)
		{
				MapComponent.Color otherColor = (color == MapComponent.Color.White) ? MapComponent.Color.Black : MapComponent.Color.White;
			
				bitsMap.setIsTaken (currentX, currentY, O_RIGHT, isTakeable (ORIENTATION [O_RIGHT] [0], ORIENTATION [O_RIGHT] [1], color, otherColor));
				bitsMap.setIsTaken (currentX, currentY, O_LEFT, isTakeable (ORIENTATION [O_LEFT] [0], ORIENTATION [O_LEFT] [1], color, otherColor));
				bitsMap.setIsTaken (currentX, currentY, O_UP, isTakeable (ORIENTATION [O_UP] [0], ORIENTATION [O_UP] [1], color, otherColor));
				bitsMap.setIsTaken (currentX, currentY, O_DOWN, isTakeable (ORIENTATION [O_DOWN] [0], ORIENTATION [O_DOWN] [1], color, otherColor));

				bitsMap.setIsTaken (currentX, currentY, O_RIGHT_UP, isTakeable (ORIENTATION [O_RIGHT_UP] [0], ORIENTATION [O_RIGHT_UP] [1], color, otherColor));
				bitsMap.setIsTaken (currentX, currentY, O_RIGHT_DOWN, isTakeable (ORIENTATION [O_RIGHT_DOWN] [0], ORIENTATION [O_RIGHT_DOWN] [1], color, otherColor));
				bitsMap.setIsTaken (currentX, currentY, O_LEFT_UP, isTakeable (ORIENTATION [O_LEFT_UP] [0], ORIENTATION [O_LEFT_UP] [1], color, otherColor));
				bitsMap.setIsTaken (currentX, currentY, O_LEFT_DOWN, isTakeable (ORIENTATION [O_LEFT_DOWN] [0], ORIENTATION [O_LEFT_DOWN] [1], color, otherColor));

		}

		private char isTakeable (int wayX, int wayY, MapComponent.Color color, MapComponent.Color otherColor)
		{
				int x = currentX + wayX;
				int y = currentY + wayY;
				int nbOtherColor = 0;

				while (x >= 0 && y >= 0 &&
		     		  x < SIZE_MAP && y < SIZE_MAP) {
						print ("nbColor = " + nbOtherColor);
						if (nbOtherColor == 2 && bitsMap.getColor (x, y) != otherColor)
								return (char)1;
						if (bitsMap.getColor (x, y) != otherColor)
								return (char)0;
						nbOtherColor++;
						x += wayX;
						y += wayY;
				}
				return (char)0;
		}

		private void weightPropagation (int wayX, int wayY, MapComponent.Color color)
		{
				int x = currentX + wayX;
				int y = currentY + wayY;
				int nbPawn = 1;


				while (x >= 0 && y >= 0 &&
		       	x < SIZE_MAP && y < SIZE_MAP && bitsMap.getColor(x, y) == color) {
						nbPawn++;
						x += wayX;
						y += wayY;
				}

				x = currentX - wayX;
				y = currentY - wayY;
				while (x >= 0 && y >= 0 &&
			       x < SIZE_MAP && y < SIZE_MAP && bitsMap.getColor(x, y) == color) {
						nbPawn++;
						x -= wayX;
						y -= wayY;
				}

				x = currentX + wayX;
				y = currentY + wayY;
				while (x >= 0 && y >= 0 &&
			       x < SIZE_MAP && y < SIZE_MAP && bitsMap.getColor(x, y) == color) {
						bitsMap.setWeight (x, y, nbPawn, color);
						x += wayX;
						y += wayY;
				}
				if (x >= 0 && y >= 0 &&
						x < SIZE_MAP && y < SIZE_MAP && bitsMap.getColor (x, y) == MapComponent.Color.Empty) {
						bitsMap.setWeight (x, y, nbPawn, color);
						//print ("x = " + x.ToString () + " y = " + y.ToString () + " nbPawn = " + nbPawn.ToString () + " weight = " + bitsMap.getWeight (x, y, color).ToString ());

				}
			
				x = currentX - wayX;
				y = currentY - wayY;
				while (x >= 0 && y >= 0 &&
			       x < SIZE_MAP && y < SIZE_MAP && bitsMap.getColor(x, y) == color) {
						bitsMap.setWeight (x, y, nbPawn, color);
						x -= wayX;
						y -= wayY;
				}
				if (x >= 0 && y >= 0 &&
						x < SIZE_MAP && y < SIZE_MAP && bitsMap.getColor (x, y) == MapComponent.Color.Empty) {
						bitsMap.setWeight (x, y, nbPawn, color);
						//print ("x = " + x.ToString () + " y = " + y.ToString () + " nbPawn = " + nbPawn.ToString () + " weight = " + bitsMap.getWeight (x, y, color).ToString ());
				}
		


				if (bitsMap.getWeight (currentX, currentY, color) < nbPawn)
						bitsMap.setWeight (currentX, currentY, nbPawn, color);
				//print ("x = " + currentX.ToString () + " y = " + currentY.ToString () + " nbPawn = " + nbPawn.ToString () + " weight = " + bitsMap.getWeight (currentX, currentY, color).ToString ());
		}
	
		public bool removePawn (int x, int y)
		{
				bitsMap.removePawn (x, y);
				map [x * SIZE_MAP + y] = (char) MapComponent.Color.Empty;
				Destroy(GameObject.Find("Pawn_" + (x * SIZE_MAP + y).ToString()));
				return true;
		}

		public char getCell (int x, int y)
		{
				return map [x * SIZE_MAP + y];
		}

		// return map converted
		public char[] getMap ()
		{
				return map;
		}

		public BitsMap getBitsMap ()
		{
				return bitsMap;
		}

		public class BitsMap
		{ 

				private class Cell
				{
						public MapComponent.Color color = MapComponent.Color.Empty;
						public System.Collections.Generic.Dictionary<MapComponent.Color, int> weight;
						public char[] takePawns = new char[8];
			
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

				public void setIsTaken (int x, int y, int orientation, char state)
				{
						print ("takeable: x = " + x + " y = " + y + " orientation = " + orientation + " state = " + ((int)state));
						this._map [x * SIZE_MAP + y].takePawns [orientation] = state;
				}

				public bool isTakeable (int x, int y, int orientation)
				{
						if (this._map [x * SIZE_MAP + y].takePawns [orientation] == 1)
								return true;
						return false;
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
						this._map [x * SIZE_MAP + y].weight[Color.Black] = 0;
						this._map [x * SIZE_MAP + y].weight[Color.White] = 0;
						foreach (KeyValuePair<int, int[]> entry in MapComponent.ORIENTATION)
						{
								setIsTaken(x, y, entry.Key, (char) 0);
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
