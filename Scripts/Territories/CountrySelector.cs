using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: think about adding a 3D selection arrow signalling who they have just selected?

public class CountrySelector : MonoBehaviour {

	DisplayEditor displayEditor;
	TargetCountry targetCountry;
	Phases phases;
	AllocateSoldiers allocateSoldiers;
	GameInstructions gameInstructions;
	ButtonColour buttonColour;
	ArmyMovement armyMovement;

	public GameObject country;
	GameObject scriptHolder, GUI;
	GameObject previousCountry;
	GameObject fromCountry, toCountry;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		allocateSoldiers = scriptHolder.GetComponent<AllocateSoldiers> ();
		targetCountry = scriptHolder.GetComponent<TargetCountry> ();
		phases = scriptHolder.GetComponent<Phases> ();
		armyMovement = scriptHolder.GetComponent<ArmyMovement> ();

		GUI = GameObject.FindGameObjectWithTag ("GUI");
		displayEditor = GUI.GetComponent<DisplayEditor> ();
		gameInstructions = GUI.GetComponent<GameInstructions> ();
		buttonColour = GUI.GetComponent<ButtonColour> ();
		}

	//Remove presvious country selected and add tag to new country selection
	void OnMouseDown(){
		
		// removes tag from previously selected country
		previousCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
		if (previousCountry != null)
			previousCountry.gameObject.tag = "Untagged";

		// updates the current country selection
		country = gameObject.transform.parent.gameObject;
		country.gameObject.tag = "SelectedCountry";

		// runs game instructions when player needs to select the +/- to move troops
		if (phases.movementPhase & previousCountry == null)
			gameInstructions.MoveTroopButtons (country.name,country.name);

		// selects countries to move troops between
		if (armyMovement.movementSelected)
			armyMovement.MovementCountries (country);

		// activates and deactivates button colours
		buttonColour.SetupPlusMinusColour ();
		buttonColour.BattleAttackColour (targetCountry.selectingDefender);

		// sets the target as "defender" rather than "selectedCountry" when target country to attack
		if (targetCountry.selectingDefender) {
			targetCountry.SetDefender ();
			return;
		}
			
		// place soldiers during the opening phase script
		if (phases.openingPhase)
			allocateSoldiers.DropSoldier (country);
			
		// Runs display selected country, doesnt run when selecting attacker or defender
		displayEditor.SelectedTerritory (country);
	}
		
}
