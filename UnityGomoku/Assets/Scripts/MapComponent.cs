using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		private Rules rules;
		private BitsMap bitsMap;
		public char[] map;
	
		// Use this for initialization
		void Start ()
		{
				bitsMap = new BitsMap ();
				map = new char[SIZE_MAP];
				arbiter = GameObject.Find ("Arbiter");
				rules = arbiter.GetComponent<Rules>();

				generateGraphicMap ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		private void generateGraphicMap() 
		{
				graphicMap = new List<List<Tile>> ();
				for (int i = 0; i < SIZE_MAP; ++i) {
						List <Tile> row = new List<Tile> ();
						for (int a = 0; a < SIZE_MAP; ++a) {
								Tile tile = ((GameObject)Instantiate (TilePrefab, 
				                                      new Vector3 (i - Mathf.Floor (SIZE_MAP / 2), 0, -a + Mathf.Floor (SIZE_MAP / 2)),
				                                      Quaternion.Euler (new Vector3 ()))).GetComponent<Tile> ();
								tile.gridPosition = new Vector2 (i, a);
								row.Add (tile);
						}
						graphicMap.Add (row);
				}
		}

		public bool putPawn (int x, int y, Color type)
		{
				if (!rules.putPawn (bitsMap, x, y))
						return false;
				bitsMap.putPawn (x, y, type);
				map [x * SIZE_MAP + y] = (char)type;
				return true;
		}

		public bool removePawn (int x, int y)
		{
				bitsMap.removePawn (x, y);
				map [x * SIZE_MAP + y] = (char)Color.Empty;
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
		
				private int[] map = new int[SIZE_MAP];
		
		
				// set Weight of a color 
				public void setWeight (int weight, Color color)
				{
			
				}
		
				public bool putPawn (int x, int y, Color type)
				{
						return true;
				}
		
				public bool removePawn (int x, int y)
				{
						return true;
				}
		
				public int getCell (int x, int y)
				{
						return map [x * SIZE_MAP + y];
				}
		
		}

}
