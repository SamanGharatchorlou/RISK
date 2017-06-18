using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCountry : MonoBehaviour {

	DisplayEditor displayEditor;

	GameObject GUI;
	GameObject attackingCountry, defendingCountry, previousCountry;

	public bool selectingDefender;

	void Awake(){
		GUI = GameObject.FindGameObjectWithTag ("GUI");
		displayEditor = GUI.GetComponent<DisplayEditor> ();
	}

	void Start(){
		selectingDefender = false;
	}

	//Label target as the attacker and sets up SetDefender function
	public void SetAttacker(){
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

	}

}
