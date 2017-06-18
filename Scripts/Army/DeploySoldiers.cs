using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: FIX - once a soldier has been deployed the player can press - and gain a soldier from another territory 
//			  essentially moving soldiers from one country to another

// soldier receives soldiers at the start of thier turn
public class DeploySoldiers : MonoBehaviour {

	SoldierBonus soldierBonus;
	PlayerTurn playerTurn;
	ReceiveBonus receiveBonus;
	Phases phases;

	GameObject territories, GUI;

	int bonusSoldierCount; 
	public int movingSoldierCount;
	string player;

	void Awake () {
		territories = GameObject.FindGameObjectWithTag ("Territories");
		soldierBonus = territories.GetComponent<SoldierBonus> ();
		GUI = GameObject.FindGameObjectWithTag ("GUI");
		receiveBonus = GUI.GetComponent<ReceiveBonus> ();

		phases = this.GetComponent<Phases> ();
		playerTurn = this.GetComponent<PlayerTurn> ();
	}
		
	// sets up the bonus soldiers a player is due to receive
	public void BonusStore(){
		// current players turn
		player = "Player" + playerTurn.CurrentPlayer ();
		// get their due soldier bonus
		bonusSoldierCount = soldierBonus.soldierIncome[player];
		// tracks the bonus due after +/- button use
		movingSoldierCount = bonusSoldierCount;
		// set up bonus soldier text
		if (phases.setupPhase)
			receiveBonus.SoldierBonusDisplay (movingSoldierCount);
	}

	// checks if player can add a soldier onto the field from the bonus soldier bank
	public bool AddSoldier(){
		if (movingSoldierCount > 0)
			return true;
		else
			return false;
	}

	// checks if player can remove a soldier from the field into the bonus soldier bank
	public bool RemoveSoldier(){
		if (movingSoldierCount < bonusSoldierCount)
			return true;
		else
			return false;
	}



}
