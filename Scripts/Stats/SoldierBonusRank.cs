using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBonusRank : MonoBehaviour {

	public List<int> SolBonusPlayerRanks;
	List<int[]> SolBonusRankStatic;
	List<int> SoldierBonus;

	SoldierBonus soldierBonus;
	PlayerRank playerRank;

	int numberOfPlayers;
	bool activeCatagory;

	void Awake () {
		soldierBonus = this.GetComponent<SoldierBonus> ();
		playerRank = this.GetComponent<PlayerRank> ();
	}

	// Ranks players based on territory count -- called in GameStats
	public void BySolBonusRank(int numberOfPlayers){
		// build a list{player number(0), number of territories owned(1)} from landcounter dictionary
		SolBonusRankStatic = new List<int[]> ();
		for (int i = 1; i <= numberOfPlayers; i++) {
			SolBonusRankStatic.Add (new int[2]);
			SolBonusRankStatic [i-1] [0] = i;
			SolBonusRankStatic [i-1] [1] = soldierBonus.soldierIncome["Player"+i];
		}
		// list of only the territory counts (values only)
		SoldierBonus = new List<int>();
		for (int j = 0; j < numberOfPlayers; j++)
			SoldierBonus.Add(SolBonusRankStatic[j][1]);

		// sort list highest to lowest
		SoldierBonus.Sort();
		SoldierBonus.Reverse ();

		// list of players in ranked order
		SolBonusPlayerRanks = new List<int> ();

		// iterate through the ordered TerritoryCounts
		for (int k = 0; k < numberOfPlayers; k++) {
			// iterate through TerrRankStatic territory counts
			for (int l = 0; l < SolBonusRankStatic.Count; l++) {
				// build list of players ranked by territory counts
				if (SoldierBonus [k] == SolBonusRankStatic [l] [1]) {
					SolBonusPlayerRanks.Add (SolBonusRankStatic [l] [0]);
					// remove the player just chosen to prevent duplicates
					SolBonusRankStatic.RemoveAt (l);
					// jump out of loop and move onto next k
					l = numberOfPlayers;
				}
			}
		}
		// only runs the soldier bonus rank is active
		if (activeCatagory)
			// updates soldier bonus rank display
			playerRank.RankedSoldierBonus ();
	}

	// finds if soldier bonus rank is the active catagory - called in ChangeCatagory
	public void UpdateSoldierBonusDisplay(string catagory){
		if (catagory == "Soldier Bonus")
			activeCatagory = true;
		else
			activeCatagory = false;
	}


}
