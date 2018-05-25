using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// player can change the category of the ranking system
public class ChangeCatagory : MonoBehaviour {

	public Text categoryButton;

	PlayerRank playerRank;
	TroopRank troopRank;
	TerritoryRank territoryRank;
	SoldierBonusRank soldierBonusRank;
	Phases phases;

	public GameObject catBoxPlacer;
	GameObject scriptHolder;

	void Awake() {
		playerRank = this.GetComponent<PlayerRank> ();
		troopRank = this.GetComponent<TroopRank> ();
		territoryRank = this.GetComponent<TerritoryRank> ();
		soldierBonusRank = this.GetComponent<SoldierBonusRank> ();

		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		phases = scriptHolder.GetComponent<Phases> ();
	}

	// when code is run change catagories - Index 1 is the active category
	public void RotateCategory(){

		if (!phases.openingPhase) {

            // changes category 
            switch (categoryButton.text) {
                case "Troop Count":
                    categoryButton.text = "Territory Count";
                    playerRank.RankedTerrCount();
                    break;
                case "Territory Count":
                    categoryButton.text = "Soldier Bonus";
                    playerRank.RankedSoldierBonus();
                    break;
                default: // case for "Soldier Bonus"
                    categoryButton.text = "Troop Count";
                    playerRank.RankedTroopCount();
                    break;
            }

			// allows the active category display to update - via other rank scripts
			troopRank.UpdateTroopCountDisplay (categoryButton.text);
			territoryRank.UpdateTerritoryRankDisplay (categoryButton.text);
			soldierBonusRank.UpdateSoldierBonusDisplay (categoryButton.text);
		}
	}
		

}
