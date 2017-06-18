using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBonus : MonoBehaviour {

	// total bonus recevied from continent and territory bonuses
	public Dictionary<string,int> soldierIncome = new Dictionary<string,int>();

	GameStats gameStats;
	TerritoryBonus territoryBonus;
	ContinentBonus continentBonus;
	SoldierBonusRank soldierBonusRank;

	int player, lastPlayer, countryCounter, playerSoldierBonus;

	void Awake(){
		gameStats = this.GetComponent<GameStats> ();
		territoryBonus = this.GetComponent<TerritoryBonus> ();
		continentBonus = this.GetComponent<ContinentBonus> ();
		soldierBonusRank = this.GetComponent<SoldierBonusRank> ();
	}

	void Start(){
		// build initial dictionaries before updating data
		for (int i = 1; i <= gameStats.numberOfPlayers; i++)
			soldierIncome.Add ("Player" + i, 0);
	}

	// build a  soldier bonus dictionary (territory bonus + continent bonus)
	// called in ContinentBonus & TerritoryBonus
	public void UpdateSoldierBonus(){
		for (int i = 1; i <= gameStats.numberOfPlayers; i++)
			soldierIncome ["Player" + i] = continentBonus.playerContBonus ["Player" + i] + territoryBonus.playerTerrBonus ["Player" + i];

		// rebuild soldier bonus list when update has occured
		soldierBonusRank.BySolBonusRank();
	}

}