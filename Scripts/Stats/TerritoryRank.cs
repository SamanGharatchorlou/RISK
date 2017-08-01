using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryRank : MonoBehaviour {

	public List<int> TerrCountPlayerRanks;
	List<int[]> TerrRankStatic;
	List<int> TerritoryCounts;

	TerritoryCount territoryCount;
	PlayerRank playerRank;
    
	bool activeCatagory;

	void Awake () {
		playerRank = this.GetComponent<PlayerRank> ();
		territoryCount = this.GetComponent<TerritoryCount> ();
	}

	void Start(){
		activeCatagory = false;
	}
		
	// Ranks players based on territory count -- called in GameStats
	public void ByTerrCount(int numberOfPlayers){
		// build a list{player number(0), number of territories owned(1)} from landcounter dictionary
		TerrRankStatic = new List<int[]> ();
		for (int i = 1; i <= numberOfPlayers; i++) {
			TerrRankStatic.Add (new int[2]);
			TerrRankStatic [i-1] [0] = i;
			TerrRankStatic [i-1] [1] = territoryCount.landCounter["Player"+i];
		}
		// list of only the territory counts (values only)
		TerritoryCounts = new List<int>();
		for (int j = 0; j < numberOfPlayers; j++)
			TerritoryCounts.Add(TerrRankStatic[j][1]);
		
		// sort list highest to lowest
		TerritoryCounts.Sort();
		TerritoryCounts.Reverse ();

		// list of player numbers in ranked order
		TerrCountPlayerRanks = new List<int> ();

		// iterate through the ordered TerritoryCounts
		for (int k = 0; k < numberOfPlayers; k++) {
			// iterate through TerrRankStatic territory counts
			for (int l = 0; l < TerrRankStatic.Count; l++) {
				// build list of players ranked by territory counts
				if (TerritoryCounts [k] == TerrRankStatic [l] [1]) {
					TerrCountPlayerRanks.Add (TerrRankStatic [l] [0]);
					// remove the player just chosen to prevent duplicates
					TerrRankStatic.RemoveAt (l);
					// jump out of loop and move onto next k
					l = numberOfPlayers;
				}
			}
		}
		// Only runs when territory rank is active
		if (activeCatagory)
			// update territory rank display
			playerRank.RankedTerrCount ();
	}

	// finds if territory count is the active catagory - called in ChangeCatagory
	public void UpdateTerritoryRankDisplay(string catagory){
		if (catagory == "Territory Count")
			activeCatagory = true;
		else
			activeCatagory = false;
	}
			
}
