using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: make attack button another colour once the defending country is being selected incase they change their mind
// TODO: setup -> battle phase all button are unactive even if an owned country has been selected right after phase change

public class ButtonColour : MonoBehaviour {

	Phases phases;
	TeamChecker teamChecker;
	PlayerTurn playerTurn;
	CountryManagement countryManagement;
	Attack attack;

	public Button plusBtn, minusBtn, attackBtn, battleBtn, moveBtn;
	public Button turnBtn, catergoryBtn;

	Image plusColour, minusColour,attackColour, battleColour, turnColour, moveColour, categoryColour;

	GameObject scriptHolder;
	GameObject selectedCountry;

	Color activeGreen, unactiveGrey, permaActiveGrey;

	int armySize;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		phases = scriptHolder.GetComponent<Phases> ();
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();
		playerTurn = scriptHolder.GetComponent<PlayerTurn> ();
		countryManagement = scriptHolder.GetComponent<CountryManagement> ();
		attack = scriptHolder.GetComponent<Attack> ();

		plusColour = plusBtn.GetComponent<Image> ();
		minusColour = minusBtn.GetComponent<Image> ();
		attackColour = attackBtn.GetComponent<Image> ();
		battleColour = battleBtn.GetComponent<Image> ();
		turnColour = turnBtn.GetComponent<Image> ();
		moveColour = moveBtn.GetComponent<Image> ();
		categoryColour = catergoryBtn.GetComponent<Image> ();
	}

	void Start(){
		activeGreen = new Color (0.27f, 0.67f, 0.22f);
		unactiveGrey = new Color (0.7f, 0.7f, 0.7f);
		permaActiveGrey = new Color (0.95f, 0.95f, 0.95f);

		DeactiveateAll ();
		PermaActive ();
	}

	// deactivates all colours
	public void DeactiveateAll(){
		plusColour.color = unactiveGrey;
		minusColour.color = unactiveGrey;
		attackColour.color = unactiveGrey;
		battleColour.color = unactiveGrey;
		moveColour.color = unactiveGrey;
		if (phases.setupPhase)
			turnColour.color = unactiveGrey;
		else
			turnColour.color = permaActiveGrey;
	}

	//TODO: add restart button to this function
	// set default colours to perma active buttons
	public void PermaActive(){
		categoryColour.color = permaActiveGrey;

	}

	// ------------------------- SETUP PHASE  -------------------------

	// +/- buttons - if owned territory is selected - Country Selector
	public void SetupPlusMinusColour(){
		if (phases.setupPhase) {
			selectedCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
			// if selected territory is owned
			if (playerTurn.CurrentPlayer () == teamChecker.GetPlayer (selectedCountry)) {
				plusColour.color = activeGreen;
				minusColour.color = activeGreen;
			} else {
				// if selected territory is not owned
				plusColour.color = unactiveGrey;
				minusColour.color = unactiveGrey;
			}
				
			//TODO: if moving soldier count = 0, only - is active. if moving soldier count = max, only + is active

		}
	}

	// end turn button - if all soldiers have been deployed - Deploy Soldiers
	public void SetupTurnColour(int movingSoldierCount){
		if (movingSoldierCount == 1)
			turnColour.color = permaActiveGrey;
		else
			turnColour.color = unactiveGrey;
	}


	// ------------------------- BATTLE PHASE  -------------------------

	// attack button - if owned territory is selected - countrySelector
	public void BattleAttackColour(){
		if (phases.battlePhase) {
			selectedCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
			if(selectedCountry != null)
				armySize = countryManagement.GetArmySize(selectedCountry.name);
			// if selected territory is owned and has >1 armies
			if (playerTurn.CurrentPlayer () == teamChecker.GetPlayer (selectedCountry) & armySize > 1) {
				attackColour.color = activeGreen;
				battleColour.color = unactiveGrey;
			} else {
				//if selected territory is not owned
				attackColour.color = unactiveGrey;
			}
		}
	}

	// battle button - if enemy territory and attack button have been selected. Deactivates attack button - targetCountry & takeControl
	public void BattleBattleColour(GameObject country){
			// if selected territory is not owned
			if (playerTurn.CurrentPlayer () != teamChecker.GetPlayer (selectedCountry)) {
				battleColour.color = activeGreen;
				attackColour.color = unactiveGrey;
			} else {
				//TODO: code never gets to this point. battle button always green after attacker selection
				//		note: the indenting here is strange...something not right.
				//if selected territory is not owned
				print("not owned");
				battleColour.color = unactiveGrey;
			}
	}

	// +/- buttons - if claim is called and stays active unitl attack has been selected again
	public void BattlePlusMinusColour(bool selectingDefender){
		if (selectingDefender) {
			plusColour.color = unactiveGrey;
			minusColour.color = unactiveGrey;
		} else {
			plusColour.color = activeGreen;
			minusColour.color = activeGreen;
		}
	}
		
	// ------------------------- MOVEMENT PHASE  -------------------------
	// movement phase
	// - after move is presses
	public void MovementMoveColour(){
		if (phases.movementPhase) {
			selectedCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
			// if selected territory is owned
			if (playerTurn.CurrentPlayer () == teamChecker.GetPlayer (selectedCountry))
				moveColour.color = activeGreen;
			else
				moveColour.color = unactiveGrey;
	
		}
	}

	public void MovementRemoveColour(){
		moveColour.color = unactiveGrey;
	}

	public void MovementPlusMinusColour(bool canMove){
		if (canMove) {
			plusColour.color = activeGreen;
			minusColour.color = activeGreen;
			moveColour.color = unactiveGrey;
		} else {
			plusColour.color = unactiveGrey;
			minusColour.color = unactiveGrey;
		}

	}


	

}
