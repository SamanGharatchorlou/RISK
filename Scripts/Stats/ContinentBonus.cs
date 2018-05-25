using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinentBonus : MonoBehaviour {

	// bonues given by each continent
	Dictionary<string,int> continentBonuses = new Dictionary<string,int> ();
	// bonus soldiers reveiced from continent bonuses
	public Dictionary<string,int> playerContBonus;

	TeamChecker teamChecker;
	SoldierBonus soldierBonus;
	PlayerTurn playerTurn;

	GameObject scriptHolder;

	int numbOfPlayers;
	int player, lastPlayer, countryCounter;
	bool notFirstPlayer;

	void Awake(){
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		teamChecker = scriptHolder.GetComponent<TeamChecker> ();
		playerTurn = scriptHolder.GetComponent<PlayerTurn> ();
		soldierBonus = this.GetComponent<SoldierBonus> ();
	}

	void Start(){
		// Continent bonuses
		continentBonuses.Add ("North America", 5);
		continentBonuses.Add ("South America", 2);
		continentBonuses.Add ("Africa", 3);
		continentBonuses.Add ("Europe", 5);
		continentBonuses.Add ("Asia", 7);
		continentBonuses.Add ("Australia", 2);
	}
		
	public void BuildContBonus(int numberOfPlayers){

		playerContBonus = new Dictionary<string,int> ();

		// build initial dictionary
		numbOfPlayers = numberOfPlayers;

		for (int i = 1; i <= numbOfPlayers; i++)
			playerContBonus.Add ("Player" + i, 0);
	}

	// Builds a dictionary of continents bonuses recevied
	public void UpdateContBonus() {

		// Reset continent bonuses before re-building the dictionary again
		for (int i = 1; i <= numbOfPlayers; i++)
			playerContBonus["Player" + i] = 0;

		// Cycle through continents
		foreach (Transform continentTrans in this.transform) {

			// Cycle through each country in continent
			// Initialise values - lastPlayer value is set to anything but 1 to 5
			countryCounter = 0;
			lastPlayer = -1;

			foreach (Transform countryTrans in continentTrans) {
				player = teamChecker.GetPlayer (countryTrans.gameObject);

				// breaks loop if the current and previous country is not owned by the same player
				if (player != lastPlayer & lastPlayer != -1)
					break;

				lastPlayer = player;
				countryCounter++;
			}

			// doesnt give continent bonus to players 1 & 2 on turn 1 first turn
			if (playerTurn.turn == 1 & player < 3)
				notFirstPlayer = false;

			else
				notFirstPlayer = true;
			
			// country bonus is given: if the player owns all countries in continent and the above condition is true
			if (countryCounter == continentTrans.childCount & notFirstPlayer) {
				playerContBonus ["Player" + player] += continentBonuses [continentTrans.name];
			}
		}
		soldierBonus.UpdateSoldierBonus (numbOfPlayers);
	}

}
