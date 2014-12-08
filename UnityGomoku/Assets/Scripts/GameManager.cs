using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gomoku;


public class GameManager : MonoBehaviour
{
		public GameObject player1;
		public GameObject player2;
		private int lastX;
		private int lastY;
		private PlayerComponent playerComponent1;
		private PlayerComponent playerComponent2;
		private Rules rules;
		public MapComponent map;
		private Gomoku.Color winner;
	
		// Use this for initialization
		void Start ()
		{
				rules = GameObject.Find ("Arbiter").GetComponent<Rules> ();
				map = GameObject.Find ("Map").GetComponent<MapComponent> ();
				winner = Gomoku.Color.Empty;

				playerComponent1 = player1.GetComponent<PlayerComponent> ();
				playerComponent1.color = Gomoku.Color.White;
				playerComponent1.playing = true;

				playerComponent2 = player2.GetComponent<PlayerComponent> ();
				playerComponent2.color = Gomoku.Color.Black;
				playerComponent2.playing = false;

		}
	
		// Update is called once per frame
		void Update ()
		{
				if (currentPlayer ().played) {
						TakePawns ();

						if ((winner = isScoringWinner ()) != Gomoku.Color.Empty)
								GameDone ();

						if ((winner = isWinner ()) != Gomoku.Color.Empty)
								GameDone ();

						if (!isEnoughSpace ())
								GameDone ();

						changeTurn ();
				}
		}

		public PlayerComponent currentPlayer ()
		{
				if (playerComponent1.playing)
						return playerComponent1;
				return playerComponent2;
		}

		private void TakePawns ()
		{
			foreach (KeyValuePair<Gomoku.Orientation, int[]> entry in MapComponent.ORIENTATION)
			{
				if (rules.CanTakePawns(map, lastX, lastY, entry.Key)) {
					rules.TakePawns(map, lastX, lastY, entry.Key);
				}
					
			}
		}

		public void SetLastPawn(int x, int y)
		{
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
				Gomoku.Map tab = map.GetMap ();

				for (int x = 0; x < MapComponent.SIZE_MAP; ++x) {
				for (int y = 0; y < MapComponent.SIZE_MAP; ++y) {
									if (tab.GetColor(x, y) == Gomoku.Color.Empty)
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

		private Gomoku.Color isScoringWinner ()
		{
				return rules.IsScoringWinner ();
		}

		private Gomoku.Color isWinner ()
		{
				return rules.IsWinner (map.GetMap(), currentPlayer().color);
		}

		private void GameDone ()
		{
				if (winner == Gomoku.Color.White) {
						print ("Victoire Blanc");
						PlayerPrefs.SetInt ("Winner", 0);
						Application.LoadLevel(3);
				} else if (winner == Gomoku.Color.Black) {
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
