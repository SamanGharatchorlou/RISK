using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountrySelector : MonoBehaviour {

	DisplayEditor displayEditor;
	TargetCountry targetCountry;
	Phases phases;
	AllocateSoldiers allocateSoldiers;
	GameInstructions gameInstructions;

	GameObject scriptHolder, GUI;
	public GameObject country;
	GameObject previousCountry;

	void Start(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		GUI = GameObject.FindGameObjectWithTag ("GUI");

		allocateSoldiers = scriptHolder.GetComponent<AllocateSoldiers> ();
		targetCountry = scriptHolder.GetComponent<TargetCountry> ();
		phases = scriptHolder.GetComponent<Phases> ();
		displayEditor = GUI.GetComponent<DisplayEditor> ();
		gameInstructions = GUI.GetComponent<GameInstructions> ();
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
	}
		
}
