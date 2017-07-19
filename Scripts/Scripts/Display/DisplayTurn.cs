using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTurn : MonoBehaviour {

	TeamChecker teamChecker;

	public Text turnText;

	GameObject scriptHolder;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();
	}

	// displays current players turn
	public void UpdateTurnText(int turn){
		turnText.text = "Player " + turn;
		turnText.color = teamChecker.GetColour (turn);
	}


}
