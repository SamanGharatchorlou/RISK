using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// allows player to transfer soldiers after a battle
public class SoldierTransfer : MonoBehaviour {

	ArmyMovement armyMovement;
	CountryManagement countryManagement;
	Attack attack;
	Phases phases;
	ButtonColour buttonColour;

	GameObject GUI;
	public GameObject fromCountry, toCountry;

	public int attackerNumbers;

	void Awake(){
		armyMovement = this.GetComponent<ArmyMovement> ();
		countryManagement = this.GetComponent<CountryManagement> ();
		attack = this.GetComponent<Attack> ();
		phases = this.GetComponent<Phases> ();

		GUI = GameObject.FindGameObjectWithTag ("GUI");
		buttonColour = GUI.GetComponent<ButtonColour> ();
	}

	// default - move all attackers over
	public void DefaultTransfer(GameObject fromCountry, GameObject toCountry){
		// moves all the troops over except from 1
		for (int i = 0; countryManagement.GetArmySize(fromCountry.name) > 1; i++) {
			armyMovement.MoveSoldier (fromCountry, toCountry);
		}
	}

	// moves troops from attacking country to claimed country (----- + button -----)
	public void FwdTransferTroops(){
		// only run code on battle phase
		if (phases.battlePhase) {
			fromCountry = attack.attackingCountry;
			toCountry = attack.defendingCountry;
			// error prevention
			if (fromCountry != null) {
				// must leave 1 man behind
				if (countryManagement.GetArmySize (fromCountry.name) > 1)
					armyMovement.MoveSoldier (fromCountry, toCountry);
				buttonColour.BattlePlusMinusColour2 (fromCountry, toCountry, 0);
			}
		}
	}
		
	// moves troops from claimed country to attacking country (----- - button -----)
	public void BackTransferTroops(){
		// only run code on battle phase
		if (phases.battlePhase) {
			fromCountry = attack.attackingCountry;
			toCountry = attack.defendingCountry;
			// maximum number of attackers is 3
			attackerNumbers = attack.attackerArmySize - 1;
			if (attackerNumbers > 3)
				attackerNumbers = 3;
			// error prevention
			if (fromCountry != null) {
				// if attacker attacked with 3 or more must leave at least those 3 behind
				if (fromCountry != null & toCountry != null & countryManagement.GetArmySize (toCountry.name) > attackerNumbers)
					armyMovement.MoveSoldier (toCountry, fromCountry);
				buttonColour.BattlePlusMinusColour2 (fromCountry, toCountry, attackerNumbers);
			}
		}
	}


}
