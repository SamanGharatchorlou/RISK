using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves soldiers from one country to another during movement phase
public class ArmyMovement : MonoBehaviour {

	public AudioSource movementAudio;
	public AudioSource click;
	List<GameObject> soldiers;

	Phases phases;
	CountryManagement countryManagement;
	AddSoldier addSoldier;
	LinkedTerritories linkedTerritories;
	TroopCount troopCount;
	PlayerTurn playerTurn;
	TeamChecker teamChecker;
	GameInstructions gameInstructions;
	DisplayEditor displayEditor;
	ButtonColour buttonColour;

	public GameObject fromCountry, toCountry;
	GameObject territories, GUI;
	GameObject soldierToTransfer, storedCountry;

	public bool movementSelected, movementComplete;

	int fromArmySize, toArmySize, soldiersMoved, armySize;

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

	// moves troops from first selection (from) to second selection (to) (+ button)
	public void FwdMoveTroops(){
		if (phases.movementPhase) {
			click.Play ();
			fromArmySize = countryManagement.GetArmySize(fromCountry.name);
			if (linkedTerritories.SafePath (fromCountry, toCountry) & fromArmySize > 1) {
				MoveSoldier (fromCountry, toCountry);
				soldiersMoved++;
				displayEditor.InitiateMovement (fromCountry, toCountry, soldiersMoved);
				click.Play ();
			}
		}
	}

	// moves troops from second selection (to) to first selection (from) (- button)
	public void BackMoveTroops(){
		if (phases.movementPhase) {
			click.Play ();
			toArmySize = countryManagement.GetArmySize (toCountry.name);
			if (linkedTerritories.SafePath (fromCountry, toCountry) & toArmySize > 1) {
				MoveSoldier (toCountry, fromCountry);
				soldiersMoved--;
				displayEditor.InitiateMovement (fromCountry, toCountry, soldiersMoved);
				click.Play ();
			}
		}
	}

	// selects fromCountry - Move Button
	public void MoveToBtn(){
		// only run code on movement phase
		if (phases.movementPhase) {
			click.Play ();
			// sets selected country as fromCountry if undercontrol, has troops to move and fromCountry has not already been selected
			storedCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
			armySize = countryManagement.GetArmySize(storedCountry.name);
			// select fromCountry
			if (teamChecker.UnderControl (storedCountry) & armySize > 1 & !movementSelected & !movementComplete) {
				fromCountry = storedCountry;
				movementSelected = true;
				displayEditor.MovementFrom (fromCountry);
				// remove previous selection
				toCountry = null;
				buttonColour.MovementFromCountrySelected ();
			} 
			// reselect fromCountry
			else if (toCountry == null & movementSelected) {
				// remove previous selections
				fromCountry = null;
				toCountry = null;
				movementSelected = false;
			} 
			// movement confirmation - locks in from and to countries
			else if (fromCountry != null & toCountry != null) {
				movementComplete = true;
				buttonColour.MovementToCountrySelected ();
				movementAudio.Play ();
				displayEditor.InitiateMovement (fromCountry, toCountry, soldiersMoved);
				gameInstructions.MoveTroopButtons (fromCountry, toCountry);
			} else if (movementComplete)
				print ("movement phase already complete");
		}
	}
		
	// selects toCountry after fromCountry has been selected
	public void MovementCountries(GameObject country){
		storedCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
		// set selected country as toCountry if undercontrol and has a safe path
		if (teamChecker.UnderControl (storedCountry) & storedCountry != fromCountry & linkedTerritories.SafePath (fromCountry, storedCountry)) {
			toCountry = storedCountry;
			//buttonColour.MovementPlusMinusColour (fromCountry, toCountry);
			soldiersMoved = 0;
			displayEditor.MovementTo (fromCountry, toCountry);
			buttonColour.MovementSelectToCountry (country, "correct");
		} else {
			gameInstructions.CannotMoveThere (country);
			buttonColour.MovementSelectToCountry (country, "incorrect");
		}

		//gameInstructions.OneMovementTurn ();
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
