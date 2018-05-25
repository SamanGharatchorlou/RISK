using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryBonus : MonoBehaviour {

	// bonus soldiers received from territory bonues
	public Dictionary<string,int> playerTerrBonus;

	TerritoryCount territoryCount;
	SoldierBonus soldierBonus;
	BoardSetUp boardSetUp;

	GameObject territories;

	int playerTerritoryBonus;

	void Awake(){
		territoryCount = this.GetComponent<TerritoryCount> ();
		soldierBonus = this.GetComponent<SoldierBonus> ();

		territories = GameObject.FindGameObjectWithTag ("Territories");
		boardSetUp = territories.GetComponent<BoardSetUp> ();
	}

	// builds a dictionary of soldier bonuses from the number of territories owned
	public void BuildTerritoryBonus(int numberOfPlayers){

		playerTerrBonus = new Dictionary<string,int> ();

		for (int i = 1; i <= numberOfPlayers; i++) {

			// bonus received
			playerTerritoryBonus = Mathf.FloorToInt(territoryCount.landCounter ["Player" + i] / 3);

			// minimun bonus = 3
			if (playerTerritoryBonus < 3)
				playerTerritoryBonus = 3;

			playerTerrBonus.Add("Player"+i,playerTerritoryBonus);
		}
	}

	// updates territory bonus according to the territory count script
	// - called in territoryCount
	public void UpdateTerritoryBonus(string attackingPlayer, string defendingPlayer){

		playerTerrBonus [attackingPlayer] = Mathf.FloorToInt (territoryCount.landCounter [attackingPlayer] / 3);
		playerTerrBonus [defendingPlayer] = Mathf.FloorToInt (territoryCount.landCounter [defendingPlayer] / 3);

		// minimun bonus = 3
		if (playerTerrBonus [attackingPlayer] < 3)
			playerTerrBonus [attackingPlayer] = 3;

		if (playerTerrBonus [defendingPlayer] < 3)
			playerTerrBonus [defendingPlayer] = 3;

		// update soldier bonus script (territory number + continent bonus)
		soldierBonus.UpdateSoldierBonus (boardSetUp.numberOfPlayers);
	}

}
