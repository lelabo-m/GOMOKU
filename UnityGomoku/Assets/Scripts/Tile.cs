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

		public void setGridPosition (Vector2 pos, Sprite sprite)
		{
				gridPosition = pos;

				/*		var croppedTexture = new Texture2D ((int)sprite.rect.width, (int)sprite.rect.height);
				var pixels = sprite.texture.GetPixels ((int)sprite.textureRect.x, 
		                                      (int)sprite.textureRect.y, 
		                                      (int)sprite.textureRect.width, 
		                                      (int)sprite.textureRect.height);
		
				croppedTexture.SetPixels (pixels);
				croppedTexture.Apply ();*/
				//transform.renderer.material.SetTexture ("_MainTex", sprite.texture);
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
				manager.currentPlayer ().selectedX = (int)gridPosition.x;
				manager.currentPlayer ().selectedY = (int)gridPosition.y;
		Debug.Log (manager.currentPlayer ().selectedX);
		Debug.Log (manager.currentPlayer ().selectedY);
				if (manager.currentPlayer ().putPawn () == false)
						transform.renderer.material.color = Color.red;
				else {
						putPawn ();
						manager.currentPlayer ().played = true;
				}

		}

		private void putPawn()
		{
			Pawn pawn = ((GameObject)Instantiate (PawnPrefab, 
			                                     new Vector3 (gridPosition.x - Mathf.Floor (MapComponent.SIZE_MAP / 2), 1, -gridPosition.y + Mathf.Floor (MapComponent.SIZE_MAP / 2)),
			                                     Quaternion.Euler (new Vector3 ()))).GetComponent<Pawn> ();
				if (manager.currentPlayer ().color == PlayerComponent.Color.Black)
							pawn.transform.renderer.material.color = Color.black;
		}

		private void removePawn()
		{
		}
}
