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

		//Defini si c'est sont tour de jouer
		public bool playing = false;

		public GameManager gm; 
		


		// Use this for initialization
		void Start ()
		{
				gm = GameObject.Find ("Arbiter").GetComponent<GameManager>();
				map = GameObject.Find ("Map").GetComponent<MapComponent>();
		}

		// Update is called once per frame
		void Update ()
		{
			if (Ia != null && playing && isplaying == false) {
                isplaying = true;
				Ia.Play(gm);	
			}
		}
	
		public bool PutPawn ()
		{
				return map.PutPawn (selectedX, selectedY, (Gomoku.Color) color);
		}

}
