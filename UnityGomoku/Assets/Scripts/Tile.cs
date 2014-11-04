using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{

		public Vector2 gridPosition = Vector2.zero;
		private GameManager manager;	

		// Use this for initialization
		void Start ()
		{
			manager = GameObject.Find ("Arbiter").GetComponent<GameManager> ();
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

		void onMouseEnter ()
		{
			transform.renderer.material.color = Color.blue;
		}

		void onMouseExit ()
		{
			transform.renderer.material.color = Color.white;
		}

		void onMouseDown ()
		{
		Debug.Log ("click");
		manager.currentPlayer ().selectedX = (int) gridPosition.x;
		manager.currentPlayer ().selectedY = (int) gridPosition.y;
		//if (manager.currentPlayer ().putPawn() == false)

		}
}
