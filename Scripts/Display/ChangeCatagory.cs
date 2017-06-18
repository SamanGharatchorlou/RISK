using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// player can change the category of the ranking system
public class ChangeCatagory : MonoBehaviour {

	public List <Text> catagories;
	List <string> catagoryLabels;


	PlayerRank playerRank;
	TroopRank troopRank;
	TerritoryRank territoryRank;
	SoldierBonusRank soldierBonusRank;
	Phases phases;

	public GameObject catBoxPlacer;
	GameObject scriptHolder;

	Vector3 catCellPos, catBox1, catBox2, catBox3;
	Vector3 adjustPosX, adjustPosY;

	public bool terrCountCat, troopCountCat, solBonusCat;
	string territoryCount, troopCount, soldierBonus, stored0String, activeCatagory;

	// Use this for initialization
	void Awake() {
		playerRank = this.GetComponent<PlayerRank> ();
		troopRank = this.GetComponent<TroopRank> ();
		territoryRank = this.GetComponent<TerritoryRank> ();
		soldierBonusRank = this.GetComponent<SoldierBonusRank> ();

		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		phases = scriptHolder.GetComponent<Phases> ();
	}

	void Start(){
		// default label order
		catagoryLabels = new List<string>();
		catagoryLabels.Add ("Troop Count"); // 0
		catagoryLabels.Add ("Territory Count"); // 1
		catagoryLabels.Add ("Soldier Bonus"); // 2
	}

	// build the 3 category headers - called in allocateSoldiers
	public void BuildCatHeaders(){
		catagories = new List<Text> ();
		// build a reverse pyramid of 3 text boxes
		for (int x = 0; x < catagoryLabels.Count; x++)
			catagories.Add (playerRank.CreateCell ());

		// set poisition modifiers
		catCellPos = catBoxPlacer.transform.position;
		adjustPosX = new Vector3 (60, 0, 0);
		adjustPosY = new Vector3 (0, 20, 0);
		// set positions relative to the placer object
		catBox1 = catCellPos + adjustPosX;
		catBox2 = catCellPos - adjustPosY;
		catBox3 = catCellPos - adjustPosX;
		// place boxes in correct positions
		catagories [0].transform.SetPositionAndRotation (catBox1, Quaternion.identity);
		catagories [1].transform.SetPositionAndRotation (catBox2, Quaternion.identity);
		catagories [2].transform.SetPositionAndRotation (catBox3, Quaternion.identity);

		// set default labels
		for (int y = 0; y < catagoryLabels.Count; y++)
			catagories [y].text = catagoryLabels [y];
	}

	// when code is run change catagories - Index 1 is the active catagory
	public void RotateCatagory(){
		if (!phases.openingPhase) {
			// store zero index for later use
			stored0String = catagories [0].text;
			// change cat types
			catagories [0].text = catagories [1].text;
			catagories [1].text = catagories [2].text;
			catagories [2].text = stored0String;

			activeCatagory = catagories [1].text;
			// displays the active category
			playerRank.DisplayCategory (activeCatagory);
			// allows the active category display to update - via other rank scripts
			troopRank.UpdateTroopCountDisplay (activeCatagory);
			territoryRank.UpdateTerritoryRankDisplay (activeCatagory);
			soldierBonusRank.UpdateSoldierBonusDisplay (activeCatagory);
		}
	}


}
