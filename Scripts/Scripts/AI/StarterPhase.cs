using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterPhase : MonoBehaviour {

	PlayerTurn playerTurn;
	SetupPhase setupPhase;
	AllocateSoldiers allocateSoldiers;

	GameObject selectedCountry;

	void Awake () {
		playerTurn = this.GetComponent<PlayerTurn> ();
		setupPhase = this.GetComponent<SetupPhase> ();
		allocateSoldiers = this.GetComponent<AllocateSoldiers> ();
	}

	// deploy troop during opening phase - called in PlayerTurn.NextPlayer
	public void AIDeployTroop(){
		// doesnt activate for player 1
		if (playerTurn.CurrentPlayer () != 1) {
			setupPhase.SelectFrontLineCountry ();
			selectedCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
			allocateSoldiers.DropSoldier (selectedCountry);
		}
	}
	

}
