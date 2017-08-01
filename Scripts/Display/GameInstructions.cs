using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInstructions : MonoBehaviour {

	public Text instructionBox;

	GameObject scriptHolder;
	TeamChecker teamChecker;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();
	}

	//----------------opening/set up phase----------------

	// place troop during opening phase
	public void OpeningPhasePlacement(){
		instructionBox.text = "Select the country you'd like to place a troop on.";
	}

	// place all troops before moving on
	public void PlaceTroops(){
		instructionBox.text = "Deploy all your remaining troops then press 'End Setup Phase'";
	}

	//----------------battle phase----------------

	// select attacker
	public void SelectAtkCountry(){
		instructionBox.text = "Select the country you'd like to attack with, then press 'Attack'.";
	}

	// select the defending country
	public void SelectDefCountry(string attacker){
		instructionBox.text = "You selected " + attacker + ".\n" +
		"Who are you going to attack?";
	}

	// press battle button - DisplayEditor.BattlingTerritories
	public void PressBattle(){
		instructionBox.text = "Lets fight! - press 'Battle'.";
	}

	// shows battle outcome and keep battling/move onto movement phase
	public void BattleOutcome(int deadAttackers, int deadDefenders){
		instructionBox.text =
		"Either press battle again to continue fighting.\n" +
		"Select another country to attack with.\n" +
		"Or press 'End Battle Phase'";
	}

	// claimed country
	public void BattleClaim(GameObject newCountry){
		instructionBox.text = "You now have control of " + newCountry.name + 
		" Select another country to attack with or\n" +
		"press 'End Battle Phase'";
	}

	//----------------movement phase----------------

	// select the from country - Phases.EndBattlePhase
	public void SelectFromCountry(){
		instructionBox.text = "Select a country to move troops from, this is your 'from country'";
	}

	// select move button to lock in from country selection
	public void SelectFromMoveButton(string buttonText){
		instructionBox.text = "Press '" + buttonText + "'";
	}

	// select the to country
	public void SelectToCountry(){
		instructionBox.text = "Select a country to move troops to, this is your 'to country'";
	}

	// select move button to lock in to country selection
	public void SelectToMoveButton(string buttonText){
		instructionBox.text = "Press '" + buttonText + "' to confirm the selection";
	}

	// press +/- to transfer troops, then press end turn - CountrySelector.OnMouseDown & LinkedTerritories.SafePath
	public void MoveTroopButtons(GameObject fromCountry, GameObject toCountry){
		instructionBox.text = "Use the '+' and '-' buttons to transfer troops from " + fromCountry.name + " to " + toCountry.name +
		"\nThen press 'End Turn'";
	}

	//----------------error messages----------------

	// error message if countries aren't connected
	public void NotConnected(string fromCountry, string toCountry){
		instructionBox.text = fromCountry + " and " + toCountry + " aren't connected and/or under your control.\n" +
		"Try selecting something else.";
	}

	public void CannotMoveThere(GameObject country){
		instructionBox.text = "You cant move troops to/from " + country.name + "\ntrying selecting another country that you own";
	}

	public void NoSelection(string selection){
		instructionBox.text = "No " + selection + " selected.";
	}

	public void OneMovementTurn(){
		instructionBox.text = "You only get one movement turn";
	}

	public void CannotAttack(GameObject attacker, GameObject defender){
		if (teamChecker.UnderControl (defender))
			instructionBox.text = "You cant attack yourself...";
		else
			instructionBox.text = attacker.name + " and " + defender.name + " aren't neighbours";
	}

}
