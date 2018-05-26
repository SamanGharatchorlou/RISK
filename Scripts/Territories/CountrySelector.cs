using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountrySelector : MonoBehaviour {

	DisplayEditor displayEditor;
	TargetCountry targetCountry;
	Phases phases;
	AllocateSoldiers allocateSoldiers;
	ButtonColour buttonColour;
	ArmyMovement armyMovement;
	PlayerTurn playerTurn;
	AudioFadeOut audioFadeOut;
	TeamChecker teamChecker;

	public GameObject country;
	GameObject scriptHolder, GUI;
	GameObject previousCountry;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		allocateSoldiers = scriptHolder.GetComponent<AllocateSoldiers> ();
		targetCountry = scriptHolder.GetComponent<TargetCountry> ();
		phases = scriptHolder.GetComponent<Phases> ();
		armyMovement = scriptHolder.GetComponent<ArmyMovement> ();
		playerTurn = scriptHolder.GetComponent<PlayerTurn> ();
		audioFadeOut = scriptHolder.GetComponent<AudioFadeOut> ();
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();

		GUI = GameObject.FindGameObjectWithTag ("GUI");
		displayEditor = GUI.GetComponent<DisplayEditor> ();
		buttonColour = GUI.GetComponent<ButtonColour> ();
		}

	//Remove presvious country selected and add tag to new country selection
	void OnMouseDown(){
		audioFadeOut.Click ();
		//TODO: adjust this to take the number human players into account
		// cannot select countries during AI turn
		if (playerTurn.CurrentPlayer () != 1)
			return;
		
		// removes tag from previously selected country
		previousCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
		if (previousCountry != null)
			previousCountry.gameObject.tag = "Untagged";

		// updates the current country selection
		country = gameObject.transform.parent.gameObject;
		country.gameObject.tag = "SelectedCountry";

		// activates and deactivates button colours
		buttonColour.SetupPlusMinusColour ();
		if (phases.battlePhase)
			buttonColour.BattleAttackColour (targetCountry.selectingDefender);

		// sets the target as "defender" rather than "selectedCountry" when target country to attack
		if (targetCountry.selectingDefender) {
			targetCountry.SetDefender ();
			return;
		}

		// movement phase mechanics
		if (phases.movementPhase) {
			buttonColour.MovementSelectFromCountry(country);
			// selects countries to move troops between
			if(armyMovement.movementSelected)
				armyMovement.MovementCountries (country);
		}

		// doesnt display selected territory during movement phase (other display is shown)
		if (armyMovement.movementSelected)
			return;
		
		// place soldiers during the opening phase script
		if (phases.openingPhase & teamChecker.UnderControl(country)) {
			allocateSoldiers.DropSoldier (country);
			audioFadeOut.MoreTroopsAudio ();
		}

		// Runs display selected country, doesnt run when selecting attacker or defender
		displayEditor.SelectedTerritory (country);
	}
		
}
