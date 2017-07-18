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

	GameObject territories, GUI;

	public bool openingPhase;
	bool soldiersDeployed;

	int startingArmies, playerArmies;
	int currentPlayer, soldiersLeft;

	// Use this for initialization
	void Awake () {
		territories = GameObject.FindGameObjectWithTag ("Territories");
		troopCount = territories.GetComponent<TroopCount> ();
		boardSetUp = territories.GetComponent<BoardSetUp> ();

		GUI = GameObject.FindGameObjectWithTag ("GUI");
		openingDeployment = GUI.GetComponent<OpeningDeployment> ();

		countryManagement = this.GetComponent<CountryManagement> ();
		playerTurn = this.GetComponent<PlayerTurn> ();
		teamChecker = this.GetComponent<TeamChecker> ();
		phases = this.GetComponent<Phases> ();
	}

	// Build a soldier bank holding the number of soldiers each player has left to deploy - BoardSetUp
	public void BuildSoldierBank(int numberOfPlayers){
		// the number of starting armies each player receives - should be 50 - ....
		startingArmies = 31 - (5 * numberOfPlayers);
		// build list of giving the number of starting armies each player gets after land has been allocated
		for (int i = 1; i <= numberOfPlayers; i++) {
			playerArmies = startingArmies - troopCount.troopCounter ["Player" + i];
			soldierBank.Add (playerArmies);
		}
	}

	// places a single soldier on mouse click and changes player turn - opening phase only
	public void DropSoldier(GameObject country){
		currentPlayer = playerTurn.CurrentPlayer ();
		// can only drop soldier on owned territory
		if (teamChecker.UnderControl(country)) {
			// place a soldier on the selected country
			addSoldier = country.GetComponent<AddSoldier> ();
			addSoldier.PlaceSoldier ();
			// update game stats
			UpdateTroopNumbers (country);
			openingDeployment.UpdateDeploymentTable (currentPlayer, soldierBank [currentPlayer - 1]);

			// ends opening phase if all soliders have been deployed
			ShouldOpeningEnd();
			playerTurn.NextPlayer ();
		}
	}

	void ShouldOpeningEnd(){
		if (soldierBank [currentPlayer - 1] == 0) {
			soldiersLeft = 0;
			foreach (int bank in soldierBank)
				soldiersLeft += bank;
			// if all soldiers have been deployed
			if (soldiersLeft == 0) {
				phases.EndOpeningPhase ();
				openingDeployment.ResetColour ();
				// player 1 starts
				while (!playerTurn.turnOrder [boardSetUp.numberOfPlayers - 1])
					playerTurn.NextPlayer ();
			}
		}
	}

	// update various stats
	void UpdateTroopNumbers(GameObject country){
		// update soldierBank
		soldierBank[playerTurn.CurrentPlayer()-1] -= 1;
		// update CountryManagement dictionary
		countryManagement.ChangeArmySize (country,1);
	}

}
