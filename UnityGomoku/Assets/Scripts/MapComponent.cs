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
		public char[] map;
	
		// Use this for initialization
		void Start ()
		{
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

		public bool putPawn (int x, int y, MapComponent.Color type)
		{
				if (!rules.putPawn (map, x, y))
						return false;
				bitsMap.putPawn (x, y, type);

				currentX = x;
				currentY = y;
				weightPropagation (0, 1, type);
				weightPropagation (1, 1, type);
				weightPropagation (1, 0, type);
				weightPropagation (1, -1, type);

				map [x * SIZE_MAP + y] = (char)type;
				gameManager.setLastColor (type);
				return true;
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
			/*	if (x >= 0 && y >= 0 &&
						x < SIZE_MAP && y < SIZE_MAP && bitsMap.getColor (x, y) == MapComponent.Color.Empty)
						bitsMap.setWeight (x, y, nbPawn, color);*/

				x = currentX - wayX;
				y = currentY - wayY;
				while (x >= 0 && y >= 0 &&
			       x < SIZE_MAP && y < SIZE_MAP && bitsMap.getColor(x, y) == color) {
						bitsMap.setWeight (x, y, nbPawn, color);
						x -= wayX;
						y -= wayY;
				}
				/*if (x >= 0 && y >= 0 &&
						x < SIZE_MAP && y < SIZE_MAP && bitsMap.getColor (x, y) == MapComponent.Color.Empty)
						bitsMap.setWeight (x, y, nbPawn, color);*/


				if (bitsMap.getWeight(currentX, currentY, color) < nbPawn)
					bitsMap.setWeight (currentX, currentY, nbPawn, color);
		print ("x = " + currentX.ToString() + " y = " + currentY.ToString() + " nbPawn = " + nbPawn.ToString() + " weight = " + bitsMap.getWeight(currentX, currentY, color).ToString());
		}

		public bool removePawn (int x, int y)
		{
				bitsMap.removePawn (x, y);
				map [x * SIZE_MAP + y] = (char)MapComponent.Color.Empty;
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
						/*if (color != MapComponent.Color.Empty) {
								BitVector32 param = new BitVector32 (weight);
								BitVector32 cell = new BitVector32 (this.map [x * SIZE_MAP + y]);

								for (int cellC = (color == MapComponent.Color.Black) ? 3 : 0, cellE = cellC + 3, paramC = 0; cellC < cellE; cellC++, paramC++)
										cell [cellC] = param [paramC];
								this.map [x * SIZE_MAP + y] = cell.Data;
						}*/
				}

				public int getWeight (int x, int y, MapComponent.Color color)
				{
						return this._map [x * SIZE_MAP + y].weight [color];
						/*BitVector32 cell;
						BitVector32 value;

						if (color == MapComponent.Color.Empty)
								return 0;
						cell = new BitVector32 (this.map [x * SIZE_MAP + y]);
						value = new BitVector32 (0);
						for (int cellC = (color == MapComponent.Color.Black) ? 3 : 0, cellE = cellC + 3, valueC = 0; cellC < cellE; cellC++, valueC++)
								value [valueC] = cell [cellC];
						return value.Data;*/
				}

				public bool putPawn (int x, int y, MapComponent.Color type)
				{
						this._map [x * SIZE_MAP + y].color = type;
						this.map [x * SIZE_MAP + y] = ((char)type << 6);
			for (int i = 0; i < SIZE_MAP * SIZE_MAP; ++i) {
				if (_map[i].weight[type] != 0)
					print (_map[i].weight[type]);			
			}
						return true;
				}

				public bool removePawn (int x, int y)
				{
						this._map [x * SIZE_MAP + y].color = 0;
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
		}
}
