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

	GameObject GUI;

	public int numberOfPlayers;

	void Awake(){
		troopCount = this.GetComponent<TroopCount> ();
		territoryCount = this.GetComponent<TerritoryCount> ();
		territoryBonus = this.GetComponent<TerritoryBonus> ();
		continentBonus = this.GetComponent<ContinentBonus> ();
		soldierBonus = this.GetComponent<SoldierBonus> ();

		//TODO: allow player to adjust this at start of game
		// input number of players
		numberOfPlayers = 3;
	}

	public void Start(){
		// build a dictionary with an array of 3 (# territories, # troops, soldier bonus)
		for (int i = 1; i <= numberOfPlayers; i++)
			gameStatistics.Add ("Player" + i, new int[3]);
	}

	// Set up all the game statistics lists
	public void SetUpGameStats(){
		// set up number of territories list
		territoryCount.BuildTerritoryBank ();
		// set up number of troops list
		troopCount.BuildTroopBank();
		// set up territory bonus list
		territoryBonus.BuildTerritoryBonus();
		// set up continent bonus list
		continentBonus.BuildContBonus();
		// set up soldier bonus list
		soldierBonus.UpdateSoldierBonus();
	}


}
