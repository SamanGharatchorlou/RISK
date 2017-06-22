using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// creates the ranking table for various stats
public class PlayerRank : MonoBehaviour {

	public List <Text[]> rankTable;

	TerritoryRank territoryRank;
	TerritoryCount territoryCount;
	TroopRank troopRank;
	TroopCount troopCount;
	SoldierBonusRank soldierBonusRank;
	SoldierBonus soldierBonus;

	public GameObject rankPlacer;
	GameObject cell;

	Text textBox, cellText, playerHeader, catagory;

	Vector3 cellPos, adjustXPos, adjustYPos;

	public bool terrCountCat, troopCountCat, solBonusCat;
	int NumbOfPlayers;
	string playerNumber;

	void Awake () {
		territoryRank = this.GetComponent<TerritoryRank> ();
		territoryCount = this.GetComponent<TerritoryCount> ();
		troopRank = this.GetComponent<TroopRank> ();
		troopCount = this.GetComponent<TroopCount> ();
		soldierBonusRank = this.GetComponent<SoldierBonusRank> ();
		soldierBonus = this.GetComponent<SoldierBonus> ();
	}

	// build the player stats table (incl headers)
	public void BuildRankTable(int numberOfPlayers){
		NumbOfPlayers = numberOfPlayers;
		rankTable = new List<Text[]> ();
		// build a 2 x numberOfPlayers table of text boxes (cells)
		for (int y = 0; y <= NumbOfPlayers; y++)
			rankTable.Add (new Text[]{ CreateCell(), CreateCell() });

		// set x and y distances between cells
		cellPos = rankPlacer.transform.position;
		adjustXPos = new Vector3 (120, 0, 0);
		adjustYPos = new Vector3 (0, 20, 0);

		// set cell positions
		for (int i = 0; i < rankTable.Count; i++) {
			// set column 1 position
			cellPos -= adjustXPos;
			cellPos -= adjustYPos;
			textBox = rankTable [i] [0];
			textBox.transform.SetPositionAndRotation (cellPos,Quaternion.identity);
			// set column 2 position
			cellPos += adjustXPos;
			textBox = rankTable [i] [1];
			textBox.transform.SetPositionAndRotation (cellPos, Quaternion.identity);
		}
		// set headers
		playerHeader = rankTable[0][0];
		catagory = rankTable [0] [1];
		playerHeader.text = "Player";
		catagory.text = "";
	}

	// create a single cell
	public Text CreateCell(){
		cell = GameObject.Instantiate (Resources.Load ("Text Box"), rankPlacer.transform) as GameObject;
		cellText = cell.GetComponent<Text> ();
		return cellText;
	}

	public void DisplayCategory(string catagory){
		// runs the relevant code
		if (catagory == "Troop Count")
			RankedTroopCount ();
		else if (catagory == "Territory Count")
			RankedTerrCount ();
		else if (catagory == "Soldier Bonus")
			RankedSoldierBonus ();
	}

	// rank by troop count
	public void RankedTroopCount(){
		// build table
		for (int j = 1; j <= NumbOfPlayers; j++) {
			playerNumber = "Player" + troopRank.TroopCountPlayerRanks [j - 1];
			// set player number
			rankTable [j] [0].text = playerNumber;
			// set value
			rankTable [j] [1].text = troopCount.troopCounter [playerNumber].ToString ();
		}
	}

	// rank by territroy count
	public void RankedTerrCount(){
		// build table
		for (int i = 1; i <= NumbOfPlayers; i++) {
			playerNumber = "Player" + territoryRank.TerrCountPlayerRanks [i - 1];
			// set player number
			rankTable [i] [0].text = playerNumber;
			// set value
			rankTable [i] [1].text = territoryCount.landCounter [playerNumber].ToString();
		}
	}
		
	// rank by soldier bonus
	public void RankedSoldierBonus(){
		// build table
		for (int k = 1; k <= NumbOfPlayers; k++) {
			playerNumber = "Player" + soldierBonusRank.SolBonusPlayerRanks [k - 1];
			// set player number
			rankTable [k] [0].text = playerNumber;
			// set value
			rankTable [k] [1].text = soldierBonus.soldierIncome [playerNumber].ToString ();
		}
	}

}
