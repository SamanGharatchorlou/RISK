using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// soldier receives soldiers at the start of thier turn
public class DeploySoldiers : MonoBehaviour {

	SoldierBonus soldierBonus;
	PlayerTurn playerTurn;
	ReceiveBonus receiveBonus;
	Phases phases;
	ButtonColour buttonColour;

	GameObject territories, GUI;

	int bonusSoldierCount; 
	public int soldiersLeft;

	void Awake () {
		territories = GameObject.FindGameObjectWithTag ("Territories");
		soldierBonus = territories.GetComponent<SoldierBonus> ();

		GUI = GameObject.FindGameObjectWithTag ("GUI");
		receiveBonus = GUI.GetComponent<ReceiveBonus> ();
		buttonColour = GUI.GetComponent<ButtonColour> ();

		phases = this.GetComponent<Phases> ();
		playerTurn = this.GetComponent<PlayerTurn> ();
	}
		
	// sets up the bonus soldiers a player is due to receive
	public void BonusStore(){

		// get their due soldier bonus
		bonusSoldierCount = soldierBonus.soldierIncome["Player" + playerTurn.CurrentPlayer ()];

		// tracks the bonus due after +/- button use
		soldiersLeft = bonusSoldierCount;

		// set up bonus soldier text
		if (phases.setupPhase)
			receiveBonus.SoldierBonusDisplay (soldiersLeft);
	}

	// checks if player can add a soldier onto the field from the bonus soldier bank
	public bool CanAddSoldier(){

		if (soldiersLeft > 0) {
			buttonColour.SetupTurnColour (soldiersLeft);
			return true;

		} else
			return false;
	}

	// checks if player can remove a soldier from the field into the bonus soldier bank
	public bool CanRemoveSoldier(){

		if (soldiersLeft < bonusSoldierCount) {
			buttonColour.SetupTurnColour (soldiersLeft);
			return true;

		} else
			return false;
	}



}
