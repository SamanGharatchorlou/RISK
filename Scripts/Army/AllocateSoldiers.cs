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

	GameStats gameStats;
	TroopCount troopCount;
	AddSoldier addSoldier;
	CountryManagement countryManagement;
	PlayerTurn playerTurn;
	TeamChecker teamChecker;
	Phases phases;
	DeploySoldiers deploySoldiers;
	ChangeCatagory changeCategory;
	OpeningDeployment openingDeployment;
	DisplayTurn displayTurn;
	GameInstructions gameInstructions;

	GameObject territories, GUI;

	public bool openingPhase;

	int startingArmies, playerArmies;
	int currentPlayer;

	// Use this for initialization
	void Awake () {
		territories = GameObject.FindGameObjectWithTag ("Territories");
		gameStats = territories.GetComponent<GameStats> ();
		troopCount = territories.GetComponent<TroopCount> ();
		changeCategory = territories.GetComponent<ChangeCatagory> ();

		GUI = GameObject.FindGameObjectWithTag ("GUI");
		openingDeployment = GUI.GetComponent<OpeningDeployment> ();
		displayTurn = GUI.GetComponent<DisplayTurn> ();
		gameInstructions = GUI.GetComponent<GameInstructions> ();

		countryManagement = this.GetComponent<CountryManagement> ();
		playerTurn = this.GetComponent<PlayerTurn> ();
		teamChecker = this.GetComponent<TeamChecker> ();
		phases = this.GetComponent<Phases> ();
		deploySoldiers = this.GetComponent<DeploySoldiers> ();
	}

	// Build a soldier bank holding the number of soldiers each player has left to deploy - BoardSetUp
	public void BuildSoldierBank(){
		// the number of starting armies each player receives - should be 50 - ....
		startingArmies = 30 - (5 * gameStats.numberOfPlayers);
		// build list of giving the number of starting armies each player gets after land has been allocated
		for (int i = 1; i <= gameStats.numberOfPlayers; i++) {
			playerArmies = startingArmies - troopCount.troopCounter ["Player" + i];
			soldierBank.Add (playerArmies);
		}
	}

	// places a single soldier on mouse click and changes player turn - setup phase only
	public void DropSoldier(GameObject country){
		currentPlayer = playerTurn.CurrentPlayer ();
		// can only drop soldier on owned territory
		if (teamChecker.GetPlayer (country) == currentPlayer) {
			// place a soldier on the selected country
			addSoldier = country.GetComponent<AddSoldier> ();
			addSoldier.PlaceSoldier ();
			// update game stats
			UpdateTroopNumbers (country);
			openingDeployment.UpdateDeploymentTable (currentPlayer, soldierBank [currentPlayer - 1]);
			// last player places their last troop - end opening phase
			if (currentPlayer == gameStats.numberOfPlayers & soldierBank [currentPlayer - 1] == 0) {
				EndOpeningPhase ();
			}
			// change player
			playerTurn.NextPlayer ();
		}
	}

	// update various stats
	void UpdateTroopNumbers(GameObject country){
		// update soldierBank
		soldierBank[playerTurn.CurrentPlayer()-1] -= 1;
		// update CountryManagement dictionary
		countryManagement.ChangeArmySize (country,1);
	}

	void EndOpeningPhase(){
		// start setup phase
		phases.openingPhase = false;
		phases.setupPhase = true;

		// reset rank table at end of phase
		openingDeployment.ResetRankTable(playerTurn.CurrentPlayer());
		// set up bonus soldier text for setup phase
		deploySoldiers.BonusStore();
		// build the 3 category headers
		changeCategory.BuildCatHeaders();
		// display ranking system
		changeCategory.RotateCatagory ();
		// display player 1 turn
		displayTurn.UpdateTurnText(1);
		// update game instructions
		gameInstructions.PlaceTroops();
	}


}
