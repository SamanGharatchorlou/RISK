using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveBonus : MonoBehaviour {

	TeamChecker teamChecker;
	PlayerTurn playerTurn;

	GameObject scriptHolder;

	public Text bonusSoldierText;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();
		playerTurn = scriptHolder.GetComponent<PlayerTurn> ();
	}

	public void SoldierBonusDisplay(int movingSoldierCount){
		bonusSoldierText.text = "Soldier Bonus: " + movingSoldierCount;
		bonusSoldierText.color = teamChecker.GetColour (playerTurn.CurrentPlayer ());
	}

	public void RemoveSoliderDisplayer(){
		bonusSoldierText.text = "";
	}

}
