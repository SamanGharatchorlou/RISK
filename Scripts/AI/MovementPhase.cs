using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovementPhase : MonoBehaviour {

	List<string> fromCountries;
	List<string> inlandCountries;

	List<string> toCountries;
	List<string> controlledCountries;
	List<string> frontlineCountries;

	ArmyMovement armyMovement;
	LinkedTerritories linkedTerritories;
	CountryManagement countryManagement;
	GlobalFunctions globalFunctions;
	Phases phases;

	GameObject toCountry, fromCountry, frontlineCountry;

	string fromCountryName, toCountryName ;
	int movementDelay, inlandArmySize, inlandCountryMax, fromCountrySize;

	void Awake () {
		armyMovement = this.GetComponent<ArmyMovement> ();
		linkedTerritories = this.GetComponent<LinkedTerritories> ();
		countryManagement = this.GetComponent<CountryManagement> ();
		globalFunctions = this.GetComponent<GlobalFunctions> ();
		phases = this.GetComponent<Phases> ();
	}

	// Calls the MoveTroops method if required - called in AIController
	public void AIMoveSoldiers(){
		movementDelay = 1;
		// selects fromCountry and toCountry
		InlandToFrontline();
		// moves troops
		if (fromCountry != null)
			MoveAfterTime ();
		else
			// ----Ends player turn if no movement is required----
			phases.EndPhase ();
	}

	void MoveAfterTime(){
		fromCountrySize = countryManagement.GetArmySize (fromCountry.name);
			if (fromCountrySize > 1) {
			armyMovement.MoveSoldier (fromCountry, toCountry);
			Invoke ("MoveAfterTime", movementDelay);
		}
		// ----Ends player turn after movement----
		else
			phases.EndPhase ();
	}

	// Moves largest inland army to largest connected frontline country
	void InlandToFrontline(){
		//----fromCountry---- inland country with the largest army
		inlandCountries = new List<string> ();
		// list of countries with armies > 1
		fromCountries = globalFunctions.ControlledCountryList(2);
		// place all inland countries into list
		foreach (string fromCountry in fromCountries) {
			if (globalFunctions.EnemyNeighbourList (fromCountry).Count == 0)
				inlandCountries.Add (fromCountry);
		}
		// ensures inland countries with armies >1 exist
		if (inlandCountries.Count > 0) {
			// sets fromCountry to the inland country with the largest army
			inlandCountryMax = 0;
			foreach (string inlandCountry in inlandCountries) {
				inlandArmySize = countryManagement.GetArmySize (inlandCountry);
				// replaces max value with larger value and sets fromCountry
				if (inlandArmySize > inlandCountryMax) {
					inlandCountryMax = inlandArmySize;
					fromCountry = GameObject.Find (inlandCountry);
				}
			}
		}
		// otherwise reduce spread of armies
		else {
			ReduceSpread ();
			return;
		}

		//----toCountry---- connected frontline country with the largest army
		frontlineCountries = new List<string>();
		// list of all controlled countries
		controlledCountries = globalFunctions.ControlledCountryList (1);
		// list of all frontline countries
		foreach (string controlledCountry in controlledCountries) {
			if (globalFunctions.EnemyNeighbourList (controlledCountry).Count > 0)
				frontlineCountries.Add (controlledCountry);
		}
		// sort list by army size
		frontlineCountries = globalFunctions.SortList (frontlineCountries);
		// check if there is a safe path toCountry ordered by largest army sizes
		foreach (string frontlineCountryName in frontlineCountries) {
			frontlineCountry = GameObject.Find (frontlineCountryName);
			if (linkedTerritories.SafePath (fromCountry, frontlineCountry)) {
				toCountry = frontlineCountry;
				return;
			}
		}
	}

	// moves a smaller army to a larger one reducing spread of armies
	void ReduceSpread(){
		// reset fromCountry
		fromCountry = null;
		// being called means no inland countries have armies > 1 therefore can assume all
		// countries with armies > 1 are frontline countries
		frontlineCountries = globalFunctions.ControlledCountryList (2);
		
		// sort list by army size
		frontlineCountries = globalFunctions.SortList (frontlineCountries);
		// iterate through list starting with largest army
		for (int i = 0; i < frontlineCountries.Count; i++) {
			toCountry = GameObject.Find (frontlineCountries [i]);
			// iterate through all the smaller values relative to i
			for (int j = i + 1; j < frontlineCountries.Count; j++) {
				fromCountry = GameObject.Find (frontlineCountries [j]);
				// if there is safe passage between these two countries complete movement
				if (linkedTerritories.SafePath (fromCountry, toCountry))
					return;
				// if there is no safe passge no movement is required
				else
					fromCountry = null;
			}
		}
	}

}
