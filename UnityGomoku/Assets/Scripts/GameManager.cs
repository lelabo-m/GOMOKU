using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gomoku;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
		public GameObject player1;
		public GameObject player2;
		private int lastX;
		private int lastY;
		private PlayerComponent playerComponent1;
		private PlayerComponent playerComponent2;
		public Rules rules;
		public MapComponent map;
		private Gomoku.Color winner;
		private GameObject end;
	
		// Use this for initialization
		void Start ()
		{
				end = GameObject.Find ("EndGame");
				NGUITools.SetActive (end, false);
				
				rules = GameObject.Find ("Arbiter").GetComponent<Rules> ();
				map = GameObject.Find ("Map").GetComponent<MapComponent> ();
				Image imgW = GameObject.Find("imgW").GetComponent<Image>();
				Image imgB = GameObject.Find("imgB").GetComponent<Image>();
				
				map.GetMap ().id = 1;
				winner = Gomoku.Color.Empty;

				playerComponent1 = player1.GetComponent<PlayerComponent> ();
				playerComponent1.color = Gomoku.Color.White;
				playerComponent1.playing = true;
				imgW.enabled = true;

				playerComponent2 = player2.GetComponent<PlayerComponent> ();
				if (PlayerPrefs.GetInt ("IA") > 0)
					playerComponent2.Ia = new MCTS_IA(SystemInfo.processorCount + 1, 1000);
				playerComponent2.color = Gomoku.Color.Black;
				playerComponent2.playing = false;
				imgB.enabled = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (map != null && currentPlayer().playing && currentPlayer ().played) {
						if ((winner = CheckMap(this.lastX, this.lastY, map.GetMap())) != Gomoku.Color.Empty)
								GameDone ();

						else if (!isEnoughSpace ())
								GameDone ();
					else 
						changeTurn ();
				}
		}

		public Gomoku.Color CheckMap(int x, int y, Gomoku.Map map) 
		{
			Gomoku.Color win = Gomoku.Color.Empty;

			CheckTakePawns (x, y, map);

			if ((win = isScoringWinner (map)) != Gomoku.Color.Empty) {
		//	MonoBehaviour.print("ScoringWinner");
						return win;
				}
			
			if ((win = isWinner (map)) != Gomoku.Color.Empty) {
			//			MonoBehaviour.print("AlignmentWinner");
						return win;
				}
			
			return win;
		}

		public PlayerComponent currentPlayer ()
		{
				if (playerComponent1.playing)
						return playerComponent1;
				return playerComponent2;
		}

		private void CheckTakePawns (int x, int y, Gomoku.Map m)
		{
			foreach (KeyValuePair<Gomoku.Orientation, int[]> entry in MapComponent.ORIENTATION)
			{
				if (rules.CanTakePawns(m, x, y, entry.Key)) {
					rules.TakePawns(m, x, y, entry.Key);
				if(m.id == 1) {
					map.removePawn (x + MapComponent.ORIENTATION [entry.Key] [0], y + MapComponent.ORIENTATION [entry.Key] [1]);
					map.removePawn (x + 2 * MapComponent.ORIENTATION [entry.Key] [0], y + 2 * MapComponent.ORIENTATION [entry.Key] [1]);
				}
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
				Image imgW = GameObject.Find ("imgW").GetComponent<Image> ();
				Image imgB = GameObject.Find("imgB").GetComponent<Image>();
				playerComponent1.playing = !playerComponent1.playing;
				playerComponent2.playing = !playerComponent2.playing;

				playerComponent1.played = false;
				playerComponent2.played = false;

				if (playerComponent1.playing) {
					
					imgW.enabled = true;
					imgB.enabled = false;	
				} 
				else {
					
					imgW.enabled = false;
					imgB.enabled = true;	
				}
		}

		private Gomoku.Color isScoringWinner (Gomoku.Map map)
		{
				return rules.IsScoringWinner (map);
		}

		private Gomoku.Color isWinner (Gomoku.Map map)
		{
				return rules.IsWinner (map, currentPlayer().color);
		}

		private void GameDone ()
		{
				this.playerComponent1.playing = false;
				this.playerComponent2.playing = false;
				Image imgW = GameObject.Find ("imgW").GetComponent<Image> ();
				Image imgB = GameObject.Find("imgB").GetComponent<Image>();
				Image bB = GameObject.Find("backG").GetComponent<Image>();
				imgW.enabled = false;
				imgB.enabled = false;
				bB.enabled = false;
				MapComponent map = GameObject.Find("Map").GetComponent<MapComponent>();
				Text c = GameObject.Find("score white").GetComponent<Text>();
				c.enabled = false;
				Text d = GameObject.Find("score black").GetComponent<Text>();
				d.enabled = false;
				NGUITools.SetActive (end, true);
				if (winner == Gomoku.Color.White) {
						print ("Victoire Blanc");
						PlayerPrefs.SetInt ("Winner", 0);
				} else if (winner == Gomoku.Color.Black) {
						print ("Victoire Noir");
						PlayerPrefs.SetInt ("Winner", 1);
				} else {
						print ("Match Nul");
						PlayerPrefs.SetInt ("Winner", 2);
				}
		}
}
