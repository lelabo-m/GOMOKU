using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour
{

		private PlayerComponent player;

		// Use this for initialization
		void Start ()
		{
				player = this.GetComponentInParent<PlayerComponent> ();


		}
	
		// Update is called once per frame
		void Update ()
		{

				if (player.play) {
			//Attendre event souris ou clavier
						/*
		 * Pour placer un pion
		 * 
		 * player.selectPosition(x, y);
		 * player.putPawn() <= false si pas possible
		 */
				}
	
		}



}
