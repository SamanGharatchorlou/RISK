using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningDeployment : MonoBehaviour {

	List<Text[]> deploymentTable;

	PlayerRank playerRank;
	AllocateSoldiers allocateSoldiers;
	TeamChecker teamChecker;
	BoardSetUp boardSetUp;

	GameObject territories, scriptHolder;

	//int nextPlayer;

	string soldiersLeft;

	void Awake(){
		territories = GameObject.FindGameObjectWithTag ("Territories");
		playerRank = territories.GetComponent<PlayerRank> ();
		boardSetUp = territories.GetComponent<BoardSetUp> ();

		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		allocateSoldiers = scriptHolder.GetComponent<AllocateSoldiers> ();
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();
	}

	// build the soldier deployment table for openingPhase
	public void BuildDeployementTable(int numberOfPlayers){
		deploymentTable = playerRank.rankTable;

		// set header
		deploymentTable[0][1].text="Soldiers Left";

		for (int i = 1; i <= numberOfPlayers; i++) {
			deploymentTable [i] [0].text = "Player" + i;
			// set player 1 colour rest goes into update
			//deploymentTable [i] [0].color = teamChecker.GetColour (i);
			soldiersLeft = allocateSoldiers.soldierBank [i - 1].ToString ();
			deploymentTable [i] [1].text = soldiersLeft;
		}
		// set player 1 colour -> green
		deploymentTable [1] [0].color = teamChecker.GetColour (1);
		deploymentTable [1] [1].color = teamChecker.GetColour (1);

	}

	// update the soldier deployment table
	public void UpdateDeploymentTable(int currentPlayer, int soldiers){
		// update number of troops left to deploy
		soldiersLeft = soldiers.ToString ();
		deploymentTable [currentPlayer] [1].text = soldiersLeft;
		// update display colour of player placing troops
		deploymentTable [NextPlayer(currentPlayer)] [0].color = teamChecker.GetColour (NextPlayer(currentPlayer));
		deploymentTable [NextPlayer(currentPlayer)] [1].color = teamChecker.GetColour (NextPlayer(currentPlayer));
		// reset previous player colour
		ResetColour(currentPlayer);
	}

	// returns the next player
	int NextPlayer(int currentPlayer){
		if (currentPlayer < boardSetUp.numberOfPlayers)
			return currentPlayer + 1;
		else
			return 1;
	}

	// resets the colour of a row
	void ResetColour(int rowNumber){
		deploymentTable [rowNumber] [0].color = teamChecker.GetColour (6);
		deploymentTable [rowNumber] [1].color = teamChecker.GetColour (6);
	}

	// resets rank table for setup phase
	public void ResetRankTable(int currentPlayer){
		deploymentTable [0] [1].text = "";
		// remove player 1 colour from table
		ResetColour (1);
	}



}
