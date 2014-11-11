using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
		public GameObject player1;
		public GameObject player2;
		private MapComponent.Color lastColor;
		private int lastX;
		private int lastY;
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
						takePawns ();

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

		private void takePawns ()
		{
			foreach (KeyValuePair<int, int[]> entry in MapComponent.ORIENTATION)
			{
				if (rules.canTakePawns(map, lastX, lastY, entry.Key)) {
					rules.takePawns(map, lastX, lastY, entry.Key);
				}
					
			}
		}

		public void setLastPawn(MapComponent.Color color, int x, int y)
		{
				lastColor = color;
				lastX = x;
				lastY = y;
		}

		public PlayerComponent getActivePlayer ()
		{
				if (playerComponent1.playing)
						return playerComponent1;
				return playerComponent2;
		}
		
		private bool isEnoughSpace ()
		{
				MapComponent.BitsMap tab = map.getBitsMap ();

				for (int x = 0; x < MapComponent.SIZE_MAP; ++x) {
				for (int y = 0; y < MapComponent.SIZE_MAP; ++y) {
									if (tab.getColor(x, y) == MapComponent.Color.Empty)
											return true;
					}
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
				if (winner == PlayerComponent.Color.White) {
						print ("Victoire Blanc");
						PlayerPrefs.SetInt ("Winner", 0);
						Application.LoadLevel(3);
				} else if (winner == PlayerComponent.Color.Black) {
						print ("Victoire Noir");
						PlayerPrefs.SetInt ("Winner", 1);
						Application.LoadLevel(3);
				} else {
						print ("Match Nul");
						PlayerPrefs.SetInt ("Winner", 2);
						Application.LoadLevel(3);
				}
				
		}
}
