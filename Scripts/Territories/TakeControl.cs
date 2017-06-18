using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeControl : MonoBehaviour {

	AddSoldier addSoldier;
	CountryManagement countryManagement;
	ArmyManagement armyManagement;
	TerritoryCount territoryCount;
	ContinentBonus continentBonus;
	SoldierTransfer soldierTransfer;
	TroopCount troopCount;

	GameObject scriptHolder, territories;

	string attackingPlayer, defendingPlayer;

	void Awake () {
		countryManagement = this.GetComponent<CountryManagement> ();
		armyManagement = this.GetComponent<ArmyManagement> ();
		soldierTransfer = this.GetComponent<SoldierTransfer> ();

		territories = GameObject.FindGameObjectWithTag ("Territories");
		territoryCount = territories.GetComponent<TerritoryCount> ();
		continentBonus = territories.GetComponent<ContinentBonus> ();
	}

	// claims the land once called - removes defender and places attacker
	public void ClaimLand(GameObject attackingCountry, GameObject defendingCountry){
		
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

		// update continent bonus dictionary - call wait time function
		StartCoroutine(ExecuteAfterTime(0.1f));
	}

	// this script has been created because BuildContBonus doesnt work properly without the delayed call
	IEnumerator ExecuteAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		continentBonus.BuildContBonus ();
	}


}
