using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBonus : MonoBehaviour {

	// total bonus recevied from continent and territory bonuses
	public Dictionary<string,int> soldierIncome;

	TerritoryBonus territoryBonus;
	ContinentBonus continentBonus;
	SoldierBonusRank soldierBonusRank;
    
	string player;

	void Awake(){
		territoryBonus = this.GetComponent<TerritoryBonus> ();
		continentBonus = this.GetComponent<ContinentBonus> ();
		soldierBonusRank = this.GetComponent<SoldierBonusRank> ();
	}

	void Start(){
		soldierIncome = new Dictionary<string,int>();
	}

	// build a  soldier bonus dictionary (territory bonus + continent bonus)
	public void UpdateSoldierBonus(int numberOfPlayers){
		for (int i = 1; i <= numberOfPlayers; i++) {
			player = "Player" + i;
			soldierIncome [player] = continentBonus.playerContBonus [player] + territoryBonus.playerTerrBonus [player];
		}
		// rebuild soldier bonus list when update has occured
		soldierBonusRank.BySolBonusRank(numberOfPlayers);
	}

}