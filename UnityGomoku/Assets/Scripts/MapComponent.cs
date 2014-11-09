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
				if (PlayerPrefs.GetInt ("5 cassables") > 1) {
						print ("Regle des 5 cassables active !");
				}
				if (PlayerPrefs.GetInt ("double 3") > 1) {
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
	
		public bool isDoubleThree (int x, int y, MapComponent.Color color)
		{
				bitsMap.putPawn (x, y, color);
			
				bool right = isDoubleFree (x, y, O_RIGHT);
				bool up = isDoubleFree (x, y, O_UP);
				bool rightUp = false;//isDoubleFree (x, y, O_RIGHT_UP);
				bool leftUp = false;//isDoubleFree (x, y, O_LEFT_UP);
		       
				bitsMap.removePawn (x, y);
		        
				return (right || up || rightUp || leftUp);
		}

		private List<int[]> getEmptyCellsOnLine (int x, int y, int orientation, MapComponent.Color color)
		{
				List<int []> empty = new List<int[]> ();
				int nbEmpty;
				int countColor;
				bool lastEmpty;
				bool alreadyEmpty;
				MapComponent.Color otherColor = (color == MapComponent.Color.Black) ? MapComponent.Color.White : MapComponent.Color.Black;

				
				print ("beginX = " + x + " beginY = " + y);
				currentX = x + ORIENTATION [orientation] [0];	
				currentY = y + ORIENTATION [orientation] [1];
				nbEmpty = 0;
				lastEmpty = false;
				alreadyEmpty = false;
				countColor = 0;
				while (currentX >= 0 && currentY >= 0 &&
		       currentX < SIZE_MAP && currentY < SIZE_MAP && 
		       bitsMap.getColor (currentX, currentY) != otherColor &&
		       countColor < 4) {



						print ("currentX = " + currentX + " currentY = " + currentY);
						print (bitsMap.getColor (currentX, currentY));

						MapComponent.Color currentColor = bitsMap.getColor (currentX, currentY);
						
						if (currentColor == color) {
								countColor++;
						}
						if (currentColor == MapComponent.Color.Empty) {
				if ((countColor != 3 && alreadyEmpty == true) || ( currentX - ORIENTATION [orientation] [0] > 0 && currentY - ORIENTATION [orientation] [1] >= 0 &&
				                                                  currentX - ORIENTATION [orientation] [0] < SIZE_MAP && currentY - ORIENTATION [orientation] [1] < SIZE_MAP &&
										bitsMap.getColor (currentX - ORIENTATION [orientation] [0], currentY - ORIENTATION [orientation] [1]) == MapComponent.Color.Empty)) {
										break;
								}
								int [] temp = new int[] { currentX, currentY };
								empty.Add (temp);
								alreadyEmpty = true;
								nbEmpty++;
						}
						
						currentX += ORIENTATION [orientation] [0];
						currentY += ORIENTATION [orientation] [1];
				}

				currentX = x - ORIENTATION [orientation] [0];	
				currentY = y - ORIENTATION [orientation] [1];
				nbEmpty = 0;
				lastEmpty = false;
				alreadyEmpty = false;
				countColor = 0;
				while (currentX >= 0 && currentY >= 0 &&
		       currentX < SIZE_MAP && currentY < SIZE_MAP && 
		       bitsMap.getColor (currentX, currentY) != otherColor &&
		       countColor < 4) {
			
			
			
						print ("currentX = " + currentX + " currentY = " + currentY);
						print (bitsMap.getColor (currentX, currentY));
			
						MapComponent.Color currentColor = bitsMap.getColor (currentX, currentY);
			
						if (currentColor == color) {
								countColor++;
						}
						if (currentColor == MapComponent.Color.Empty) {
				if ((countColor != 3 && alreadyEmpty == true) || ( currentX + ORIENTATION [orientation] [0] > 0 && currentY + ORIENTATION [orientation] [1] >= 0 &&
				    currentX + ORIENTATION [orientation] [0] < SIZE_MAP && currentY + ORIENTATION [orientation] [1] < SIZE_MAP &&
										bitsMap.getColor (currentX + ORIENTATION [orientation] [0], currentY + ORIENTATION [orientation] [1]) == MapComponent.Color.Empty)) {
										break;
								}
								int [] temp = new int[] { currentX, currentY };
								empty.Add (temp);
								alreadyEmpty = true;
								nbEmpty++;
						}
			
						currentX -= ORIENTATION [orientation] [0];
						currentY -= ORIENTATION [orientation] [1];
				}


				
				print ("==================empty==================");
				print (empty.Count);
				foreach (int [] cell in empty) {
						print ("x = " + cell [0] + " y = " + cell [1]);
				}

				return empty;
		}

		private bool isDoubleFree (int x, int y, int orientation)
		{
				MapComponent.Color color = bitsMap.getColor (x, y);
				List<List<int[]>> freeLines = threeFree (x, y, orientation, color);

				foreach (List<int[]> threeLine in freeLines) {
						foreach (int[] element in threeLine) {
								print ("=====================check======================");
								/*if (bitsMap.getColor (element [0], element [1]) == MapComponent.Color.Empty)
										bitsMap.putPawn (element [0], element [1], bitsMap.getColor (x, y));*/
								foreach (KeyValuePair<int, int[]> entry in MapComponent.ORIENTATION) {
										if (!((entry.Value [0] == MapComponent.ORIENTATION [orientation] [0] && entry.Value [1] == MapComponent.ORIENTATION [orientation] [1]) ||
												(entry.Value [0] == -(MapComponent.ORIENTATION [orientation] [0]) && entry.Value [1] == -(MapComponent.ORIENTATION [orientation] [1])))) {

												List<List<int[]>> line = threeFree (element [0], element [1], entry.Key, color);

												print ("orientation = " + orientation);
												print ("key = " + entry.Key);
												print ("=====================Line====================");
												print (line.Count);
												foreach (List<int[]> elem in line) {
														print ("threeFree");
														print (elem.Count);
														foreach (int [] cell in elem) {
																print ("x = " + cell [0] + " y = " + cell [1]);
														}
												}
												if (line.Count > 0) {
														print ("=========================================\n==================Double=================\n=========================================");
														//bitsMap.removePawn (element [0], element [1]);
														return true;
												}
										}
								}
								//bitsMap.removePawn (element [0], element [1]);		
						}
				}
				return false;
		}

		private List<List<int[]>> threeFree (int x, int y, int orientation, MapComponent.Color color)
		{
				List<int[]> emptyCells = getEmptyCellsOnLine (x, y, orientation, color);
				List<List<int[]>> threeFree = new List<List<int[]>> ();
				MapComponent.Color otherColor = (color == MapComponent.Color.Black) ? MapComponent.Color.White : MapComponent.Color.Black;


				foreach (int[] element in emptyCells) {
						List<int[]> pattern = new List<int[]> ();
						int[] temp;
						int countColor;
						int countEmpty;
						bool done;
						bool alreadyEmpty;
						bool lastEmpty;

						temp = new int[] { element [0], element [1] };
						pattern.Add (temp);
						alreadyEmpty = false;
						countColor = 0;
						countEmpty = 1;
						currentX = element [0] + ORIENTATION [orientation] [0];
						currentY = element [1] + ORIENTATION [orientation] [1];
						while (currentX >= 0 && currentY >= 0 &&
			       				currentX < SIZE_MAP && currentY < SIZE_MAP
			      				 && bitsMap.getColor (currentX, currentY) != otherColor &&
			       				countColor < 4) {
								
								MapComponent.Color currentColor = bitsMap.getColor (currentX, currentY);

								if (currentColor == color) {
										countColor++;
										temp = new int[] { element [0], element [1] };
										pattern.Add (temp);
								}
								if (currentColor == MapComponent.Color.Empty) {
										if (bitsMap.getColor (pattern [pattern.Count - 1] [0], pattern [pattern.Count - 1] [1]) == MapComponent.Color.Empty || 
												(countColor != 3 && alreadyEmpty == true)) {
												break;
										}
										temp = new int[] { element [0], element [1] };
										pattern.Add (temp);
										alreadyEmpty = true;
										countEmpty++;
								}

								currentX += ORIENTATION [orientation] [0];
								currentY += ORIENTATION [orientation] [1];
						}

						if (countEmpty <= 3 && countColor == 3 && pattern.Count == 5 || pattern.Count == 6) {
								threeFree.Add (pattern);
						}

						temp = new int[] { element [0], element [1] };
						pattern.Add (temp);
						alreadyEmpty = false;
						countColor = 0;
						countEmpty = 1;
						currentX = element [0] - ORIENTATION [orientation] [0];
						currentY = element [1] - ORIENTATION [orientation] [1];
						while (currentX >= 0 && currentY >= 0 &&
						       currentX < SIZE_MAP && currentY < SIZE_MAP
						       && bitsMap.getColor (currentX, currentY) != otherColor &&
						       countColor < 4) {
							
								MapComponent.Color currentColor = bitsMap.getColor (currentX, currentY);
							
								if (currentColor == color) {
										countColor++;
										temp = new int[] { element [0], element [1] };
										pattern.Add (temp);
								}
								if (currentColor == MapComponent.Color.Empty) {
										if (bitsMap.getColor (pattern [pattern.Count - 1] [0], pattern [pattern.Count - 1] [1]) == MapComponent.Color.Empty || 
												(countColor != 3 && alreadyEmpty == true)) {
												break;
										}
										temp = new int[] { element [0], element [1] };
										pattern.Add (temp);
										alreadyEmpty = true;
										countEmpty++;
								}
							
								currentX -= ORIENTATION [orientation] [0];
								currentY -= ORIENTATION [orientation] [1];
						}
						
						if (countEmpty <= 3 && countColor == 3 && pattern.Count == 5 || pattern.Count == 6) {
								threeFree.Add (pattern);
						}
				}

				print ("===================ListthreeFree=================");
				print (threeFree.Count);
				foreach (List<int[]> element in threeFree) {
						print ("threeFree");
						print (element.Count);
						foreach (int [] cell in element) {
								print ("x = " + cell [0] + " y = " + cell [1]);
						}
				}
				return threeFree;
		}

		public bool putPawn (int x, int y, MapComponent.Color color)
		{
				if (!rules.putPawn (map, x, y) || (rules.doubleThree && isDoubleThree (x, y, color)))
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

				}
				map [x * SIZE_MAP + y] = (char)color;
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

				/*for (int i = 0; i < 8; i++) {
					print (bitsMap.isTaking (currentX, currentY, i));
				}*/
		}

		private char IsTaking (int wayX, int wayY, MapComponent.Color color, MapComponent.Color otherColor)
		{
				int x = currentX + wayX;
				int y = currentY + wayY;
				int nbOtherColor = 0;

				while (x >= 0 && y >= 0 &&
		     		  x < SIZE_MAP && y < SIZE_MAP) {
						if (bitsMap.getColor (x, y) != otherColor && nbOtherColor == 2)
								return (char)1;
						if (bitsMap.getColor (x, y) != otherColor)
								return (char)0;
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
						bitsMap.setWeight (x, y, weight, color);
						x += ORIENTATION [orientation] [0];
						y += ORIENTATION [orientation] [1];
				}
			
				x = currentX - ORIENTATION [orientation] [0];
				y = currentY - ORIENTATION [orientation] [1];
				while (x >= 0 && y >= 0 &&
			       x < SIZE_MAP && y < SIZE_MAP && bitsMap.getColor(x, y) == color) {
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
				map [x * SIZE_MAP + y] = (char)MapComponent.Color.Empty;
				Destroy (GameObject.Find ("Pawn_" + (x * SIZE_MAP + y).ToString ()));
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
						public char[] free = new char[8];
			
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

				public void setIsTaking (int x, int y, int orientation, char state)
				{
						this._map [x * SIZE_MAP + y].takePawns [orientation] = state;
				}

				public bool isTaking (int x, int y, int orientation)
				{
						return  (this._map [x * SIZE_MAP + y].takePawns [orientation % 8] == 1);
				}

				public bool isFree (int x, int y, int orientation)
				{
						return  (this._map [x * SIZE_MAP + y].free [orientation % 8] == 1);
				}

				public void setIsFree (int x, int y, int orientation, char state)
				{
						this._map [x * SIZE_MAP + y].free [orientation % 8] = state;
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
