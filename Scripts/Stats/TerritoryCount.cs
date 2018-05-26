using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryCount : MonoBehaviour {

	// dictionary holds the number of territories owned by each player
	public Dictionary<string,int> landCounter;

	Attack attack;
	BoardSetUp boardSetUp;
	TerritoryBonus territoryBonus;
	TerritoryRank territoryRank;

	GameObject scriptHolder;

	int numbOfPlayers;
	string attackingPlayer, defendingPlayer;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		attack = scriptHolder.GetComponent<Attack> ();

		boardSetUp = this.GetComponent<BoardSetUp> ();
		territoryBonus = this.GetComponent<TerritoryBonus> ();
		territoryRank = this.GetComponent<TerritoryRank> ();
	}

	// builds a dictionary of the number of territories owned
	public void BuildTerritoryBank(int numberOfPlayers) {
		landCounter = new Dictionary<string, int> ();
		numbOfPlayers = numberOfPlayers;
		// re runs PlayerLandBank code as previous table is modified by BoardSetUp.RandomPlayer()
		boardSetUp.PlayerLandBank (numbOfPlayers);
		for (int i = 0; i < numbOfPlayers; i++)
			landCounter.Add ("Player" + (boardSetUp.landBank [i] [0] + 1), boardSetUp.landBank [i] [1]);
		// build rank system
		territoryRank.ByTerrCount (numbOfPlayers);
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
		territoryRank.ByTerrCount(numbOfPlayers);
	}
		

}
