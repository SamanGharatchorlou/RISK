using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour {

	// master dictionary holding all game stats
	public Dictionary<string, int[]> gameStatistics = new Dictionary<string, int[]>();

	TroopCount troopCount;
	TerritoryCount territoryCount;
	TerritoryBonus territoryBonus;
	ContinentBonus continentBonus;
	SoldierBonus soldierBonus;
	PlayerRank playerRank;
	BoardSetUp boardSetUp;

	GameObject scriptHolder;

	void Awake(){
		troopCount = this.GetComponent<TroopCount> ();
		territoryCount = this.GetComponent<TerritoryCount> ();
		territoryBonus = this.GetComponent<TerritoryBonus> ();
		continentBonus = this.GetComponent<ContinentBonus> ();
		soldierBonus = this.GetComponent<SoldierBonus> ();
		playerRank = this.GetComponent<PlayerRank> ();
		boardSetUp = this.GetComponent<BoardSetUp> ();
	}
		
	// Set up all the game statistics lists
	public void SetUpGameStats(int numberOfPlayers){
		BuildGameStats ();
		// set up number of territories list
		territoryCount.BuildTerritoryBank (numberOfPlayers);
		// set up number of troops list
		troopCount.BuildTroopBank(numberOfPlayers);
		// set up territory bonus list
		territoryBonus.BuildTerritoryBonus(numberOfPlayers);
		// set up continent bonus list
		continentBonus.BuildContBonus(numberOfPlayers);
		//Do i need this line of code?
		continentBonus.UpdateContBonus();
		// set up soldier bonus list
		soldierBonus.UpdateSoldierBonus(numberOfPlayers);
		// build rank table
		playerRank.BuildRankTable(boardSetUp.numberOfPlayers);
	}
		
	void BuildGameStats(){
		// build a dictionary with an array of 3 (# territories, # troops, soldier bonus)
		for (int i = 1; i <= boardSetUp.numberOfPlayers; i++)
			gameStatistics.Add ("Player" + i, new int[3]);
	}

}
