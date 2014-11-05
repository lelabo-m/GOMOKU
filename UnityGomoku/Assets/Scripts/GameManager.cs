using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
		public GameObject player1;
		public GameObject player2;
		private MapComponent.Color lastColor;
		private PlayerComponent playerComponent1;
		private PlayerComponent playerComponent2;
		private Rules rules;
		private MapComponent map;
		private PlayerComponent.Color winner;
	
		// Use this for initialization
		void Start ()
		{
				rules = GameObject.Find ("Arbiter").GetComponent<Rules> ();
				map = GameObject.Find ("Map").GetComponent<MapComponent> ();
				winner = PlayerComponent.Color.Empty;

				playerComponent1 = player1.GetComponent<PlayerComponent> ();
				playerComponent1.color = PlayerComponent.Color.White;
				playerComponent1.playing = true;

				playerComponent2 = player2.GetComponent<PlayerComponent> ();
				playerComponent2.color = PlayerComponent.Color.Black;
				playerComponent2.playing = false;

		}
	
		// Update is called once per frame
		void Update ()
		{
				if (currentPlayer ().played) {
						takePawn ();

						if ((winner = isScoringWinner ()) != PlayerComponent.Color.Empty)
								gameDone ();

						if ((winner = isWinner ()) != PlayerComponent.Color.Empty)
								gameDone ();

						if (!isEnoughSpace ())
								gameDone ();

						changeTurn ();
				}
		}

		public PlayerComponent currentPlayer ()
		{
				if (playerComponent1.playing)
						return playerComponent1;
				return playerComponent2;
		}

		private void takePawn ()
		{
		}

		public void setLastColor (MapComponent.Color color)
		{
				lastColor = color;
		}

		public PlayerComponent getActivePlayer ()
		{
				if (playerComponent1.playing)
						return playerComponent1;
				return playerComponent2;
		}
		
		private bool isEnoughSpace ()
		{
				char [] tab = map.getMap ();

				for (int i = 0; i < MapComponent.SIZE_MAP * MapComponent.SIZE_MAP; ++i) {
						if (tab [i] == (char)MapComponent.Color.Empty)
								return true;
				}
				return false;
		}

		private void changeTurn ()
		{
				playerComponent1.playing = !playerComponent1.playing;
				playerComponent2.playing = !playerComponent2.playing;

				playerComponent1.played = false;
				playerComponent2.played = false;
		}

		private PlayerComponent.Color isScoringWinner ()
		{
				return rules.isScoringWinner ();
		}

		private PlayerComponent.Color isWinner ()
		{
				return rules.isWinner (map);
		}

		private void gameDone ()
		{
			print ("test");
		}
}
