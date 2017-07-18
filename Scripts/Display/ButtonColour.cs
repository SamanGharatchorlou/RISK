using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColour : MonoBehaviour {

	Phases phases;
	TeamChecker teamChecker;
	CountryManagement countryManagement;
	Attack attack;
	DeploySoldiers deploySoldiers;

	public Button plusBtn, minusBtn, attackBtn, battleBtn, moveBtn, turnBtn, catergoryBtn;

	Image plusColour, minusColour,attackColour, battleColour, turnColour, moveColour, categoryColour;

	Text attackText;

	GameObject scriptHolder;
	GameObject selectedCountry;

	Color activeGreen, unactiveGrey, permaActiveGrey;

	int armySize;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		phases = scriptHolder.GetComponent<Phases> ();
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();
		countryManagement = scriptHolder.GetComponent<CountryManagement> ();
		attack = scriptHolder.GetComponent<Attack> ();
		deploySoldiers = scriptHolder.GetComponent<DeploySoldiers> ();

		plusColour = plusBtn.GetComponent<Image> ();
		minusColour = minusBtn.GetComponent<Image> ();
		attackColour = attackBtn.GetComponent<Image> ();
		battleColour = battleBtn.GetComponent<Image> ();
		turnColour = turnBtn.GetComponent<Image> ();
		moveColour = moveBtn.GetComponent<Image> ();
		categoryColour = catergoryBtn.GetComponent<Image> ();

		attackText = attackBtn.GetComponentInChildren<Text> ();
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
		
	// set default colours to perma active buttons
	public void PermaActive(){
		categoryColour.color = permaActiveGrey;
	}
		
	// ------------------------- SETUP PHASE  -------------------------

	// +/- buttons - if owned territory is selected - Country Selector
	public void SetupPlusMinusColour(){
		if (phases.setupPhase) {
			selectedCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
			// if selected territory is owned and various troop count conditions are met
			if (teamChecker.UnderControl (selectedCountry)) {
				// + button
				if (deploySoldiers.soldiersLeft > 0)
					plusColour.color = activeGreen;
				else
					plusColour.color = unactiveGrey;
				// - button
				foreach (Transform soldier in selectedCountry.transform) {
					if (soldier.gameObject.tag == "DeployedSoldier") {
						minusColour.color = activeGreen;
						break;
					} else
						minusColour.color = unactiveGrey;
				}
			} else {
				// if selected territory is not owned
				plusColour.color = unactiveGrey;
				minusColour.color = unactiveGrey;
			}
		}
	}

	// end turn button - if all soldiers have been deployed - Deploy Soldiers
	public void SetupTurnColour(int soldiersLeft){
		if (soldiersLeft == 1)
			turnColour.color = permaActiveGrey;
		else
			turnColour.color = unactiveGrey;
	}

	// ------------------------- BATTLE PHASE  -------------------------

	// attack button - if owned territory is selected - countrySelector
	public void BattleAttackColour(bool selectingDefender){
		if (phases.battlePhase) {
			selectedCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
			if(selectedCountry != null)
				armySize = countryManagement.GetArmySize(selectedCountry.name);
			// if selected territory is owned and has >1 armies
			if (selectingDefender)
				BattleDeselectAttacker ();
			else if (teamChecker.UnderControl (selectedCountry) & armySize > 1)
				BattleReselectAttacker ();
			else
				//if selected territory is not owned
				attackColour.color = unactiveGrey;
		}
	}

	// attack button - player can press attack to deselect attacker
	void BattleDeselectAttacker(){
		attackText.text = "Deselect\nAttacker";
		attackText.fontSize = 10;
		attackColour.color = permaActiveGrey;
	}

	// attack button - player can press attack to re-select attacker
	void BattleReselectAttacker(){
		attackText.text = "Attack";
		attackText.fontSize = 14;
		attackColour.color = activeGreen;
		battleColour.color = unactiveGrey;
	}

	// battle button - if enemy territory and attack button have been selected. Deactivates attack button - targetCountry & takeControl
	public void BattleBattleColour(GameObject attackingCountry, GameObject defendingCountry){
		if(attackingCountry == null || defendingCountry == null)
			battleColour.color = unactiveGrey;
		// if selected territory is not owned and a neighbour
		else if(!teamChecker.UnderControl(defendingCountry) & attack.CanAttack(attackingCountry,defendingCountry))
			battleColour.color = activeGreen;
		else
			battleColour.color = unactiveGrey;

	}

	// +/- buttons - if claim is called and stays active unitl attack has been selected again - targetCountry & takeControl
	public void BattlePlusMinusColour(bool selectingDefender){
		selectedCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
		if (selectingDefender) {
			plusColour.color = unactiveGrey;
			minusColour.color = unactiveGrey;
		} else {
			plusColour.color = unactiveGrey;
			minusColour.color = activeGreen;
		}
	}

	// +/- buttons - affect after takeControl has been called - SoldierTransfer
	public void BattlePlusMinusColour2(GameObject fromCountry, GameObject toCountry, int attackerNumbers){
		// set + conditions
		if (countryManagement.GetArmySize (fromCountry.name) > 1)
			plusColour.color = activeGreen;
		else
			plusColour.color = unactiveGrey;
		// set - conditions
		if (countryManagement.GetArmySize (toCountry.name) > attackerNumbers)
			minusColour.color = activeGreen;
		else
			minusColour.color = unactiveGrey;
	}
		
	// ------------------------- MOVEMENT PHASE  -------------------------

	// - after move is presses
	public void MovementMoveColour(string status){
		if (status == "active")
			moveColour.color = activeGreen;
		else if(status == "unactive")
			moveColour.color = unactiveGrey;
	}

	public void MovementPlusMinusColour(GameObject fromCountry, GameObject toCountry){
		// set + conditions
		if (countryManagement.GetArmySize (fromCountry.name) > 1)
			plusColour.color = activeGreen;
		else
			plusColour.color = unactiveGrey;
		// set - conditions
		if (countryManagement.GetArmySize (toCountry.name) > 1)
			minusColour.color = activeGreen;
		else
			minusColour.color = unactiveGrey;
	

	}


	

}
