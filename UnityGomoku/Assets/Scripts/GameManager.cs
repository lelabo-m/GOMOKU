using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	
		private Rules rules;
		private MapComponent map;
		private PlayerComponent.Color winner;
	
		// Use this for initialization
		void Start ()
		{
				rules = (Rules)GameObject.Find ("Arbiter").GetComponent ("Rules");
				map = (MapComponent)GameObject.Find ("Map").GetComponent ("MapComponent");
				winner = PlayerComponent.Color.Empty;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if ((winner = isScoringWinner ()) != PlayerComponent.Color.Empty)
						gameDone ();

			/*	if ((winner = isWinner ()) != PlayerComponent.Color.Empty)
					gameDone ();*/		
		
		}

		private PlayerComponent.Color isScoringWinner ()
		{
				if (rules.scores [PlayerComponent.Color.White] == 10)
						return PlayerComponent.Color.White;
				if (rules.scores [PlayerComponent.Color.Black] == 10)
						return PlayerComponent.Color.Black;
				return PlayerComponent.Color.Empty;
		}

		private PlayerComponent.Color isWinner() {
			return PlayerComponent.Color.White;
		}

		private void gameDone ()
		{

		}
}
