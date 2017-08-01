using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// activates the different phases of the game
public class Phases : MonoBehaviour {

	public AudioSource click;
    public GameObject scroll;

	GameObject[] deployedSoldiers;

	public PhaseButton phaseButton;
	DeploySoldiers deploySoldiers;
	PlayerTurn playerTurn;
	ReceiveBonus receiveBonus;
	GameInstructions gameInstructions;
	ButtonColour buttonColour;
	ArmyMovement armyMovement;
	TerritoryCount territoryCount;
	OpeningDeployment openingDeployment;
    EndGame endGame;
    DisplayEditor displayEditor;

	GameObject GUI, territories;

	public bool startingPhase, openingPhase, setupPhase, battlePhase, movementPhase, deadPlayer;

	void Awake(){
		GUI = GameObject.FindGameObjectWithTag ("GUI");
		receiveBonus = GUI.GetComponent<ReceiveBonus> ();
		gameInstructions = GUI.GetComponent<GameInstructions> ();
		buttonColour = GUI.GetComponent<ButtonColour> ();
		openingDeployment = GUI.GetComponent<OpeningDeployment> ();
        endGame = GUI.GetComponent<EndGame>();
        displayEditor = GUI.GetComponent<DisplayEditor>();

		territories = GameObject.FindGameObjectWithTag ("Territories");
		territoryCount = territories.GetComponent<TerritoryCount> ();

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
		buttonColour.LockAllButtons ();
		deadPlayer = false;
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
		ResetButtons ();
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
			// remove all "DeployedSoldier" tags
			deployedSoldiers = GameObject.FindGameObjectsWithTag("DeployedSoldier");
			foreach (GameObject soldier in deployedSoldiers)
				soldier.tag = "Untagged";

			// resets button colours
			ResetButtons();
			buttonColour.BattleStartPhase ();
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
			gameInstructions.SelectFromCountry ();
            // remove defending country display text
            displayEditor.RemoveBattleText();
			// resets button colours
			ResetButtons();
			buttonColour.MovementStartPhase ();
		}
	}

	void EndMovementPhase(){
		if (movementPhase) {
			// set bools for in game checking
			movementPhase = false;
			setupPhase = true;
			// ends movement audio
			armyMovement.movementAudio.Stop ();
			// skips defeated players turn
			if (territoryCount.landCounter ["Player" + playerTurn.FollowingPlayer()] == 0)
				deadPlayer = true;
            // ends game if required
            if (territoryCount.landCounter["Player" + playerTurn.CurrentPlayer()] == 0)
                endGame.DoesGameEnd();
			//end player turn
			playerTurn.NextPlayer (true);
			// allows next player to move troops
			armyMovement.movementComplete = false;
			// displays soldier bonus display
			receiveBonus.SoldierBonusDisplay (deploySoldiers.soldiersLeft);
			// reset movement phase variables
			armyMovement.ResetMoveVariables();
			// reset buttons
			phaseButton.EndSetupText ();
			ResetButtons ();
			buttonColour.MovementDefault ();
		}
	}

	// moves onto the next phase
	public void EndPhase(){
		click.Play ();
		if (setupPhase == true)
			EndSetupPhase ();
		else if (battlePhase == true)
			EndBattlePhase ();
		else
			EndMovementPhase ();
	}

    void ResetButtons() {
        buttonColour.DeactiveateAll();
        buttonColour.LockAllButtons();
    }

}
