using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI - movement Phase
public class SetupPhase : MonoBehaviour {

	// random slection
	List<string> controlledCountries;
	// front line selection
	List<string> frontLineCountries;
	List<string> neighbouringEnemies;
	GameObject[] previousSelections;

	DeploySoldiers deploySolders;
	ArmyManagement armyManagement;
	GlobalFunctions globalFunctions;
	Phases phases;
	AttackPhase attackPhase;

	GameObject randomCountry, selectedCountry;

	string randomCountryName;
	int randomCountryIndex;
	float placementDelay;

	void Awake () {
		deploySolders = this.GetComponent<DeploySoldiers> ();
		armyManagement = this.GetComponent<ArmyManagement> ();
		globalFunctions = this.GetComponent<GlobalFunctions> ();
		phases = this.GetComponent<Phases> ();
		attackPhase = this.GetComponent<AttackPhase> ();
	}

	//place troops on a chosen country - to be called by AI controller
	public void AIPlaceTroops(){
		placementDelay = globalFunctions.timeDelay;
		// select a single front line country
		SelectFrontLineCountry();
		// place all soldiers onto 1 frontline country
		PlaceAfterTime();
	}

	// places soldier, waits a given time, executes itself again
	void PlaceAfterTime(){
		if (deploySolders.soldiersLeft > 0) {
			armyManagement.Add ();
			Invoke ("PlaceAfterTime", placementDelay);
		} 
		// ---- Begin attack phase ----
		else {
			phases.EndPhase ();
			attackPhase.AIAttackCountry ();
		}
	}

	//TODO: move this to globalFunctions? also used by starterPhase

	// selects a random country under player control given that it has an enemy neighbour
	public void SelectFrontLineCountry(){
		frontLineCountries = new List<string> ();
		// removes all previous selection - why is there more than 1? dont know but there is sometimes...
		previousSelections = GameObject.FindGameObjectsWithTag ("SelectedCountry");
		foreach(GameObject selection in previousSelections)
			selection.gameObject.tag = "Untagged";
		
		// build a list of all controlled countries
		controlledCountries = globalFunctions.ControlledCountryList (1);
		foreach (string country in controlledCountries) {
			neighbouringEnemies = globalFunctions.EnemyNeighbourList (country);
			if (neighbouringEnemies.Count > 0)
				frontLineCountries.Add (country);
		}
		// select random country from selection
		randomCountryIndex = Mathf.FloorToInt (Random.Range (0f, frontLineCountries.Count));
		randomCountry = GameObject.Find (frontLineCountries [randomCountryIndex]);
		randomCountry.gameObject.tag = "SelectedCountry";
	}
		
}
