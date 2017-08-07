using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningDeployment : MonoBehaviour {

	public List<Text[]> deploymentTable;
	public Text categoryButton;

	PlayerRank playerRank;
	AllocateSoldiers allocateSoldiers;
	TeamChecker teamChecker;
	BoardSetUp boardSetUp;

	GameObject territories, scriptHolder;

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
		// default category
		categoryButton.text = "Soldiers Left";
		categoryButton.color = Color.black;
		// display the number of soldiers left to deploy
		for (int i = 0; i < numberOfPlayers; i++) {
            deploymentTable[i][0].text = playerRank.DisplayPlayers(i + 1);
            soldiersLeft = allocateSoldiers.soldierBank[i].ToString();
            deploymentTable[i][1].text = soldiersLeft;
		}
        // set player 1 colour
        deploymentTable[0][0].color = teamChecker.GetColour(1);
        deploymentTable[0][1].color = teamChecker.GetColour(1);
	}

	// update the soldier deployment table
	public void UpdateDeploymentTable(int currentPlayer, int soldiers){
        // update number of troops left to deploy
		soldiersLeft = soldiers.ToString ();
        deploymentTable[currentPlayer - 1][1].text = soldiersLeft;
		// update display colour of player placing troops
		ResetColour();
        // deployment table takes player index, team checker takes player number
        deploymentTable[NextPlayer(currentPlayer) - 1][0].color = teamChecker.GetColour(NextPlayer(currentPlayer));
        deploymentTable[NextPlayer(currentPlayer) - 1][1].color = teamChecker.GetColour(NextPlayer(currentPlayer));

	}

	// returns the next player
	int NextPlayer(int currentPlayer){
		if (currentPlayer < boardSetUp.numberOfPlayers)
			return currentPlayer + 1;
		else
			return 1;
	}

	// resets the colour of a row
	public void ResetColour(){
		for (int i = 0; i < boardSetUp.numberOfPlayers; i++) {
            deploymentTable[i][0].color = teamChecker.GetColour(6);
            deploymentTable[i][1].color = teamChecker.GetColour(6);
		}
	}

}
