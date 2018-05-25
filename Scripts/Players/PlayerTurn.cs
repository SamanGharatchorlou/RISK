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
	LockoutPlayer lockoutPlayer;
	AllocateSoldiers allocateSoldiers;
	DisplayTurn displayTurn;
	ChangeCatagory changeCategory;
	GameInstructions gameInstructions;
	BoardSetUp boardSetUp;

	GameObject territories, GUI;

	bool player1, player2, player3, player4, player5;

	public int turn;

	// Set up array of player colours
	void Awake () {
		deploySoldiers = this.GetComponent<DeploySoldiers> ();
		phases = this.GetComponent<Phases> ();
		starterPhase = this.GetComponent<StarterPhase> ();
		setupPhase = this.GetComponent<SetupPhase> ();
		allocateSoldiers = this.GetComponent<AllocateSoldiers> ();

		territories = GameObject.FindGameObjectWithTag ("Territories");
		continentBonus = territories.GetComponent<ContinentBonus> ();
		changeCategory = territories.GetComponent<ChangeCatagory> ();
		boardSetUp = territories.GetComponent<BoardSetUp> ();

		GUI = GameObject.FindGameObjectWithTag ("GUI");
		lockoutPlayer = GUI.GetComponent<LockoutPlayer> ();
		displayTurn = GUI.GetComponent<DisplayTurn> ();
		gameInstructions = GUI.GetComponent<GameInstructions> ();
	}

	void Start(){

		// sets the turn to 1 after opening phase
		turn = 1;

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
			new float[] {0,1,0},            //green
			new float[] {0,0,1},            //blue
			new float[] {1,0,0},            //red
			new float[] {1, 0.92f, 0.016f}, //yellow
			new float[] {0,0,0},            //black
			new float[] {0.2f,0.2f,0.2f}    // dark grey - default text colour
		};
	}

	// removes the unused players from list (input given by button)
	public void ChangePlayerCount(int numberOfPlayers) {
		turnOrder.RemoveRange (numberOfPlayers, turnOrder.Count - numberOfPlayers);
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

	//NOTE: this function hasnt actually been used anywhere yet.
	// returns the next players number
	public int FollowingPlayer(){

		if (CurrentPlayer () < boardSetUp.numberOfPlayers)
			return CurrentPlayer () + 1;

		else
			return 1;
	}

	// Ends the current players turn
	public void NextPlayer(bool activeAIPlayer) {

		for (int i = 0; i < turnOrder.Count; i++) {

			if (turnOrder [i] == true) {

				turnOrder [i] = false;

				// changes player (not incl. last player)
				if (i < turnOrder.Count - 1) {

					turnOrder [i + 1] = true;

					// breaks loop prevents continual turn changes
					break;
				}

				// changes last player to player 1
				else
					turnOrder [0] = true;

				// updates turn number
				turn++;
			}
		}

		UpdateStats ();

		// activate AI player - some circumstances require the AI code not to run (allocateSoldiers.EndOpeningPhase)
		if (activeAIPlayer) {
			ActivateAI ();
			lockoutPlayer.ButtonLocks ();
		}
	}
		
	// activate AI players if its not player 1
	void ActivateAI(){

		if (phases.openingPhase)
			starterPhase.AIDeployTroop ();
        
		else if (CurrentPlayer () != 1)
			setupPhase.AIPlaceTroops ();
	}
		
	void UpdateStats(){

		// calculate bonus from continent bonus
		continentBonus.UpdateContBonus ();

		// give player bonus soldiers
		deploySoldiers.BonusStore ();

		// displays the players turn, game turn numbers & game instructions
		displayTurn.UpdateTurnText (CurrentPlayer(),turn);
		gameInstructions.PlaceTroops ();

		// prevents player adding multiple players during openingPhase
		if(phases.openingPhase)
			allocateSoldiers.dropCounter = turn;

		// update rank display before player 1's turn - 3 rotations brings it back to original category
		if (!phases.openingPhase & CurrentPlayer () ==  1) {

			for (int i = 0; i < 3; i++)
				changeCategory.RotateCategory ();

		}
	}

}
