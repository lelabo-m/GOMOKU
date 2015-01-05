using UnityEngine;
using System.Collections;
using Gomoku;

public class PlayerComponent : MonoBehaviour
{

		public bool played = false;
		public bool isplaying = false;
		private MapComponent map;
		public int selectedX;
		public int selectedY;
		public Gomoku.Color color;
		public MCTS_IA Ia = null;
		public bool playing = false;
		public GameManager gm;
		
		void Start ()
		{
				gm = GameObject.Find ("Arbiter").GetComponent<GameManager> ();
				map = GameObject.Find ("Map").GetComponent<MapComponent> ();
		}
	
		void Update ()
		{
				if (Ia != null && playing && isplaying == false) {
						isplaying = true;
						Ia.Play (gm);	
				}
		}
	
		public bool PutPawn ()
		{
				return map.PutPawn (selectedX, selectedY, (Gomoku.Color)color);
		}

}
