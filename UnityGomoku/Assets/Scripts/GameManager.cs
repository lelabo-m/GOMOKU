using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

		public GameObject player1;
		public GameObject player2;
		private PlayerComponent playerComponent1;
		private PlayerComponent playerComponent2;
		private Rules rules;
		private MapComponent map;
		private PlayerComponent.Color winner;
	
		// Use this for initialization
		void Start ()
		{
				rules = (Rules)GameObject.Find ("Arbiter").GetComponent<Rules> ();
				map = (MapComponent)GameObject.Find ("Map").GetComponent<MapComponent> ();
				winner = PlayerComponent.Color.Empty;

				playerComponent1 = (PlayerComponent)player1.GetComponent<PlayerComponent> ();
				playerComponent1.color = PlayerComponent.Color.White;
				playerComponent1.active = true;

				playerComponent2 = (PlayerComponent)player2.GetComponent<PlayerComponent> ();
				playerComponent2.color = PlayerComponent.Color.Black;
				playerComponent2.active = false;

		}
	
		// Update is called once per frame
		void Update ()
		{
				if ((winner = isScoringWinner ()) != PlayerComponent.Color.Empty)
						gameDone ();

				/*	if ((winner = isWinner ()) != PlayerComponent.Color.Empty)
					gameDone ();*/

			
				changeTurn ();
		}

		private void changeTurn ()
		{
				playerComponent1.active = !playerComponent1.active;
				playerComponent2.active = !playerComponent2.active;
		}

		private PlayerComponent.Color isScoringWinner ()
		{
				if (rules.scores [PlayerComponent.Color.White] == 10)
						return PlayerComponent.Color.White;
				if (rules.scores [PlayerComponent.Color.Black] == 10)
						return PlayerComponent.Color.Black;
				return PlayerComponent.Color.Empty;
		}

		private PlayerComponent.Color isWinner ()
		{
				return PlayerComponent.Color.White;
		}

		private void gameDone ()
		{

		}
}
