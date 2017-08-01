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
	GameInstructions gameInstructions;

	public Button plusBtn, minusBtn, attackBtn, battleBtn, moveBtn, turnBtn, catergoryBtn, startBtn, scroll;
    public InputField numberOfEnemies;

	Image plusColour, minusColour,attackColour, battleColour, turnColour, moveColour, categoryColour;

	Text attackText, moveText;

	GameObject scriptHolder;
	GameObject selectedCountry;

	Color activeGreen, incorrectRed, unactiveGrey, permaActiveGrey;

	int armySize;

	void Awake(){
		gameInstructions = this.GetComponent<GameInstructions> ();

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
		moveText = moveBtn.GetComponentInChildren<Text> ();
	}

	void Start(){
		activeGreen = new Color (0.27f, 0.67f, 0.22f);
		incorrectRed = new Color (0.78f, 0.3f, 0.1f);
		unactiveGrey = new Color (0.7f, 0.7f, 0.7f);
		permaActiveGrey = new Color (0.95f, 0.95f, 0.95f);
		// deactivate all button colours except change category
		DeactiveateAll ();
		categoryColour.color = permaActiveGrey;

        // set up starting buttons and input field
        LockButton("start");
        numberOfEnemies.interactable = false;
	}

	// ------------------------- GENERAL  -------------------------

	// deactivates all button colours
	public void DeactiveateAll(){
		plusColour.color = unactiveGrey;
		minusColour.color = unactiveGrey;
		attackColour.color = unactiveGrey;
		battleColour.color = unactiveGrey;
		moveColour.color = unactiveGrey;
		turnColour.color = unactiveGrey;
	}

	// locks a given button
	//plusBtn, minusBtn, attackBtn, battleBtn, moveBtn, turnBtn, catergoryBtn, startBtn
	public void LockButton(string button){
        switch (button) {
            case "category":
                catergoryBtn.interactable = false;
                break;
            case "start":
                startBtn.interactable = false;
                break;
            case "plus":
                plusBtn.interactable = false;
                break;
            case "minus":
                minusBtn.interactable = false;
                break;
            case "attack":
                attackBtn.interactable = false;
                break;
            case "battle":
                battleBtn.interactable = false;
                break;
            case "move":
                moveBtn.interactable = false;
                break;
            case "turn":
                turnBtn.interactable = false;
                break;
            case "input":
                numberOfEnemies.interactable = false;
                break;
            case "instructions":
                scroll.interactable = false;
                break;
            default:
                print("Nothing locked");
                break;
        }
	}

	// locks all buttons
	public void LockAllButtons(){
		plusBtn.interactable = false;
		minusBtn.interactable = false;
		attackBtn.interactable = false;
		battleBtn.interactable = false;
		moveBtn.interactable = false;
		turnBtn.interactable = false;
	}

	// unlocks a given button
	public void UnlockButton(string button){
        switch (button) {
            case "category":
                catergoryBtn.interactable = true;
                break;
            case "start":
                startBtn.interactable = true;
                break;
            case "plus":
                plusBtn.interactable = true;
                break;
            case "minus":
                minusBtn.interactable = true;
                break;
            case "attack":
                attackBtn.interactable = true;
                break;
            case "battle":
                battleBtn.interactable = true;
                break;
            case "move":
                moveBtn.interactable = true;
                break;
            case "turn":
                turnBtn.interactable = true;
                break;
            case "input":
                numberOfEnemies.interactable = true;
                break;
            default:
                print("Nothing unlocked");
                break;
        }
	}
		
	// ------------------------- SETUP PHASE  -------------------------

	// +/- buttons - if owned territory is selected - Country Selector
	public void SetupPlusMinusColour(){
		if (phases.setupPhase) {
			selectedCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
			// if selected territory is owned and various troop count conditions are met
			if (teamChecker.UnderControl (selectedCountry)) {
				// + button
				if (deploySoldiers.soldiersLeft > 0) {
					plusColour.color = activeGreen;
					UnlockButton ("plus");
				} else {
					plusColour.color = unactiveGrey;
					LockButton ("plus");
				}
				// - button
				foreach (Transform soldier in selectedCountry.transform) {
					if (soldier.gameObject.tag == "DeployedSoldier") {
						minusColour.color = activeGreen;
						UnlockButton ("minus");
						break;
					} else {
						minusColour.color = unactiveGrey;
						LockButton ("minus");
					}
					
				}
			} else {
				// if selected territory is not owned
				plusColour.color = unactiveGrey;
				minusColour.color = unactiveGrey;
				LockButton ("plus");
				LockButton ("minus");
			}
		}
	}

	// end turn button - if all soldiers have been deployed - Deploy Soldiers
	public void SetupTurnColour(int soldiersLeft){
		if (soldiersLeft == 1) {
			turnColour.color = permaActiveGrey;
			UnlockButton ("turn");
		} else {
			turnColour.color = unactiveGrey;
			LockButton ("turn");
		}
	}

    // ------------------------- BATTLE PHASE  -------------------------

    public void BattleStartPhase() {
        UnlockButton("turn");
        turnColour.color = permaActiveGrey;
        UnlockButton("attack");
        attackColour.color = activeGreen;
    }

    // attack button - if owned territory is selected - countrySelector
    public void BattleAttackColour(bool selectingDefender){
		selectedCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
		if (selectedCountry != null)
			armySize = countryManagement.GetArmySize (selectedCountry.name);
		
		// if defender is selected lock attack button & adjust text accordingly
		if (selectingDefender)
			BattleDeselectAttacker ();
		// if attacker is selected with army > 1 unlock attack button
		else if (teamChecker.UnderControl (selectedCountry) & armySize > 1)
			BattleReselectAttacker ();
		else {
			//if selected territory is not owned
			attackColour.color = incorrectRed;
			LockButton ("attack");
		}
	}

	// attack button - player can press attack to deselect attacker
	void BattleDeselectAttacker(){
		attackText.text = "Deselect\nAttacker";
		attackText.fontSize = 10;
		attackColour.color = permaActiveGrey;
		UnlockButton ("attack");
	}

	// attack button - player can press attack to re-select attacker
	void BattleReselectAttacker(){
		attackText.text = "Attack";
		attackText.fontSize = 14;
		attackColour.color = activeGreen;
		battleColour.color = unactiveGrey;
		UnlockButton ("attack");
		LockButton ("battle");
	}

	// battle button - if enemy territory and attack button have been selected. Deactivates attack button - targetCountry & takeControl
	public void BattleBattleColour(GameObject attackingCountry, GameObject defendingCountry){
		if (attackingCountry == null || defendingCountry == null) {
			battleColour.color = unactiveGrey;
			LockButton ("battle");
		}
		// if selected territory is not owned and a neighbour
		else if (!teamChecker.UnderControl (defendingCountry) & attack.CanAttack (attackingCountry, defendingCountry)) {
			battleColour.color = activeGreen;
			UnlockButton ("battle");
		} else {
			battleColour.color = unactiveGrey;
			LockButton ("battle");
		}

	}

	// +/- buttons - if claim is called and stays active unitl attack has been selected again - targetCountry & takeControl
	public void BattlePlusMinusColour(bool selectingDefender){
		selectedCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
		if (selectingDefender) {
			plusColour.color = unactiveGrey;
			minusColour.color = unactiveGrey;
			LockButton ("plus");
			LockButton ("minus");
		} else {
			plusColour.color = unactiveGrey;
			minusColour.color = activeGreen;
			LockButton ("plus");
			UnlockButton ("minus");
		}
	}

	// +/- buttons -  once takeControl has been called - SoldierTransfer
	public void BattlePlusMinusColour2(GameObject fromCountry, GameObject toCountry, int attackerNumbers){
		// set + conditions
		if (countryManagement.GetArmySize (fromCountry.name) > 1) {
			plusColour.color = activeGreen;
			UnlockButton ("plus");
		} else {
			plusColour.color = unactiveGrey;
			LockButton ("plus");
		}
		// set - conditions
		if (countryManagement.GetArmySize (toCountry.name) > attackerNumbers) {
			minusColour.color = activeGreen;
			UnlockButton ("minus");
		} else {
			minusColour.color = unactiveGrey;
			LockButton ("minus");
		}
	}




    // ------------------------- MOVEMENT PHASE  -------------------------

    public void MovementStartPhase(){
        UnlockButton("turn");
        turnColour.color = permaActiveGrey;
        UnlockButton("move");
        moveColour.color = permaActiveGrey;
	}

	// from country selection process
	public void MovementSelectFromCountry(GameObject fromCountry){
		// correct fromCountry selected
		if (teamChecker.UnderControl (fromCountry) & countryManagement.GetArmySize (fromCountry.name) > 1) {
			moveText.text = "Select as\n'From Country'";
			moveColour.color = activeGreen;
			UnlockButton ("move");
			gameInstructions.SelectFromMoveButton (moveText.text);
		} 
		// incorrect fromCountry selected
		else {
			moveColour.color = incorrectRed;
			LockButton ("move");
		}
	}

	// to country selection process
	public void MovementSelectToCountry(GameObject toCountry, string selection){
		// correct toCountry selected
		if (selection == "correct") {
			moveText.text = "Select as\n'To Country'";
			moveColour.color = activeGreen;
			gameInstructions.SelectToMoveButton (moveText.text);
			UnlockButton ("move");
		}
		// incorrect toCountry selected
		else {
			moveColour.color = incorrectRed;
			LockButton ("move");
		}
	}
		
	// Move Button - Selects fromCountry and moves selection to toCountry
	public void MovementFromCountrySelected(){
		moveColour.color = unactiveGrey;
		moveText.text = "Select a\n'To Country'";
		gameInstructions.SelectToCountry ();
		LockButton ("move");
	}
	
	// Move Button - confirms from and to country selections
	public void MovementToCountrySelected(){
		moveColour.color = unactiveGrey;
		moveText.text = "Selection\nConfirmed";

		LockButton ("move");
		UnlockButton ("plus");
		plusColour.color = activeGreen;
		UnlockButton ("minus");
		minusColour.color = activeGreen;
	}

	// activates/deactivates +/- buttons
	public void MovementPlusMinusColour(GameObject fromCountry, GameObject toCountry){
		// set + conditions
		if (countryManagement.GetArmySize (fromCountry.name) > 1) {
			plusColour.color = activeGreen;
			UnlockButton ("plus");
		} else {
			plusColour.color = unactiveGrey;
			LockButton ("plus");
		}
		// set - conditions
		if (countryManagement.GetArmySize (toCountry.name) > 1) {
			minusColour.color = activeGreen;
			UnlockButton ("minus");
		} else {
			minusColour.color = unactiveGrey;
			LockButton ("minus");
		}
	}

	// restores movement button to default for start of next turm
	public void MovementDefault(){
		moveText.text = "Move Troops";
		moveColour.color = unactiveGrey;
	}


	

}
