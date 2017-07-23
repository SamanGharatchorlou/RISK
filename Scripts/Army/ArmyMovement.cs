﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves soldiers from one country to another during movement phase
public class ArmyMovement : MonoBehaviour {

	List<GameObject> soldiers;

	Phases phases;
	CountryManagement countryManagement;
	AddSoldier addSoldier;
	LinkedTerritories linkedTerritories;
	TroopCount troopCount;
	PlayerTurn playerTurn;
	CountrySelector countrySelector;
	TeamChecker teamChecker;
	GameInstructions gameInstructions;
	DisplayEditor displayEditor;
	ButtonColour buttonColour;

	public GameObject fromCountry, toCountry;
	GameObject territories, GUI;
	GameObject soldierToTransfer;

	public bool movementSelected;

	int fromArmySize, toArmySize, soldiersMoved;

	void Awake(){
		territories = GameObject.FindGameObjectWithTag ("Territories");
		troopCount = territories.GetComponent<TroopCount> ();

		GUI = GameObject.FindGameObjectWithTag ("GUI");
		gameInstructions = GUI.GetComponent<GameInstructions> ();
		buttonColour = GUI.GetComponent<ButtonColour> ();
		displayEditor = GUI.GetComponent<DisplayEditor> ();

		phases = this.GetComponent<Phases> ();
		countryManagement = this.GetComponent<CountryManagement> ();
		addSoldier = this.GetComponent<AddSoldier> ();
		linkedTerritories = this.GetComponent<LinkedTerritories> ();
		playerTurn = this.GetComponent<PlayerTurn> ();
		teamChecker = this.GetComponent<TeamChecker> ();

	}

	public void MoveToBtn(){
		// only run code on movement phase
		if (phases.movementPhase) {
			movementSelected = true;
			displayEditor.MovementFrom ();
		}
	}

	// moves troops from first selection (from) to second selection (to) (+ button)
	public void FwdMoveTroops(){
		if (phases.movementPhase) {
			fromArmySize = countryManagement.GetArmySize(fromCountry.name);
			if (linkedTerritories.SafePath (fromCountry, toCountry) & fromArmySize > 1) {
				MoveSoldier (fromCountry, toCountry);
				soldiersMoved++;
				displayEditor.InitiateMovement (fromCountry, toCountry, soldiersMoved);
			}
		}
	}

	// moves troops from second selection (to) to first selection (from) (- button)
	public void BackMoveTroops(){
		if (phases.movementPhase) {
			toArmySize = countryManagement.GetArmySize (toCountry.name);
			if (linkedTerritories.SafePath (fromCountry, toCountry) & toArmySize > 1) {
				MoveSoldier (toCountry, fromCountry);
				soldiersMoved--;
				displayEditor.InitiateMovement (fromCountry, toCountry, soldiersMoved);
			}
		}
	}

	// sets from and to country for movement phase - only accepts valid countries
	public void MovementCountries(GameObject country){
		// selects fromCountry
		if (fromCountry == null) {
			if (teamChecker.UnderControl (country) & countryManagement.GetArmySize (country.name) > 1) {
				fromCountry = country;
				buttonColour.MovementMoveColour ("unactive");
				gameInstructions.SelectToCountry (fromCountry);
				displayEditor.MovementTo (fromCountry);
			} else
				gameInstructions.CannotMoveThere (country);
		} 
		// selects toCountry
		else if (toCountry == null) {
			if (teamChecker.UnderControl (country) & country != fromCountry) {
				toCountry = country;
				buttonColour.MovementPlusMinusColour (fromCountry, toCountry);
				soldiersMoved = 0;
				displayEditor.InitiateMovement (fromCountry, toCountry, soldiersMoved);
			} else
				gameInstructions.CannotMoveThere (country);
		} else
			gameInstructions.OneMovementTurn ();
	}

	// moves a soldier fromCountry to toCountry - called by fwd and back buttons
	public void MoveSoldier(GameObject fromCountry, GameObject toCountry){
		// create a list of the fromCountry's soldiers
		soldiers = new List<GameObject> ();
		foreach (Transform soldier in fromCountry.transform) {
			if (soldier.name == "Soldier(Clone)")
				soldiers.Add (soldier.gameObject);
		}
		// remove the last soldier fromCountry
		soldierToTransfer = soldiers [soldiers.Count - 1];
		GameObject.DestroyImmediate (soldierToTransfer);
		// add a soldier toCountry
		addSoldier = toCountry.GetComponent<AddSoldier> ();
		addSoldier.PlaceSoldier ();

		// update game stats
		troopCount.UpdateTroopBankV2 (playerTurn.CurrentPlayer (), -1);
		countryManagement.ChangeArmySize (fromCountry, -1);
		countryManagement.ChangeArmySize (toCountry, 1);

		buttonColour.MovementPlusMinusColour (fromCountry, toCountry);
	}

	// reset move phase end of player turn - Phases.EndMovement
	public void ResetMoveVariables(){
		movementSelected = false;
		fromCountry = null;
		toCountry = null;
	}
		
}
