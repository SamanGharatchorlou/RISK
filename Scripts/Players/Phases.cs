using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// activates the different phases of the game
public class Phases : MonoBehaviour {

	DeploySoldiers deploySoldiers;
	PlayerTurn playerTurn;
	ReceiveBonus receiveBonus;
	DisplayTurn displayTurn;
	GameInstructions gameInstructions;
	public PhaseButton phaseButton;

	GameObject GUI;

	public bool startingPhase, openingPhase, setupPhase, battlePhase, movementPhase;

	void Awake(){
		GUI = GameObject.FindGameObjectWithTag ("GUI");
		receiveBonus = GUI.GetComponent<ReceiveBonus> ();
		displayTurn = GUI.GetComponent<DisplayTurn> ();
		gameInstructions = GUI.GetComponent<GameInstructions> ();
		playerTurn = this.GetComponent<PlayerTurn> ();
		deploySoldiers = this.GetComponent<DeploySoldiers> ();
		startingPhase = true;
	}

	void Start () {
		openingPhase = false;
		setupPhase = false;
		battlePhase = false;
		movementPhase = false;
		phaseButton.EndSetupText ();
	}
	
	void EndSetupPhase(){
		// only runs if all soldiers have been deployed
		if (setupPhase & deploySoldiers.movingSoldierCount == 0) {
			setupPhase = false;
			battlePhase = true;
			phaseButton.EndBattleText ();
			receiveBonus.RemoveSoliderDisplayer ();
			gameInstructions.SelectAtkCountry ();
		}
	}

	void EndBattlePhase(){
		if (battlePhase) {
			battlePhase = false;
			movementPhase = true;
			phaseButton.EndTurnText ();
			gameInstructions.SelectFromCountry ();
		}
	}

	void EndMovementPhase(){
		if (movementPhase) {
			movementPhase = false;
			setupPhase = true;
			phaseButton.EndSetupText ();
			//end player turn
			playerTurn.NextPlayer ();
			// displays soldier bonus display
			receiveBonus.SoldierBonusDisplay (deploySoldiers.movingSoldierCount);

			displayTurn.UpdateTurnText (playerTurn.CurrentPlayer ());
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

}
