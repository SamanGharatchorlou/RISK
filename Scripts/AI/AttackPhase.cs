using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPhase : MonoBehaviour {

	public List<string> attackingCountryList;
	string[] neighbours;

	TeamChecker teamChecker;
	CountryManagement countryManagement;
	TargetingNetwork targetingNetwork;
	Attack attack;
	GlobalFunctions globalFunctions;
	Phases phases;
	MovementPhase movementPhase;

	GameObject previousTag, countryToTag;

	int armySize, attackerArmySize, defenderArmySize;
	int attackerIndex, attackDelay;
	string attackingCountry, defendingCountry;


	void Awake () {
		teamChecker = this.GetComponent<TeamChecker> ();
		countryManagement = this.GetComponent<CountryManagement> ();
		targetingNetwork = this.GetComponent<TargetingNetwork> ();
		attack = this.GetComponent<Attack> ();
		globalFunctions = this.GetComponent<GlobalFunctions> ();
		phases = this.GetComponent<Phases> ();
		movementPhase = this.GetComponent<MovementPhase> ();
	}

	// Calls AttackCountry method - to be called by AIController 
	public void AIAttackCountry(){
		// Set variables
		attackingCountry = null;
		defendingCountry = null;
		armySize = 4;  // the lower this value the more aggressive/reckless the AI
		attackDelay = 1;
		attackerIndex = 0;
		// begins the process of attack
		BeginAssault ();
	}

	// select attacker and target an enermy
	void BeginAssault(){
		SelectAttacker ();
		if (attackingCountry != null)
			SelectDefender ();
		// attack enemy target
		if (defendingCountry != null)
			AttackAfterTime ();
	}

	// selects which country to attack with
	void SelectAttacker(){
		// only be called once during attack phase
		if (attackerIndex == 0)
			// list of countries that can attack with 3 or more troops
			attackingCountryList = globalFunctions.ControlledCountryList (armySize);
		// no other attacking possibilities ends assault
		if (attackerIndex == attackingCountryList.Count) {
			attackingCountry = null;
			defendingCountry = null;
			// ---- Begin movement phase ----
			phases.EndPhase ();
			movementPhase.AIMoveSoldiers ();
			return;
		}
		attackingCountry = attackingCountryList [attackerIndex];
	}

	// Selects which country to attack given an attacker has been selected
	void SelectDefender(){
		// list of the attacking country's Neighbours
		neighbours = targetingNetwork.Neighbours (attackingCountry);
		// targets an enemy country and with an army <= itself until death
		for (int i = 0; i < neighbours.Length; i++) {
			defendingCountry = neighbours [i];
			UpdateArmySizes ();
			if (!teamChecker.UnderControlName (defendingCountry) & attackerArmySize >= defenderArmySize)
				break;
			// setting atttacker army to 0 stops assault if there are no countries to attack
			else if (i == neighbours.Length - 1)
				attackerArmySize = 0;
		}
		// tags attacker and defender
		AddTag ("DefendingCountry", defendingCountry);
		AddTag ("AttackingCountry", attackingCountry);
	}

	// attacks repeatedly given certain conditions are met
	void AttackAfterTime(){
		if (attackerArmySize > 1) {
			attack.ATTACK ();
			UpdateArmySizes ();
			// recall function after time
			Invoke ("AttackAfterTime", attackDelay);
		}
		// if country is claimed make new assault from newly claimed land (i.e. defending country becomes attacking country) and reselect defender
		else if (teamChecker.UnderControlName (defendingCountry) & defenderArmySize >= armySize) {
			attackingCountry = defendingCountry;
			SelectDefender ();
			UpdateArmySizes ();
			Invoke ("AttackAfterTime", attackDelay);
		} 
		// attacks again from another attacking point
		else {
			attackerIndex++;
			BeginAssault ();
		}
	}
		
	// updates attackerArmySize & defenderArmySize army sizes for given countries
	void UpdateArmySizes(){
		attackerArmySize = countryManagement.GetArmySize (attackingCountry);
		defenderArmySize = countryManagement.GetArmySize (defendingCountry);
	}

	// adds a tag to the given country
	void AddTag(string tag, string country){
		// checks for previous tags
		previousTag = GameObject.FindGameObjectWithTag (tag);
		if (previousTag != null)
			previousTag.gameObject.tag = "Untagged";
		// add new tag
		countryToTag = GameObject.Find (country);
		countryToTag.gameObject.tag = tag;
	}

}
