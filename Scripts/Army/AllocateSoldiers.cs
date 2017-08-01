using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllocateSoldiers : MonoBehaviour {

	public List <int> soldierBank = new List<int>();
	//40 armies for two players
	//35 armies each if three players
	//30 armies each if four players 
	//25 armies each if five players
	//20 armies each if six players

	TroopCount troopCount;
	AddSoldier addSoldier;
	CountryManagement countryManagement;
	PlayerTurn playerTurn;
	TeamChecker teamChecker;
	Phases phases;
	OpeningDeployment openingDeployment;
	BoardSetUp boardSetUp;
	GlobalFunctions globalFunctions;
	DisplayEditor displayEditor;
	ButtonColour buttonColour;
	ChangeCatagory changeCategory;

	GameObject territories, GUI;

	public bool openingPhase;

	int startingArmies, playerArmies;
	int currentPlayer, soldiersLeft;
	public int dropCounter;

	// Use this for initialization
	void Awake () {
		territories = GameObject.FindGameObjectWithTag ("Territories");
		troopCount = territories.GetComponent<TroopCount> ();
		boardSetUp = territories.GetComponent<BoardSetUp> ();
		changeCategory = territories.GetComponent<ChangeCatagory> ();

		GUI = GameObject.FindGameObjectWithTag ("GUI");
		openingDeployment = GUI.GetComponent<OpeningDeployment> ();
		displayEditor = GUI.GetComponent<DisplayEditor> ();
		buttonColour = GUI.GetComponent<ButtonColour> ();

		countryManagement = this.GetComponent<CountryManagement> ();
		playerTurn = this.GetComponent<PlayerTurn> ();
		teamChecker = this.GetComponent<TeamChecker> ();
		phases = this.GetComponent<Phases> ();
		globalFunctions = this.GetComponent<GlobalFunctions> ();
	}

	// Build a soldier bank holding the number of soldiers each player has left to deploy - BoardSetUp
	public void BuildSoldierBank(int numberOfPlayers){
		// the number of starting armies each player receives - should be 50 - ....
		startingArmies = 50 - (5 * numberOfPlayers);
		// build list of giving the number of starting armies each player gets after land has been allocated
		for (int i = 1; i <= numberOfPlayers; i++) {
			playerArmies = startingArmies - troopCount.troopCounter ["Player" + i];
			soldierBank.Add (playerArmies);
		}
		// initialise dropCounter value
		dropCounter = 1;
	}

	// places a single soldier on mouse click and changes player turn - opening phase only
	public void DropSoldier(GameObject country){
		currentPlayer = playerTurn.CurrentPlayer ();
		// can only drop soldier on owned territory
		if (teamChecker.UnderControl(country) & dropCounter == playerTurn.turn) {
			// place a soldier on the selected country
			addSoldier = country.GetComponent<AddSoldier> ();
			addSoldier.PlaceSoldier ();
			// update game stats
			UpdateStats (country);
			// ends opening phase if all soliders have been deployed
			FinishOpeningPhase();
			// changes player after time delay
			StartCoroutine(ExecuteAfterTime(globalFunctions.timeDelay));
		}
	}

	// waits a time t then changes turn
	IEnumerator ExecuteAfterTime(float time) {
		yield return new WaitForSeconds(time);
		playerTurn.NextPlayer (true);
		// display ranking system as troop count
		do
			changeCategory.RotateCategory ();
		while(phases.setupPhase & changeCategory.categoryButton.text != "Troop Count");
	}

	// ends opening phase and changes player turn to the last player - Execute after time (function above) then sets turn to player 1
	void FinishOpeningPhase(){
		// sums the remaining number of soldiers
		if (soldierBank [currentPlayer - 1] == 0) {
			soldiersLeft = 0;
			foreach (int bank in soldierBank)
				soldiersLeft += bank;
			// if all soldiers have been deployed
			if (soldiersLeft == 0) {
				phases.EndOpeningPhase ();
				// sets it to last players turn
				while (!playerTurn.turnOrder [boardSetUp.numberOfPlayers - 1])
					playerTurn.NextPlayer (false);
				// activate category button
				buttonColour.UnlockButton ("category");
			}
		}
	}

	// update various stats
	void UpdateStats(GameObject country){
		// update deployment table
		soldierBank[currentPlayer-1] -= 1;
		openingDeployment.UpdateDeploymentTable (currentPlayer, soldierBank [currentPlayer - 1]);
		// update CountryManagement dictionary
		countryManagement.ChangeArmySize (country,1);
		// update display
		displayEditor.OpeningPlaceSoldier (country);
		// prevents player placing multiple units
		dropCounter = playerTurn.turn + 1;
	}

}
