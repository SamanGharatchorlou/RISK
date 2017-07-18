using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI - central controller script
public class AIController : MonoBehaviour {

	SetupPhase setupPhase;
	AttackPhase attackPhase;
	MovementPhase movementPhase;
	Phases phases;
	PlayerTurn playerTurn;

	// Use this for initialization
	void Awake () {
		setupPhase = this.GetComponent<SetupPhase> ();
		attackPhase = this.GetComponent<AttackPhase> ();
		movementPhase = this.GetComponent<MovementPhase> ();
		phases = this.GetComponent<Phases> ();
		playerTurn = this.GetComponent<PlayerTurn> ();
	}

	// to be run after every turn to check if AI player turn
	public void CheckPlayer(){
		// all but player 1
		if(playerTurn.CurrentPlayer() != 1)
			ActivateAIPlayer();
	}
		
	void ActivateAIPlayer(){
		if (!phases.openingPhase) {
			// setup phase
			//setupPhase.PlaceTroops ();
			/*
			phases.EndPhase ();
			// attacking phase
			attackPhase.AIAttackCountry ();
			phases.EndPhase ();
			// movement phase
			movementPhase.AIMoveSoldiers ();
			if(playerTurn.CurrentPlayer() != 1)
				phases.EndPhase();
				*/
		}
	}



	//TODO: add winning mechanic
}
