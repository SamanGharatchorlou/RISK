using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountrySelector : MonoBehaviour {

	DisplayEditor displayEditor;
	TargetCountry targetCountry;
	Phases phases;
	AllocateSoldiers allocateSoldiers;
	GameInstructions gameInstructions;
	ButtonColour buttonColour;
	ArmyMovement armyMovement;
	TeamChecker teamChecker;
	PlayerTurn playerTurn;

	public GameObject country;
	GameObject scriptHolder, GUI;
	GameObject previousCountry, toCountry;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		allocateSoldiers = scriptHolder.GetComponent<AllocateSoldiers> ();
		targetCountry = scriptHolder.GetComponent<TargetCountry> ();
		phases = scriptHolder.GetComponent<Phases> ();
		armyMovement = scriptHolder.GetComponent<ArmyMovement> ();
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();
		playerTurn = scriptHolder.GetComponent<PlayerTurn> ();

		GUI = GameObject.FindGameObjectWithTag ("GUI");
		displayEditor = GUI.GetComponent<DisplayEditor> ();
		gameInstructions = GUI.GetComponent<GameInstructions> ();
		buttonColour = GUI.GetComponent<ButtonColour> ();
		}

	void OnMouseDown(){
		//Remove presvious country selected and add tag to new country selection
		previousCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");

		// removes tag from previously selected country
		if (previousCountry != null)
			previousCountry.gameObject.tag = "Untagged";

		// updates the current country selection
		country = gameObject.transform.parent.gameObject;
		country.gameObject.tag = "SelectedCountry";

		// runs game instructions when player needs to select the +/- to move troops
		if (phases.movementPhase & previousCountry == null)
			gameInstructions.MoveTroopButtons (country.name,country.name);

		// sets the target as "defender" rather than "selectedCountry" when target country to attack
		if (targetCountry.selectingDefender == true) {
			targetCountry.SetDefender ();
			return;
		}
			
		// place soldiers during the opening phase script
		if (phases.openingPhase) {
			allocateSoldiers.DropSoldier (country);
		}

		// Runs display selected country, doesnt run when selecting attacker or defender
		displayEditor.SelectedTerritory (country);

		// activates and deactivates button colours
		buttonColour.SetupPlusMinusColour ();
		buttonColour.BattleAttackColour ();
		buttonColour.MovementMoveColour ();
		if (phases.movementPhase & armyMovement.fromCountry != null)
			buttonColour.MovementPlusMinusColour(armyMovement.CanMoveArmy(country));
	}
		
}
