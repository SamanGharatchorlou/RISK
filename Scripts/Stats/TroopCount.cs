using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopCount : MonoBehaviour {

	// dictionary holds the number of territories owned by each player
	public Dictionary<string,int> troopCounter = new Dictionary<string, int> ();

	BoardSetUp boardSetUp;
	GameStats gameStats;
	TeamChecker teamChecker;
	TroopRank troopRank;

	GameObject scriptHolder;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();

		boardSetUp = this.GetComponent<BoardSetUp> ();
		gameStats = this.GetComponent<GameStats> ();
		troopRank = this.GetComponent<TroopRank> ();
	}

	void Start(){
		// Number of troops owned by each player
		for (int i = 1; i <= gameStats.numberOfPlayers; i++)
			troopCounter.Add ("Player" + i, 0);
	}

	// builds a dictionary of the number of troops owned
	public void BuildTroopBank() {
		boardSetUp.PlayerLandBank (gameStats.numberOfPlayers);
		for (int i = 0; i < gameStats.numberOfPlayers; i++)
			troopCounter ["Player" + (boardSetUp.landBank [i] [0] + 1)] = boardSetUp.landBank [i] [1];
		// build rank system
		troopRank.ByTroopCount();
	}

	// update troop numbers - AddSoldier.PlaceSoldier
	public void UpdateTroopBank(GameObject country, int soldiers){
		string player = "Player" + teamChecker.GetPlayer (country);
		troopCounter [player] += soldiers;

		// Update troop count list when update has occured
		troopRank.ByTroopCount();
	}

	// update troop numbers v2 takes a different input for remove funtions - Remove() & RemoveDead()
	public void UpdateTroopBankV2(int playerNumber, int soldiers){
		string player = "Player" + playerNumber;
		troopCounter [player] += soldiers;

		// Update troop count list when update has occured
		troopRank.ByTroopCount();
	}

}
