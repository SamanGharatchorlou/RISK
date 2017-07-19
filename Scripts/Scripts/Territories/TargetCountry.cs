using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetCountry : MonoBehaviour {

	DisplayEditor displayEditor;
	ButtonColour buttonColour;
	TeamChecker teamChecker;
	Attack attack;
	GameInstructions gameInstructions;
	CountryManagement countryManagement;

	Text attackBtnText;

	GameObject GUI, scriptHolder;
	public GameObject attackingCountry, defendingCountry;
	GameObject previousCountry, selectedCountry;
	public Button attackButton;

	public bool selectingDefender;

	void Awake(){
		GUI = GameObject.FindGameObjectWithTag ("GUI");
		displayEditor = GUI.GetComponent<DisplayEditor> ();
		buttonColour = GUI.GetComponent<ButtonColour> ();
		gameInstructions = GUI.GetComponent<GameInstructions> ();

		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();
		attack = scriptHolder.GetComponent<Attack> ();
		countryManagement = scriptHolder.GetComponent<CountryManagement> ();
	}

	void Start(){
		selectingDefender = false;
	}
		
	//Label target as the attacker and sets up SetDefender function - Attack button
	void SetAttacker(){
		// Remove previous attckers tag
		previousCountry = GameObject.FindGameObjectWithTag ("AttackingCountry");
		if (previousCountry != null)
			previousCountry.gameObject.tag = "Untagged";

		// Selected country becomes attacker
		attackingCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
		// Can only attack with countries owned
		if (attackingCountry != null) {
			if (!selectingDefender & !teamChecker.UnderControl (attackingCountry) || countryManagement.GetArmySize (attackingCountry.name) == 1)
				return;
		}

		// prevents error if player changes target during attack
		if (attackingCountry == null & previousCountry != null) {
			previousCountry.gameObject.tag = "AttackingCountry";
			attackingCountry = previousCountry;
		} // this part should run most of the time
		else if (attackingCountry != null) {
			attackingCountry.gameObject.tag = "AttackingCountry";
			displayEditor.AttackingTerritory (attackingCountry);
		}

		// Allows SetDefender to be called as the following country selection in countrySelector
		if (!selectingDefender) {
			// runs here when 'attack' is selected
			selectingDefender = true;
			// change button colours
			buttonColour.BattleAttackColour (selectingDefender);
			// reomves +/- colour after 'attack' has been selected
			buttonColour.BattlePlusMinusColour (selectingDefender);
		} else {
			// runs here when 'deselect attacker' is selected - reverts game back to pre attack state
			selectingDefender = false;
			// prevents error if attacker tries to attack itself
			if (attackingCountry != null) {
				attackingCountry.gameObject.tag = "SelectedCountry";
				displayEditor.SelectedTerritory (attackingCountry);
				buttonColour.BattleBattleColour (attackingCountry,defendingCountry);
			} else {
				defendingCountry.gameObject.tag = "SelectedCountry";
				displayEditor.SelectedTerritory (defendingCountry);
				buttonColour.BattleBattleColour (attackingCountry,defendingCountry);
			}
			buttonColour.BattleAttackColour (selectingDefender);
		}
	}
		
	// Labels target as the defender
	public void SetDefender(){
		// Remove previous defenders tag
		previousCountry = GameObject.FindGameObjectWithTag ("DefendingCountry");
		if (previousCountry != null)
			previousCountry.gameObject.tag = "Untagged";

		// Selected country becomes defender
		defendingCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
		defendingCountry.gameObject.tag = "DefendingCountry";

		// corrects display if player targets its own country
		if (!teamChecker.UnderControl (defendingCountry) & attack.CanAttack(attackingCountry,defendingCountry)) {
			displayEditor.BattlingTerritories (attackingCountry, defendingCountry);
			buttonColour.BattleBattleColour (attackingCountry,defendingCountry);
		} else {
			displayEditor.CannotAttack (attackingCountry,defendingCountry);
			buttonColour.BattleBattleColour (attackingCountry,defendingCountry);
			gameInstructions.CannotAttack (attackingCountry, defendingCountry);
		}
	}

}