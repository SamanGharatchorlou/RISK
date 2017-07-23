using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTurn : MonoBehaviour {

	TeamChecker teamChecker;

	public Text turnText;
	public Text TurnNumberText;

	GameObject scriptHolder;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();
	}

	// displays game and player turn numbers
	public void UpdateTurnText(int playerTurnNumber, int gameTurnNumber){
		// displays the players turn number
		turnText.text = "Player " + playerTurnNumber;
		turnText.color = teamChecker.GetColour (playerTurnNumber);
		// displays the games turn number
		TurnNumberText.text = gameTurnNumber.ToString ();
	}


}
