using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: game buttons change colour when you need to press them - other ones fade out/change colour?

public class GameInstructions : MonoBehaviour {

	public Text instructionBox;

	ArmyMovement armyMovement;
	GameObject scriptHolder;

	string fromCountry, toCountry;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		armyMovement = scriptHolder.GetComponent<ArmyMovement> ();
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
	public void SelectFromCountry(){
		instructionBox.text = "You can move 1 collocetion of troops across connected territories.\n" +
		"Select the territory you'd like to move troops from then select 'Move To'";
	}

	// select country to move troops to - ArmyMovement.MoveToBtn
	public void SelectToCountry(string fromCountry){
		instructionBox.text = "You are moving troops from " + fromCountry + ".\n" +
		"Now select the country you'd like to move the troops to.";
	}

	// press +/- to transfer troops, then press end turn - CountrySelector.OnMouseDown & LinkedTerritories.SafePath
	public void MoveTroopButtons(string fromCountry, string toCountry){
		if (fromCountry == toCountry)
			fromCountry = armyMovement.fromCountry.name;
		instructionBox.text = "Press '+' to transfer a single troop from " + fromCountry + " to " + toCountry +
		"\nPress '-' to move a single troop back from " + toCountry + " to " + fromCountry +
		"\nOnce you're done press 'End Turn'.";
	}

	//----------------error messages----------------

	// error message if countries aren't connected
	public void NotConnect(string fromCountry, string toCountry){
		instructionBox.text = fromCountry + " and " + toCountry + " aren't connected and/or under your control.\n" +
		"Try selecting something else.";
	}

	public void NoSelection(string selection){
		instructionBox.text = "No " + selection + " selected.";
	}


	//TODO: add a want to know your odds of winning this fight?
	//		add a prediction mechanic -.... i predict the outcome will be...
	//		make it more of a bot that talks and reacts to the fight make it so the player is taking on the computer
	// 		who controls the comp players etc.

}
