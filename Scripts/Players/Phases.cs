using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// activates the different phases of the game
public class Phases : MonoBehaviour {

	GameObject[] deployedSoldiers;

	DeploySoldiers deploySoldiers;
	PlayerTurn playerTurn;
	ReceiveBonus receiveBonus;
	DisplayTurn displayTurn;
	GameInstructions gameInstructions;
	public PhaseButton phaseButton;
	ButtonColour buttonColour;
	ArmyMovement armyMovement;
	TerritoryCount territoryCount;
	TerritoryRank territoryRank;
	OpeningDeployment openingDeployment;
	ChangeCatagory changeCategory;

	GameObject GUI, territories;

	public bool startingPhase, openingPhase, setupPhase, battlePhase, movementPhase;

	void Awake(){
		GUI = GameObject.FindGameObjectWithTag ("GUI");
		receiveBonus = GUI.GetComponent<ReceiveBonus> ();
		displayTurn = GUI.GetComponent<DisplayTurn> ();
		gameInstructions = GUI.GetComponent<GameInstructions> ();
		buttonColour = GUI.GetComponent<ButtonColour> ();
		openingDeployment = GUI.GetComponent<OpeningDeployment> ();

		territories = GameObject.FindGameObjectWithTag ("Territories");
		territoryCount = territories.GetComponent<TerritoryCount> ();
		territoryRank = territories.GetComponent<TerritoryRank> ();
		changeCategory = territories.GetComponent<ChangeCatagory> ();

		playerTurn = this.GetComponent<PlayerTurn> ();
		deploySoldiers = this.GetComponent<DeploySoldiers> ();
		armyMovement = this.GetComponent<ArmyMovement> ();
	}

	void Start () {
		startingPhase = true;
		openingPhase = false;
		setupPhase = false;
		battlePhase = false;
		movementPhase = false;
		phaseButton.EndSetupText ();
	}

	public void EndOpeningPhase(){
		// start setup phase
		openingPhase = false;
		setupPhase = true;
		// set up bonus soldier text for setup phase
		deploySoldiers.BonusStore();
		// update game instructions
		gameInstructions.PlaceTroops();
		//resets row colours of rank table
		openingDeployment.ResetColour();
		// reset player turn
		playerTurn.turn = 0;
	}

	
	void EndSetupPhase(){
		// only runs if all soldiers have been deployed
		if (setupPhase & deploySoldiers.soldiersLeft == 0) {
			// set bools for in game checking
			setupPhase = false;
			battlePhase = true;
			// change phase button text
			phaseButton.EndBattleText ();
			// remove unneeded display
			receiveBonus.RemoveSoliderDisplayer ();
			// adjust game instructions
			gameInstructions.SelectAtkCountry ();
			// resets button colours
			buttonColour.DeactiveateAll();
			buttonColour.BattleAttackColour (false);
			// remove all "DeployedSoldier" tags
			deployedSoldiers = GameObject.FindGameObjectsWithTag("DeployedSoldier");
			foreach (GameObject soldier in deployedSoldiers)
				soldier.tag = "Untagged";
			/*
			// testing mode
			if(!openingPhase)
				attackPhase.AIAttackCountry ();
			*/
		}
	}

	void EndBattlePhase(){
		if (battlePhase) {
			// set bools for in game checking
			battlePhase = false;
			movementPhase = true;
			// change phase button text
			phaseButton.EndTurnText ();
			// adjust game instructions
			gameInstructions.SelectMoveButton ();
			// resets button colours
			buttonColour.DeactiveateAll();
			// activeate move button colour
			buttonColour.MovementMoveColour ("active");

			/*
			// testing mode
			if(!openingPhase)
				AImovementPhase.AIMoveSoldiers ();
			*/
		}
	}

	void EndMovementPhase(){
		if (movementPhase) {
			// set bools for in game checking
			movementPhase = false;
			setupPhase = true;
			//end player turn
			playerTurn.NextPlayer (true);
			// skips defeated players turn
			if (territoryCount.landCounter ["Player" + playerTurn.CurrentPlayer ()] == 0) {
				// ends game if required
				DoesGameEnd ();
				setupPhase = false;
				movementPhase = true;
				EndMovementPhase ();
				return;
			}
			// change phase button text
			phaseButton.EndSetupText ();
			// displays soldier bonus display
			receiveBonus.SoldierBonusDisplay (deploySoldiers.soldiersLeft);
			// resets button colours
			buttonColour.DeactiveateAll();
			// reset movement phase variables
			armyMovement.ResetMoveVariables();
		}
	}

	// moves onto the next phase
	public void EndPhase(){
		if (setupPhase == true)
			EndSetupPhase ();
		else if (battlePhase == true)
			EndBattlePhase ();
		else
			EndMovementPhase ();
	}

	// ends game if only 1 more player is left
	void DoesGameEnd(){
		// gets the number of the player ranked 1st and 2nd by territory count
		string playerRank1 = "Player" + territoryRank.TerrCountPlayerRanks [0];
		string playerRank2 = "Player" + territoryRank.TerrCountPlayerRanks [1];
		// if this player has no territories game is over
		if (territoryCount.landCounter [playerRank2] == 0) {
			// do something to say game is over
			print("Game Over. The winner is " + playerRank1);
		}
	}

}
