using UnityEngine;
using System.Collections;
using Gomoku;

public class PlayerComponent : MonoBehaviour
{

		public bool played = false;
		private MapComponent map;
		public int selectedX;
		public int selectedY;
		public Gomoku.Color color;

		//Defini si c'est sont tour de jouer
		public bool playing = false;
		


		// Use this for initialization
		void Start ()
		{
				map = GameObject.Find ("Map").GetComponent<MapComponent>();
		}

		// Update is called once per frame
		void Update ()
		{
		}
	
		public bool PutPawn ()
		{
				return map.PutPawn (selectedX, selectedY, (Gomoku.Color) color);
		}

}
