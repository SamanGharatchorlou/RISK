using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves soldiers from one country to another during movement phase
public class ArmyMovement : MonoBehaviour {

	List<GameObject> soldiers;

	Phases phases;
	CountryManagement countryManagement;
	AddSoldier addSoldier;
	TeamChecker teamChecker;
	LinkedTerritories linkedTerritories;
	GameInstructions gameInstructions;
	TroopCount troopCount;
	PlayerTurn playerTurn;

	public GameObject fromCountry, toCountry;

	GameObject GUI, territories;
	GameObject soldierToTransfer;

	bool availablePathing, sameTeam;

	int fromArmySize, toArmySize;
	int fromCountryTeam, toCountryTeam;

	void Awake(){
		GUI = GameObject.FindGameObjectWithTag ("GUI");
		gameInstructions = GUI.GetComponent<GameInstructions> ();

		territories = GameObject.FindGameObjectWithTag ("Territories");
		troopCount = territories.GetComponent<TroopCount> ();

		phases = this.GetComponent<Phases> ();
		countryManagement = this.GetComponent<CountryManagement> ();
		addSoldier = this.GetComponent<AddSoldier> ();
		teamChecker = this.GetComponent<TeamChecker> ();
		linkedTerritories = this.GetComponent<LinkedTerritories> ();
		playerTurn = this.GetComponent<PlayerTurn> ();
	}

	// move troops to... button
	public void MoveToBtn(){
		// only run code on movement phase
		if (phases.movementPhase) {
			fromCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
			fromCountry.tag = "Untagged";
			gameInstructions.SelectToCountry (fromCountry.name);
		}
	}

	// moves troops from first selection (from) to second selection (to) (+ button)
	public void FwdMoveTroops(){
		// only run code on movement phase
		if (phases.movementPhase) {
			toCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
			// only runs if two countries selected
			if (CanMoveArmy (fromCountry, toCountry) & fromArmySize > 1)
				MoveSoldier (fromCountry, toCountry);
		}
	}

	// moves troops from second selection (to) to first selection (from) (- button)
	public void BackMoveTroops(){
		// only run code on movement phase
		if (phases.movementPhase) {
			toCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
			// only runs if two countries selected
			if (CanMoveArmy (fromCountry, toCountry) & toArmySize > 1)
				MoveSoldier (toCountry, fromCountry);
		}
	}

	// moves a soldier fromCountry to toCountry - called by fwd and back buttons
	public void MoveSoldier(GameObject fromCountry, GameObject toCountry){
		// create a list of the fromCountry's soldiers
		soldiers = new List<GameObject> ();
		foreach (Transform soldier in fromCountry.transform) {
			if (soldier.name == "Soldier(Clone)")
				soldiers.Add (soldier.gameObject);
		}
		// remove the last soldier fromCountry in soldiers
		soldierToTransfer = soldiers [soldiers.Count - 1];
		GameObject.DestroyImmediate (soldierToTransfer);
		// update game stats
		troopCount.UpdateTroopBankV2 (playerTurn.CurrentPlayer (), -1);
		countryManagement.ChangeArmySize (fromCountry, -1);

		// add a soldier toCountry
		addSoldier = toCountry.GetComponent<AddSoldier> ();
		addSoldier.PlaceSoldier ();
		// update game stats
		countryManagement.ChangeArmySize (toCountry, 1);
	}
		

	// makes the required checks to ensure soldiers can move between countries
	bool CanMoveArmy(GameObject fromCountry, GameObject toCountry){
		// checks who owns the given countries
		fromCountryTeam = teamChecker.GetPlayer (fromCountry);
		toCountryTeam = teamChecker.GetPlayer (toCountry);
		sameTeam = false;
		if (fromCountryTeam == toCountryTeam)
			sameTeam = true;

		// checks that there's at least 1 man left behind - create variables for +/- script to check as they have diff requirements
		fromArmySize = countryManagement.GetArmySize(fromCountry.name);
		toArmySize = countryManagement.GetArmySize (toCountry.name);

		// checks if the countries are connected
		availablePathing = false;
		availablePathing = linkedTerritories.SafePath (fromCountry, toCountry);

		// checks if all conditions are satisfied
		if (sameTeam & availablePathing)
			return true;
		else
			return false;
	}

}
