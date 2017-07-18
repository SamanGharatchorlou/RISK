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
	TeamChecker teamChecker;
	PlayerTurn playerTurn;

	public GameObject rankPlacer;
	GameObject cell;
	GameObject scriptHolder;

	Text textBox, cellText, playerHeader, catagory;

	Color teamColour;

	Vector3 cellPos, adjustXPos, adjustYPos;

	public bool terrCountCat, troopCountCat, solBonusCat;
	int NumbOfPlayers, playerNumber;
	string player;

	void Awake () {
		territoryRank = this.GetComponent<TerritoryRank> ();
		territoryCount = this.GetComponent<TerritoryCount> ();
		troopRank = this.GetComponent<TroopRank> ();
		troopCount = this.GetComponent<TroopCount> ();
		soldierBonusRank = this.GetComponent<SoldierBonusRank> ();
		soldierBonus = this.GetComponent<SoldierBonus> ();

		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();
		playerTurn = scriptHolder.GetComponent<PlayerTurn> ();
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

	// displays the category chosen by the player
	public void DisplayCategory(string catagory){
		// runs the relevant code
		if (catagory == "Troop Count")
			RankedTroopCount ();
		else if (catagory == "Territory Count")
			RankedTerrCount ();
		else if (catagory == "Soldier Bonus")
			RankedSoldierBonus ();
	}

	// rank by territroy count
	public void RankedTerrCount(){
		// build table
		for (int i = 1; i <= NumbOfPlayers; i++) {
			playerNumber = territoryRank.TerrCountPlayerRanks [i - 1];
			player = "Player" + playerNumber;
			// set player number, values and colours
			rankTable [i] [1].text = territoryCount.landCounter [player].ToString();
			RankedTableProperties (i);
		}
	}

	// rank by troop count
	public void RankedTroopCount(){
		// build table
		for (int j = 1; j <= NumbOfPlayers; j++) {
			playerNumber = troopRank.TroopCountPlayerRanks [j - 1];
			player = "Player" + playerNumber;
			// set player number, value and colour
			rankTable [j] [1].text = troopCount.troopCounter [player].ToString ();
			RankedTableProperties (j);
		}
	}
		
	// rank by soldier bonus
	public void RankedSoldierBonus(){
		// build table
		for (int k = 1; k <= NumbOfPlayers; k++) {
			playerNumber = soldierBonusRank.SolBonusPlayerRanks [k - 1];
			player = "Player" + playerNumber;
			// set player number, values and colours
			rankTable [k] [1].text = soldierBonus.soldierIncome [player].ToString ();
			RankedTableProperties (k);
		}
	}

	// build various common table properies in function
	void RankedTableProperties(int index){
		// rank player number
		rankTable [index] [0].text = player;
		// set rank colour
		teamColour = teamChecker.GetColour (playerNumber);
		if (playerTurn.CurrentPlayer () != playerNumber)
			teamColour.a = 0.4f;
		rankTable [index] [0].color = teamColour;
		rankTable [index] [1].color = teamColour;
	}

}
