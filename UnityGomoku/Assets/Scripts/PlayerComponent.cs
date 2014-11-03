using UnityEngine;
using System.Collections;

public class PlayerComponent : MonoBehaviour
{

		public enum Color
		{
				Empty,
				White,
				Black
		}

		private MapComponent map;
		public int selectedX;
		public int selectedY;
		public Color color;
		//Defini si c'est sont tour de jouer
		public bool active = false;



		// Use this for initialization
		void Start ()
		{
				map = (MapComponent)GameObject.Find ("Map").GetComponent<MapComponent>();
		}
	
		// Update is called once per frame
		void Update ()
		{
		}



		/*******
		 * Player Movements
		 * *****/

		public void selectPosition (int x, int y)
		{
				selectedX = x;
				selectedY = y;
		}

		public void moveUp ()
		{
				if (selectedX == 0)
						selectedX = 18;
				else
						selectedX--;
		}

		public void moveDown ()
		{
				if (selectedX == 18)
						selectedX = 0;
				else
						selectedX++;
		}

		public void moveRight ()
		{
				if (selectedY == 18)
						selectedY = 0;
				else
						selectedY++;
		}

		public void moveLeft ()
		{
				if (selectedY == 0)
						selectedY = 18;
				else
						selectedY--;
		}

		public bool putPawn ()
		{
				return map.putPawn (selectedX, selectedY, (MapComponent.Color)color);
		}
}
