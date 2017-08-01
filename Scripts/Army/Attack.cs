using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// country with tag "AttackingCountry" battles country with tag "DefendingCountry"
public class Attack : MonoBehaviour {

	public AudioSource battleSound;
	public AudioSource click;

	TargetCountry targetCountry;
	ArmyManagement armyManagement;
	CountryManagement countryManagement;
	DiceRoll diceRoll;
	TargetingNetwork targetingNetwork;
	TakeControl takeControl;
	Phases phases;
	TeamChecker teamChecker;
	GameInstructions gameInstructions;
	DisplayEditor displayEditor;
	AudioFadeOut audioFadeOut;

	public GameObject attackingCountry, defendingCountry;
	GameObject GUI;

	public bool canAttack, UpdateTroopCnt;
	bool Neighbours;

	public int attackingPlayer, defendingPlayer, attackerArmySize, defenderArmySize;
	int deadAttackers, deadDefenders;

	void Awake(){
		GUI = GameObject.FindGameObjectWithTag ("GUI");
		gameInstructions = GUI.GetComponent<GameInstructions> ();
		displayEditor = GUI.GetComponent<DisplayEditor> ();

		targetingNetwork = this.GetComponent<TargetingNetwork> ();
		armyManagement = this.GetComponent<ArmyManagement>();
		countryManagement = this.GetComponent<CountryManagement> ();
		takeControl = this.GetComponent<TakeControl> ();
		diceRoll = this.GetComponent<DiceRoll>();
		phases = this.GetComponent<Phases> ();
		teamChecker = this.GetComponent<TeamChecker> ();
		targetCountry = this.GetComponent<TargetCountry> ();
		audioFadeOut = this.GetComponent<AudioFadeOut> ();
	}

	// Two countries battle i.e one set of die rolls (max 2 deaths) ---- Battle button ----
	public void ATTACK(){
		// only run code during battle phase
		if (phases.battlePhase == true) {
			click.Play ();
			// these two functions set the attacking and defending country variables
			AttackerArmySize ();
			DefenderArmySize ();
			// only runs code if an attacker & defender has been selected
			if (AttackerArmySize () != -1 & DefenderArmySize () != -1) {
				// Checks if the countries can battle
				if (AttackerArmySize () > 1 & CanAttack(attackingCountry,defendingCountry)) {
					Battle (AttackerArmySize (), DefenderArmySize ());
					gameInstructions.BattleOutcome (deadAttackers, deadDefenders);
					displayEditor.BattleResult (attackingCountry, defendingCountry, deadAttackers, deadDefenders);
				}
				// if defender has 0 troops attacker claims the land
				if (DefenderArmySize () == 0)
					takeControl.ClaimLand (attackingCountry, defendingCountry);
			}
		}
	}

	// BATTLE! - calculates remaining troops
	void Battle(int attackers, int defenders){
		if (!battleSound.isPlaying) {
			//battleSound.Play ();
			battleSound.time = Random.Range(0f,battleSound.clip.length);
			battleSound.Play();
			StartCoroutine (audioFadeOut.FadeOut (battleSound, 3f));
		}
		// attackers must leave 1 man behind
		attackers = attackers-1;
		diceRoll.CalculateBattle(attackers,defenders,out deadAttackers,out deadDefenders);

		armyManagement.RemoveDead ("AttackingCountry", deadAttackers);
		armyManagement.RemoveDead ("DefendingCountry", deadDefenders);

		targetCountry.selectingDefender = false;
	}

	// Checks if the countries are neighbours and not owned by the same player
	public bool CanAttack(GameObject attackingCountry, GameObject defendingCountry){
		// are they neighbours?
		Neighbours = targetingNetwork.isNeighbour (attackingCountry.name, defendingCountry.name);
		// are they enemies?
		attackingPlayer = teamChecker.GetPlayer (attackingCountry);
		defendingPlayer = teamChecker.GetPlayer (defendingCountry);

		if (attackingPlayer != defendingPlayer & Neighbours)
			return true;
		else
			return false;
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



}
