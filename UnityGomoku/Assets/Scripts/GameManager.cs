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
			if (map.getBitsMap().isTakeable(lastX, lastY, entry.Key) && map.getBitsMap().getColor(lastX + 3 * entry.Value[0], lastY + 3 * entry.Value[1]) == map.getBitsMap().getColor(lastX, lastY)) {
					rules.scoring((PlayerComponent.Color) map.getBitsMap().getColor(lastX, lastY), 2);
					map.getBitsMap().setIsTaken(lastX, lastY, entry.Key, (char) 0);
					map.getBitsMap().setIsTaken(lastX + 3 * entry.Value[0], lastY + 3 * entry.Value[1], entry.Key, (char) 0);
					map.removePawn(lastX + entry.Value[0], lastY + entry.Value[1]);
					map.removePawn(lastX + 2 * entry.Value[0], lastY + 2 * entry.Value[1]);
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
				if (winner == PlayerComponent.Color.White)
						print ("Victoire Blanc");
				else if (winner == PlayerComponent.Color.Black)
						print ("Victoire Noir");
				else
						print ("Match Nul");
				
		}
}
