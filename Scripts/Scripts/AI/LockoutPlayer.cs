using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockoutPlayer : MonoBehaviour {

	public  Button plus, minus, attack, battle, moveTo, nextPhase, startGame;

	PlayerTurn playerTurn;

	GameObject scriptHolder;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		playerTurn = scriptHolder.GetComponent<PlayerTurn> ();
	}

	// locks buttons during AI turn and unlocks during player turn - PlayerTurn
	public void ButtonLocks(){
		if (playerTurn.CurrentPlayer()==1) {
			plus.interactable = true;
			minus.interactable = true;
			attack.interactable = true;
			battle.interactable = true;
			moveTo.interactable = true;
			nextPhase.interactable = true;
		} else {
			plus.interactable = false;
			minus.interactable = false;
			attack.interactable = false;
			battle.interactable = false;
			moveTo.interactable = false;
			nextPhase.interactable = false;
		}
	}

	// start button locks after single use - BoardSetUp
	public void LockStartBtn(){
		startGame.interactable = false;
	}

}
