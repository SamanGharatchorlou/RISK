﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// allows two armies to battle
public class Attack : MonoBehaviour {

	TargetCountry targetCountry;
	ArmyManagement armyManagement;
	CountryManagement countryManagement;
	DiceRoll diceRoll;
	AddSoldier addSoldier;
	TargetingNetwork targetingNetwork;
	TakeControl takeControl;
	Phases phases;
	TeamChecker teamChecker;
	GameInstructions gameInstructions;

	public GameObject attackingCountry, defendingCountry;
	GameObject GUI, Territories;

	public bool canAttack;
	bool Neighbours, Enemies;
	public int attackingPlayer, defendingPlayer;
	public int attackerArmySize, defenderArmySize;
	int deadAttackers, deadDefenders;

	public bool UpdateTroopCnt;


	void Awake(){
		GUI = GameObject.FindGameObjectWithTag ("GUI");
		gameInstructions = GUI.GetComponent<GameInstructions> ();

		targetingNetwork = this.GetComponent<TargetingNetwork> ();
		armyManagement = this.GetComponent<ArmyManagement>();
		countryManagement = this.GetComponent<CountryManagement> ();
		takeControl = this.GetComponent<TakeControl> ();
		diceRoll = this.GetComponent<DiceRoll>();
		phases = this.GetComponent<Phases> ();
		teamChecker = this.GetComponent<TeamChecker> ();
	}

	// Two countries battle i.e one set of die rolls (max 2 deaths) ---- Battle button ----
	public void ATTACK(){
		// only run code during battle phase
		if (phases.battlePhase == true) {
			AttackerArmySize ();
			DefenderArmySize ();
			// only runs code if an attacker & defender has been selected
			if (AttackerArmySize () != -1 & DefenderArmySize () != -1) {
				// Checks if the countries are neighbours and not owned by the same player
				Neighbours = false;
				canAttack = false;

				Neighbours = targetingNetwork.isNeighbour (attackingCountry.name, defendingCountry.name);
				attackingPlayer = teamChecker.GetPlayer (attackingCountry);
				defendingPlayer = teamChecker.GetPlayer (defendingCountry);
				if (attackingPlayer != defendingPlayer & Neighbours)
					canAttack = true;

				if (AttackerArmySize () > 1 & canAttack) {
					Battle (AttackerArmySize (), DefenderArmySize ());
					gameInstructions.BattleOutcome (deadAttackers, deadDefenders);
				}

				// if defender has 0 troops attacker claims the land
				if (DefenderArmySize () == 0)
					takeControl.ClaimLand (attackingCountry, defendingCountry);
			}
		}
	}

	// Gets the attackers army size
	public int AttackerArmySize(){
		attackingCountry = GameObject.FindGameObjectWithTag("AttackingCountry");
		// ensures an attacker has been selected
		if (attackingCountry != null) {
			attackerArmySize = countryManagement.GetArmySize (attackingCountry.name);
			return attackerArmySize;
		} else {
			gameInstructions.NoSelection("attacker");
			// returns a non sense value for checking
			return -1;
		}
	}

	// Gets the defenders army size;
	int DefenderArmySize(){
		defendingCountry = GameObject.FindGameObjectWithTag ("DefendingCountry");
		// ensures a defender has been selected
		if (defendingCountry != null) {
			defenderArmySize = countryManagement.GetArmySize (defendingCountry.name);
			return defenderArmySize;
		} else {
			gameInstructions.NoSelection("defender");
			// returns a non sense value for checking
			return -1;
		}
	}

	// BATTLE! - calculates remaining troops
	void Battle(int attackers, int defenders){
		// attackers must leave 1 man behind
		attackers = attackers-1;
		diceRoll.CalculateBattle(attackers,defenders,out deadAttackers,out deadDefenders);

		armyManagement.RemoveDead ("AttackingCountry", deadAttackers);
		armyManagement.RemoveDead ("DefendingCountry", deadDefenders);
	}

}
