using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour {

	// master dictionary holding all game stats
	public Dictionary<string, int[]> gameStatistics;

	TroopCount troopCount;
	TerritoryCount territoryCount;
	TerritoryBonus territoryBonus;
	ContinentBonus continentBonus;
	SoldierBonus soldierBonus;
	PlayerRank playerRank;
	BoardSetUp boardSetUp;

	void Awake(){
		troopCount = this.GetComponent<TroopCount> ();
		territoryCount = this.GetComponent<TerritoryCount> ();
		territoryBonus = this.GetComponent<TerritoryBonus> ();
		continentBonus = this.GetComponent<ContinentBonus> ();
		soldierBonus = this.GetComponent<SoldierBonus> ();
		playerRank = this.GetComponent<PlayerRank> ();
		boardSetUp = this.GetComponent<BoardSetUp> ();
	}

	void Start(){
		gameStatistics = new Dictionary<string, int[]>();
	}
		
	// Set up all the game statistics lists
	public void SetUpGameStats(int numberOfPlayers){

		// set up number of territories list
		territoryCount.BuildTerritoryBank (numberOfPlayers);

		// set up number of troops list
		troopCount.BuildTroopBank(numberOfPlayers);

		// set up territory bonus list
		territoryBonus.BuildTerritoryBonus(numberOfPlayers);

		// set up continent bonus list
		continentBonus.BuildContBonus(numberOfPlayers);

		//not required but just in case lets update the stats
		continentBonus.UpdateContBonus();

		// set up soldier bonus list
		soldierBonus.UpdateSoldierBonus(numberOfPlayers);

		// build rank table
		playerRank.BuildRankTable(boardSetUp.numberOfPlayers);
	}

}
