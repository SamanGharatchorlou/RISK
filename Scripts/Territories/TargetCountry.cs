using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: once attack button has been presses any country selection is the defending country until either battle is presses
//		or attack has been pressed again to deactivate bool or something

public class TargetCountry : MonoBehaviour {

	DisplayEditor displayEditor;
	ButtonColour buttonColour;

	GameObject GUI;
	GameObject attackingCountry, defendingCountry, previousCountry;

	public bool selectingDefender;

	void Awake(){
		GUI = GameObject.FindGameObjectWithTag ("GUI");
		displayEditor = GUI.GetComponent<DisplayEditor> ();
		buttonColour = GUI.GetComponent<ButtonColour> ();
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

		if (attackingCountry != null) {
			attackingCountry.gameObject.tag = "AttackingCountry";

			displayEditor.AttackingTerritory (attackingCountry);
			// Allows SetDefender to be called as the following selection
			selectingDefender = true;

			buttonColour.BattleAttackColour ();
			buttonColour.BattlePlusMinusColour (selectingDefender);
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

		displayEditor.BattlingTerritories (attackingCountry,defendingCountry);

		selectingDefender = false;

		buttonColour.BattleBattleColour (defendingCountry);
	}

}
