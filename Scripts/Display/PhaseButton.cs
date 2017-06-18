using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Changes the text on the end phase/turn button
public class PhaseButton : MonoBehaviour {

	Text endPhaseText;

	void Awake(){
		endPhaseText = this.GetComponentInChildren<Text> ();
	}

	public void EndSetupText(){
		endPhaseText.text = "End Setup Phase";
	}

	public void EndBattleText(){
		endPhaseText.text = "End Battle Phase";
	}

	public void EndTurnText(){
		endPhaseText.text = "End Turn";
	}

}
