using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurn : MonoBehaviour {

	public Text TurnNumberText;
	public List<bool> turnOrder;
	public float[][] playerColourList;

	ContinentBonus continentBonus;
	DeploySoldiers deploySoldiers;
	Phases phases;
	StarterPhase starterPhase;
	SetupPhase setupPhase;

	GameObject territories;

	bool player1, player2, player3, player4, player5;

	public int turn;

	// Set up array of player colours
	void Awake () {
		deploySoldiers = this.GetComponent<DeploySoldiers> ();
		phases = this.GetComponent<Phases> ();
		starterPhase = this.GetComponent<StarterPhase> ();
		setupPhase = this.GetComponent<SetupPhase> ();

		territories = GameObject.FindGameObjectWithTag ("Territories");
		continentBonus = territories.GetComponent<ContinentBonus> ();
	}

	void Start(){
		// sets the turn to 1 after opening phase
		turn = 0;
		// Player 1 starts
		player1 = true;
		player2 = false;
		player3 = false;
		player4 = false;
		player5 = false;
		// Create list of players 1-5 - can have upto 5 players
		turnOrder = new List<bool> ();
		turnOrder.Add (player1);
		turnOrder.Add (player2);
		turnOrder.Add (player3);
		turnOrder.Add (player4);
		turnOrder.Add (player5);
		// multi-dim arrary of colours (r,g,b) a is always 1
		playerColourList = new float[][] {
			new float[] {0,1,0}, //green
			new float[] {0,0,1}, //blue
			new float[] {1,0,0}, //red
			new float[] {1, 0.92f, 0.016f}, //yellow
			new float[] {0,0,0}, //black
			new float[] {0.2f,0.2f,0.2f} // dark grey - default text colour
		};
	}

	// removes the unused players from list (input given by button)
	public void ChangePlayerCount(int numberOfPlayers) {
		turnOrder.RemoveRange (numberOfPlayers, turnOrder.Count - numberOfPlayers);
	}

	// Ends the current players turn
	public void NextPlayer() {
		for (int i = 0; i < turnOrder.Count; i++) {
			if (turnOrder [i] == true) {
				turnOrder [i] = false;
				// changes player (not incl. last player)
				if (i < turnOrder.Count - 1) {
					turnOrder [i + 1] = true;
					break;
				}
				// changes from last player to player 1
				turnOrder [0] = true;
				// updates turn number
				if (!phases.openingPhase)
					turn += 1;
			}
		}
		// calculate bonus from continent bonus
		continentBonus.UpdateContBonus ();
		// give player bonus soldiers
		deploySoldiers.BonusStore ();
		// activate AI players
		if (phases.openingPhase)
			starterPhase.AIDeployTroop ();
		else if (CurrentPlayer () != 1)
			setupPhase.AIPlaceTroops ();


		// active AI player if required
		//aiController.CheckPlayer();

		//test
		TurnNumber();
	}

	public void TurnNumber(){
		TurnNumberText.text = turn.ToString();
	}
		
	// Returns the current player number
	public int CurrentPlayer(){
		int currentPlayerTurn = 0;
		for (int a = 0; a < turnOrder.Count; a++) {
			if (turnOrder [a] == true)
				currentPlayerTurn = a;
		}
		return currentPlayerTurn+1;
	}
	

}
