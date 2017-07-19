using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSoldier : MonoBehaviour {

	PlayerTurn playerTurn;
	CountryManagement countryManagement;
	SoldierManagement soldierManagement;
	TroopCount troopCount;
	TeamChecker teamChecker;
	Phases phases;

	GameObject scriptHolder, territories;
	GameObject placer, clone;

	Vector3 childPosition, childMassVector;
	Vector3 countryMassVector, countryCOM;
	Vector3 soldierPosition, adjustment;

	float childMass, countryMass;
	float r, g, b, a;
	int soldierNumber;
	int x, n;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		countryManagement = scriptHolder.GetComponent<CountryManagement> ();
		soldierManagement = scriptHolder.GetComponent<SoldierManagement> ();
		playerTurn = scriptHolder.GetComponent<PlayerTurn> ();
		phases = scriptHolder.GetComponent<Phases> ();

		territories = GameObject.FindGameObjectWithTag ("Territories");
		troopCount = territories.GetComponent<TroopCount> ();
	}

	// Finds the county's C.O.M
	public Vector3 findCOM(Transform countryTransform){
		// initialise values to 0
		countryMass = 0;
		countryMassVector = new Vector3(0,0,0);
		countryCOM = new Vector3 (0,0,0);

		foreach(Transform child in countryTransform){
			// Sum all the individual child mass vectors into childMassVector
			childMass = child.localScale.x * child.localScale.z;
			childPosition = child.position;
			childMassVector = childPosition * childMass;

			countryMass = countryMass + childMass;
			countryMassVector = countryMassVector + childMassVector;

			countryCOM = countryMassVector / countryMass;
			}
			
		return countryCOM;
	}

	// Adjusts the location of the solider being placed creating a square of increasing size
	Vector3 PositionAdjustment(Vector3 countryCOM){
		adjustment = new Vector3 (0, 0, 0);

		// Get the current number of soldiers (including the one being added)
		soldierNumber = countryManagement.GetArmySize (this.name) + 1;

		// place a "placer" object at the COM - this will become the soldiers location
		placer = Instantiate(Resources.Load("Placer"),countryCOM,Quaternion.identity) as GameObject;
		placer.transform.parent = this.transform;
		x = 0;
		n = 0;
		// Some crazy ass code that places the "placer" in a square shape of increasing size depending on the number
		// of soldiers the country has
		if (soldierNumber > 1) {
			for (int i = 1; i <= soldierNumber; i++) {
				// Sets the number of fowards to execute before a rotation
				if (x % 2 == 0 & x > 0)
					n++;
				// Iterates through the number of times it needs to move forward according to n
				for (int a = 0; a <= n; a++) {
					// Moves the placer forward one
					if (i == soldierNumber)
						break;
					placer.transform.Translate (Vector3.forward * 4.5f);
					i++;
				}
				// Rotates the placer right
				placer.transform.Rotate(Vector3.up * 90, Space.Self);
				x++;
				if (i != soldierNumber)
					i--;
			}
		}
		// Stores the placers position and removes placer
		adjustment = placer.transform.position;
		Destroy (placer);
			
		return adjustment;
	}

	// Places a soldier at the country's adjusted COM
	public void PlaceSoldier(){
		soldierPosition = new Vector3 (0, 0, 0);
		soldierPosition = PositionAdjustment(findCOM(this.transform));

		// Instantiates a soldier at the stored placer location
		clone = Instantiate (Resources.Load("Soldier"), soldierPosition, Quaternion.identity) as GameObject;
		clone.transform.parent = this.transform;
		if (phases.setupPhase)
			clone.gameObject.tag = "DeployedSoldier";
		// requires player index as unput, hence the -1
		soldierManagement.SetSoldierColour(clone, playerTurn.CurrentPlayer()-1);
		// update game stats
		troopCount.UpdateTroopBankV2(playerTurn.CurrentPlayer(),1);
	}

}