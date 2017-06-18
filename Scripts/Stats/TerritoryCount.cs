using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryCount : MonoBehaviour {

	// dictionary holds the number of territories owned by each player
	public Dictionary<string,int> landCounter = new Dictionary<string, int> ();

	Attack attack;
	BoardSetUp boardSetUp;
	TerritoryBonus territoryBonus;
	GameStats gameStats;
	TerritoryRank territoryRank;

	GameObject scriptHolder;

	string attackingPlayer, defendingPlayer;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		attack = scriptHolder.GetComponent<Attack> ();

		boardSetUp = this.GetComponent<BoardSetUp> ();
		gameStats = this.GetComponent<GameStats> ();
		territoryBonus = this.GetComponent<TerritoryBonus> ();
		territoryRank = this.GetComponent<TerritoryRank> ();
	}

	void Start(){
		// Number of territories owned by each player
		for (int i = 1; i <= gameStats.numberOfPlayers; i++)
			landCounter.Add ("Player" + i, 0);
	}

	// builds a dictionary of the number of territories owned
	public void BuildTerritoryBank() {
		// re runs PlayerLandBank code as previous table is modified by BoardSetUp.RandomPlayer()
		boardSetUp.PlayerLandBank (gameStats.numberOfPlayers);
		for (int i = 0; i < gameStats.numberOfPlayers; i++) {
			landCounter ["Player" + (boardSetUp.landBank [i] [0] + 1)] = boardSetUp.landBank [i] [1];
		}
		// build rank system
		territoryRank.ByTerrCount ();
	}

	// updates territory bank - called in TakeControl
	public void UpdateTerritoryBank(GameObject attacker, GameObject defender){
		attackingPlayer = "Player" + attack.attackingPlayer;
		defendingPlayer = "Player" + attack.defendingPlayer;

		landCounter [attackingPlayer] += 1;
		landCounter [defendingPlayer] -= 1;

		// update territory bonus dictionary
		territoryBonus.UpdateTerritoryBonus (attackingPlayer, defendingPlayer);

		// rebuild territoryRank list when an updated has occured
		territoryRank.ByTerrCount();
	}
		

}
