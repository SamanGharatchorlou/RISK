using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInstructions : MonoBehaviour {

	public Text instructionBox;

	ArmyMovement armyMovement;
	GameObject scriptHolder;
	TeamChecker teamChecker;

	string fromCountry, toCountry;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		armyMovement = scriptHolder.GetComponent<ArmyMovement> ();
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();
	}

	//----------------opening/set up phase----------------

	// place troop during opening phase - BoardSetUp.StartGame
	public void OpeningPhasePlacement(){
		instructionBox.text = "Select the country you'd like to place a single troop on.";
	}


	// place all troops before moving on - AllocateSoldiers.EndOpeningPhase
	public void PlaceTroops(){
		instructionBox.text = "Deploy all your remaining troops then press 'End Setup' to move onto the battle phase.";
	}

	//----------------battle phase----------------

	// select attacker - Phases.EndSetupPhase
	public void SelectAtkCountry(){
		instructionBox.text = "Select the country you'd like to attack with, then press 'Attack'.";
	}

	// select the defending country - DisplayEditor.AttackingTerritory
	public void SelectDefCountry(string attacker){
		instructionBox.text = "You selected " + attacker + ".\n" +
		"Who are you going to crush?";
	}

	// press battle button - DisplayEditor.BattlingTerritories
	public void PressBattle(){
		instructionBox.text = "Lets fight! - press 'Battle'.";
	}

	// shows battle outcome and keep battling/move onto movement phase - Attack.ATTACK
	public void BattleOutcome(int deadAttackers, int deadDefenders){
		instructionBox.text = "You lost: " + deadAttackers + " army(s).\n" +
		"Enemy lost: " + deadDefenders + " army(s).\n" +
		"Either press battle again to continue fighting,\n" +
		"select another country to attack with,\n" +
		"or press 'End Battle' if you'd like to move on the movement phase.";
	}

	//----------------movement phase----------------

	// move troops, select from country - Phases.EndBattlePhase
	public void SelectMoveButton(){
		instructionBox.text = "You can move a set of troops across connected territories.\n" +
		"Select the 'Move Troops' button then select the country you want to move the troop FROM, followed by " +
		"the country you want to move the troops TO";
	}

	// select country to move troops to - ArmyMovement.MoveToBtn
	public void SelectToCountry(GameObject fromCountry){
		instructionBox.text = "You are moving troops from " + fromCountry.name + ".\n" +
		"Now select the country you'd like to move the troops to.";
	}

	// press +/- to transfer troops, then press end turn - CountrySelector.OnMouseDown & LinkedTerritories.SafePath
	public void MoveTroopButtons(string fromCountry, string toCountry){
		if (fromCountry == toCountry)
			fromCountry = armyMovement.fromCountry.name;

		if(fromCountry == null || toCountry == null)
			return;
		
		instructionBox.text = "Press '+' to transfer a single troop from " + fromCountry + " to " + toCountry +
		"\nPress '-' to move a single troop back from " + toCountry + " to " + fromCountry +
		"\nOnce you're done press 'End Turn'.";
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


	//TODO: add a want to know your odds of winning this fight?
	//		add a prediction mechanic -.... i predict the outcome will be...
	//		make it more of a bot that talks and reacts to the fight make it so the player is taking on the computer
	// 		who controls the comp players etc.

}
