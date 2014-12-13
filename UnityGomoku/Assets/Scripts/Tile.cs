using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
		public GameObject PawnPrefab;
		public Vector2 gridPosition = Vector2.zero;
		private GameManager manager;	

		// Use this for initialization
		void Start ()
		{
				manager = GameObject.Find ("Arbiter").GetComponent<GameManager> ();
		transform.renderer.material.color = Color.grey;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void setGridPosition (Vector2 pos)
		{
				gridPosition = pos;

		string type = "C";

		if ((int)pos.x == 0) {

			if ((int) pos.y == 0) {
				type = "SE";
			} else if ((int) pos.y == MapComponent.SIZE_MAP - 1) {
				type = "NE";
			} else
				type = "E";
		}

		if ((int)pos.x == MapComponent.SIZE_MAP - 1) {
			
			if ((int) pos.y == 0) {
				type = "SO";
			} else if ((int) pos.y == MapComponent.SIZE_MAP - 1) {
				type = "NO";
			} else
				type = "O";
		}

		if ((int)pos.x != 0 && (int)pos.x != MapComponent.SIZE_MAP - 1) {
			
			if ((int) pos.y == 0) {
				type = "S";
			} else if ((int) pos.y == MapComponent.SIZE_MAP - 1)
				type = "N";
		}



				Sprite sprite = Resources.Load<Sprite> ("Sprites/board" + type);
				transform.renderer.material.SetTexture ("_MainTex", sprite.texture);
		}

		void OnMouseEnter ()
		{
				transform.renderer.material.color = Color.blue;
		}

		void OnMouseExit ()
		{
				transform.renderer.material.color = Color.grey;
		}

		void OnMouseDown ()
		{
			if (manager.currentPlayer ().Ia == null) {
						PutPawn ();
				}
		}

		public bool PutPawn()
		{
			manager.currentPlayer ().selectedX = (int)gridPosition.x;
			manager.currentPlayer ().selectedY = (int)gridPosition.y;
			if (manager.currentPlayer ().PutPawn () == false) {
						if (manager.currentPlayer ().Ia == null) {
								transform.renderer.material.color = Color.red;
						}
				}
			else {
			Pawn pawn = ((GameObject)Instantiate (PawnPrefab, 
			                                     new Vector3 (gridPosition.x - Mathf.Floor (MapComponent.SIZE_MAP / 2), 0.7f, -gridPosition.y + Mathf.Floor (MapComponent.SIZE_MAP / 2)),
			                                     Quaternion.Euler (new Vector3 ()))).GetComponent<Pawn> ();
				pawn.name = "Pawn_" + (gridPosition.x * MapComponent.SIZE_MAP + gridPosition.y).ToString();
				if (manager.currentPlayer ().color == Gomoku.Color.Black)
							pawn.transform.renderer.material.color = Color.black;
				manager.currentPlayer ().played = true;
				return true;
			}
			return false;
		}

		private void removePawn()
		{
			Destroy (GameObject.Find ("Pawn_" + (gridPosition.x * MapComponent.SIZE_MAP + gridPosition.y).ToString()));
		}
}
