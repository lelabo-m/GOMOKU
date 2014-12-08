using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;
using Gomoku;

public class MapComponent : MonoBehaviour
{
		public GameObject TilePrefab;
		private List <List <Tile>> graphicMap;
		public const int SIZE_MAP = 19;
		private GameObject arbiter;
		private GameManager gameManager;
		private Rules rules;
		private Gomoku.Map map;
		private int currentX;
		private int currentY;
		public static Dictionary<Gomoku.Orientation , int[]> ORIENTATION ;

	
		// Use this for initialization
		void Start ()
		{
				ORIENTATION = new Dictionary<Gomoku.Orientation, int[]> ()
		{
			{ Orientation.EAST , new int[] { 1 , 0 } },
			{ Orientation.WEST , new int[] { -1 , 0 } },
			{ Orientation.SOUTH , new int[] { 0 , -1 } },
			{ Orientation.NORTH , new int[] { 0 , 1 } },
			{ Orientation.SOUTHEAST, new int[] { 1 , -1 } },
			{ Orientation.NORTHEAST , new int[] { 1 , 1 } },
			{ Orientation.SOUTHWEST , new int[] { -1 , -1 } },
			{ Orientation.NORTHWEST , new int[] { -1 , 1 } }
		};
				if (PlayerPrefs.GetInt ("5 cassables") > 1) {
						print ("Regle des 5 cassables active !");
				}
				if (PlayerPrefs.GetInt ("double 3") > 1) {
						print ("Regle des double 3 active !");
				}
				this.map = new Gomoku.Map (SIZE_MAP);
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
		
		public bool PutPawn (int x, int y, Gomoku.Color color)
		{
				if (!rules.PutPawn (map, x, y) || (rules.DoubleThree && rules.IsDoubleThree (map, x, y, color)))
						return false;
				map.PutPawn (x, y, color);

				rules.UpdateMap (map, x, y);

				map.GeneratePossibleCells(x, y, 2);
				
				gameManager.SetLastPawn (x, y);
				return true;
		}

		
		public bool removePawn (int x, int y)
		{
				Gomoku.Color color = map.GetColor (x, y);
				map.RemovePawn (x, y);
				map.UpdateBigWeight (color);
				Destroy (GameObject.Find ("Pawn_" + (x * SIZE_MAP + y).ToString ()));
				return true;
		}
	
		public Gomoku.Map GetMap ()
		{
				return this.map;
		}

		public static Gomoku.Orientation? FindOrientation (int wayX, int wayY)
		{
				foreach (KeyValuePair<Gomoku.Orientation, int[]> entry in MapComponent.ORIENTATION) {
						if (entry.Value [0] == wayX && entry.Value [1] == wayY)
								return entry.Key;
				}
				return null;
		}
}
