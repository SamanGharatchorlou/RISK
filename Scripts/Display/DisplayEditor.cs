using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayEditor : MonoBehaviour {

	CountryManagement countryManagement;
	TeamChecker teamChecker;
	GameInstructions gameInstructions;
	PlayerTurn playerTurn;

	public Text selectedCountryText;
	public Text defendingCountryText;

	GameObject scriptHolder, GUI;
	GameObject storedDefender;

	TextGenerator atkTextGen, defTextGen;
	TextGenerationSettings atkSetting, defSetting;

	Color storedColour;

	Vector3 defTextAdjust, atkTextPos ;
	string fromCountryName, toCountryName;
	float atkWidth, defWidth;
	int soldierNumbers, currentPlayer, enemyPlayer;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		countryManagement = scriptHolder.GetComponent<CountryManagement> ();
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();
		playerTurn = scriptHolder.GetComponent<PlayerTurn> ();

		GUI = GameObject.FindGameObjectWithTag ("GUI");
		gameInstructions = GUI.GetComponent<GameInstructions> ();
	}

	void Start(){
		atkTextGen = new TextGenerator ();
		defTextGen = new TextGenerator ();
	}


	// ------------------------- OPENING PHASE  -------------------------

	// "1 Soldier placed on country(size)" - AllocateSoldiers.DropSoldier
	public void OpeningPlaceSoldier(GameObject country){
		DefaultPosition();
		selectedCountryText.text = "1 Soldier placed on " + country.name + AddCountrySize(country);
		SetColour (selectedCountryText, country);
	}


	// ------------------------- SETUP PHASE  -------------------------

	// "X soldiers added to country"
	public void SetupDeploySoldier(GameObject country){
		DefaultPosition();
		// counts up all the soldiers placed on the country this turn
		soldierNumbers = 0;
		foreach (Transform soldier in country.transform) {
			if (soldier.tag == "DeployedSoldier") {
				soldierNumbers++;
			}
		}
		selectedCountryText.text = soldierNumbers + " soldier(s) added to " + country.name + AddCountrySize (country);
		SetColour (selectedCountryText, country);
	}

	// ------------------------- ATTACK PHASE  -------------------------


	// ---------- Attack ----------

	// Display "attacker -->"
	public void AttackingTerritory(GameObject attacker){
		DefaultPosition();
		// set attack text
		selectedCountryText.text = attacker.name + "(" + countryManagement.GetArmySize (attacker.name) + ") is attacking... ";
		SetColour (selectedCountryText, attacker);
		// remove defender text
		RemoveBattleText();
		gameInstructions.SelectDefCountry (attacker.name);
	}

	// Display "attacker --> defender"
	public void BattlingTerritories(GameObject attacker, GameObject defender){
		if (attacker == null || defender == null)
			return;
		DefaultPosition();
		// set attacker text
		selectedCountryText.text = attacker.name + "(" + countryManagement.GetArmySize (attacker.name) + ") is attacking ";
		SetColour (selectedCountryText, attacker);
		// set defender text
		defendingCountryText.text = defender.name + "(" + countryManagement.GetArmySize (defender.name) + ")";
		SetColour (defendingCountryText, defender);
		// stores defender attributes for BattleResult use
		storedDefender = defender;
		storedColour = teamChecker.PlayerColour (defender);

		HorizontalBattleTxtPos ();
		gameInstructions.PressBattle ();
	}
		
	// ---------- Battle ----------

	// shows battle outcome and keep battling/move onto movement phase - Attack.ATTACK
	public void BattleResult(GameObject attackingCountry, GameObject defendingCountry, int deadAttackers, int deadDefenders){
		VerticalBattleTxtPos ();
		// player lost x soldiers
		currentPlayer = playerTurn.CurrentPlayer();
		// customised text if player is attacking
		if (currentPlayer == 1)
			selectedCountryText.text = "You lost: " + deadAttackers + " soldier(s)";
		else
			selectedCountryText.text = "Player " + playerTurn.CurrentPlayer () + " lost: " + deadAttackers + " soldier(s)";
		SetColour (selectedCountryText, attackingCountry);
		// defender lost x soldiers
		enemyPlayer = teamChecker.GetPlayer (storedDefender);
		// customised text if player is defending
		if (enemyPlayer == 1)
			defendingCountryText.text = "You lost " + deadDefenders + " soldier(s)";
		else
			defendingCountryText.text = "Player " + teamChecker.GetPlayer (storedDefender) + " lost " + deadDefenders + " soldier(s)";
		defendingCountryText.color = storedColour;
	}

	public void CannotAttack(GameObject attacker, GameObject defender){
		DefaultPosition();
		// attacker cant attack...
		selectedCountryText.text = attacker.name + " cannot attack ";
		SetColour (selectedCountryText, attacker);
		// defender
		defendingCountryText.text = defender.name;
		SetColour (defendingCountryText, defender);
		HorizontalBattleTxtPos ();
	}

	// ------------------------- MOVEMENT  -------------------------

	// movement button - fromCountry selected
	public void MovementFrom(GameObject fromCountry){
		DefaultPosition ();
		selectedCountryText.text = "Moving troops from " + fromCountry.name;
		selectedCountryText.color = teamChecker.GetColour (playerTurn.CurrentPlayer ());
	}

	// selected fromCountry - toCountry selected
	public void MovementTo(GameObject fromCountry, GameObject toCountry){
		selectedCountryText.text = "Moving troops from " + fromCountry.name + " to " + toCountry.name;
		SetColour (selectedCountryText, fromCountry);
	}

	// Selected toCountry & +/- buttons
	public void InitiateMovement(GameObject fromCountry, GameObject toCountry, int troopsMoved){
		// reverses names if more troops are moved to fromCountry than toCountry
		if (troopsMoved < 0) {
			fromCountryName = toCountry.name;
			toCountryName = fromCountry.name;
			troopsMoved = -troopsMoved;
		} else {
			fromCountryName = fromCountry.name;
			toCountryName = toCountry.name;
		}
		selectedCountryText.text = troopsMoved + " soldier(s) moved from " + fromCountryName + " to " + toCountryName;
		SetColour (selectedCountryText, fromCountry);
	}


	// ------------------------- OTHER  -------------------------

	// returns "(country size)"
	string AddCountrySize(GameObject country){
		return " (" + countryManagement.GetArmySize (country.name) + ")";
	}

	// set colour of text
	void SetColour(Text textBox, GameObject country){
		textBox.color = teamChecker.PlayerColour (country);
	}
		
	// Display selected country stats
	public void SelectedTerritory(GameObject country){
		DefaultPosition();
		selectedCountryText.text = country.name + AddCountrySize(country) + " selected";
		SetColour (selectedCountryText, country);
		RemoveBattleText ();
	}

	// removes defender text when not valid anymore
	public void RemoveBattleText(){
		DefaultPosition();
		defendingCountryText.text = "";
	}

	// dual box: row set up
	void DefaultPosition(){
		atkTextPos = new Vector3 ((Screen.width*0.8f)/2f, Screen.height*0.06f, 0);
		selectedCountryText.transform.position = atkTextPos;
	}

	// dual box: column set up
	void PositionOne(Text textBox){
		atkTextPos = new Vector3 (Screen.width/ 2f, Screen.height * 0.8f, 0);
		textBox.transform.position = atkTextPos;
	}

	// dynamically adjusts the horizontal textbox positions
	void HorizontalBattleTxtPos(){
		DefaultPosition();
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

	// adjusts the vetical textbox positions
	void VerticalBattleTxtPos(){
		PositionOne (selectedCountryText);
		// build adjustment vectors
		defTextAdjust = new Vector3 (0,-20,0);
		atkTextPos = selectedCountryText.transform.position;
		// adjust defender text position
		defendingCountryText.transform.SetPositionAndRotation (atkTextPos + defTextAdjust,Quaternion.identity);
	}



}
