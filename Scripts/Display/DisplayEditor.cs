using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayEditor : MonoBehaviour {

	CountryManagement countryManagement;
	TeamChecker teamChecker;
	GameInstructions gameInstructions;

	public Text selectedCountryText;
	public Text defendingCountryText;

	GameObject scriptHolder, GUI;

	TextGenerator atkTextGen, defTextGen;
	TextGenerationSettings atkSetting, defSetting;

	Vector3 defTextAdjust, atkTextPos ;
	float atkWidth, defWidth;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		countryManagement = scriptHolder.GetComponent<CountryManagement> ();
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();
		GUI = GameObject.FindGameObjectWithTag ("GUI");
		gameInstructions = GUI.GetComponent<GameInstructions> ();
	}

	void Start(){
		atkTextGen = new TextGenerator ();
		defTextGen = new TextGenerator ();
	}

	// Display selected country stats
	public void SelectedTerritory(GameObject country){
		selectedCountryText.text = country.name + "(" + countryManagement.GetArmySize (country.name) + ")";
		selectedCountryText.color = teamChecker.PlayerColour (country);
		RemoveBattleText ();
	}

	// Display "attacker -->"
	public void AttackingTerritory(GameObject attacker){
		selectedCountryText.text = attacker.name + "(" + countryManagement.GetArmySize (attacker.name) + ") is attacking... ";
		selectedCountryText.color = teamChecker.PlayerColour (attacker);
		gameInstructions.SelectDefCountry (attacker.name);

	}

	// Display "attacker --> defender"
	public void BattlingTerritories(GameObject attacker, GameObject defender){
		if (attacker == null || defender == null)
			return;
		// set attacker text
		selectedCountryText.text = attacker.name + "(" + countryManagement.GetArmySize (attacker.name) + ") is attacking ";
		selectedCountryText.color = teamChecker.PlayerColour (attacker);
		// set defender text
		defendingCountryText.text = defender.name + "(" + countryManagement.GetArmySize (defender.name) + ")";
		defendingCountryText.color = teamChecker.PlayerColour (defender);

		BattleTxtPos ();
		gameInstructions.PressBattle ();
	}

	// removes defender text when not valid anymore
	public void RemoveBattleText(){
		defendingCountryText.text = "";
	}

	// dynamically adjusts the textbox positions
	void BattleTxtPos(){
		// set up settings
		atkSetting = selectedCountryText.GetGenerationSettings (selectedCountryText.rectTransform.rect.size);
		defSetting = defendingCountryText.GetGenerationSettings (defendingCountryText.rectTransform.rect.size);
		// calc textbox widths
		atkWidth = atkTextGen.GetPreferredWidth (selectedCountryText.text, atkSetting);
		defWidth = defTextGen.GetPreferredWidth (defendingCountryText.text, defSetting);
		// build adjustment vectors
		defTextAdjust = new Vector3 ((atkWidth + defWidth) / 2,0,0);
		atkTextPos = selectedCountryText.transform.position;
		// adjust defender text position according to its own and attcker text size
		defendingCountryText.transform.SetPositionAndRotation (atkTextPos + defTextAdjust,Quaternion.identity);
	}
		
		
}
