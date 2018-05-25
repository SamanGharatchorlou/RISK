using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeControl : MonoBehaviour {

	public AudioSource victoryCry;

	AddSoldier addSoldier;
	CountryManagement countryManagement;
	ArmyManagement armyManagement;
	TerritoryCount territoryCount;
	ContinentBonus continentBonus;
	SoldierTransfer soldierTransfer;
	ButtonColour buttonColour;
	GameInstructions gameInstructions;

	GameObject territories, GUI;

	public bool controlTaken;

	void Awake () {
		countryManagement = this.GetComponent<CountryManagement> ();
		armyManagement = this.GetComponent<ArmyManagement> ();
		soldierTransfer = this.GetComponent<SoldierTransfer> ();

		territories = GameObject.FindGameObjectWithTag ("Territories");
		territoryCount = territories.GetComponent<TerritoryCount> ();
		continentBonus = territories.GetComponent<ContinentBonus> ();

		GUI = GameObject.FindGameObjectWithTag ("GUI");
		buttonColour = GUI.GetComponent<ButtonColour> ();
		gameInstructions = GUI.GetComponent<GameInstructions> ();
	}

	void Start(){
		controlTaken = false;
	}

	// claims the land once called - removes defender and places attacker
	// NOTE: do not change order of commands below
	public void ClaimLand(GameObject attackingCountry, GameObject defendingCountry){
		victoryCry.Play ();
		controlTaken = true;
		defendingCountry.gameObject.tag = "SelectedCountry";
		addSoldier = defendingCountry.GetComponent<AddSoldier> ();

		// Adds a new soldier to defending country (attacker colour)
		addSoldier.PlaceSoldier ();

		// causes a troop to be added to defender and removed from attacker in UpdateTroopCounter;
		countryManagement.ChangeArmySize (defendingCountry, 1);

		// Removes a soldier from attacker (moved to defender land - now claimed)
		armyManagement.RemoveDead ("AttackingCountry", 1);

		// default transfers all attackers over to claimed land
		soldierTransfer.DefaultTransfer(attackingCountry,defendingCountry);

		// update the number of territories dictionary
		territoryCount.UpdateTerritoryBank (attackingCountry, defendingCountry);

		// update continent bonus
		continentBonus.UpdateContBonus ();

		// set button colours
		buttonColour.BattleBattleColour(attackingCountry,defendingCountry);
		buttonColour.BattleAttackColour (false);
		buttonColour.BattlePlusMinusColour (false);

		// update instructions
		gameInstructions.BattleClaim(defendingCountry);
	}

}
