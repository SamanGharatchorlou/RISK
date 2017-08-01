using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopRank : MonoBehaviour {

	public List<int> TroopCountPlayerRanks;
	List<int[]> TroopRankStatic;
	List<int>TroopValues;

	TroopCount troopCount;
	PlayerRank playerRank;
	BoardSetUp boardSetUp;

	int numberOfPlayers;
	bool activeCatagory;

	void Awake () {
		playerRank = this.GetComponent<PlayerRank> ();
		troopCount = this.GetComponent<TroopCount> ();
		boardSetUp = this.GetComponent<BoardSetUp> ();
	}

	void Start(){
		activeCatagory = false;
	}

	// sorts players by troop count
	public void ByTroopCount(){
		numberOfPlayers = boardSetUp.numberOfPlayers;
		// build a list{player number, number of troops owned} from troopCounter dictionary
		TroopRankStatic = new List<int[]> ();
		for (int i = 1; i <= numberOfPlayers; i++) {
			TroopRankStatic.Add (new int[2]);
			TroopRankStatic [i - 1] [0] = i;
			TroopRankStatic [i - 1] [1] = troopCount.troopCounter ["Player" + i];
		}
		// list of only the troop counts (values only)
		TroopValues = new List<int>();
		for (int j = 0; j < numberOfPlayers; j++)
			TroopValues.Add (TroopRankStatic [j] [1]);

		// sort list from highest to lowest
		TroopValues.Sort();
		TroopValues.Reverse ();

		// list of players in ranked order
		TroopCountPlayerRanks = new List<int> ();

		// iterate through the ordered TroopValues
		for (int k = 0; k < numberOfPlayers; k++) {
			// iterate through TroopRankStatic territory counts
			for (int l = 0; l < TroopRankStatic.Count; l++) {
				// build list of players ranked by troop counts
				if (TroopValues [k] == TroopRankStatic [l] [1]) {
					TroopCountPlayerRanks.Add (TroopRankStatic [l] [0]);
					// remove the player just chosen to prevent duplicates
					TroopRankStatic.RemoveAt (l);
					// jump out of loop and move onto next k
					l = numberOfPlayers;
				}
			}
		}
		// Only runs when troop count rank is active
		if (activeCatagory)
			// update troop count display
			playerRank.RankedTroopCount ();
	}

	// finds if troop count is the active catagory - called in ChangeCatagory
	public void UpdateTroopCountDisplay(string catagory){
		if (catagory == "Troop Count")
			activeCatagory = true;
		else
			activeCatagory = false;
	}

}

