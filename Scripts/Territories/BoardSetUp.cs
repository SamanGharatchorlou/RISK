using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets up the board - call most opening scripts here that begin the game
public class BoardSetUp : MonoBehaviour {

	public List <int[]> landBank;

	AddSoldier addSoldier;
	ArmyManagement armyManagement;
	CountryManagement countryManagement;
	SoldierManagement soldierManagement;
	GameStats gameStats;
	AllocateSoldiers allocateSoldiers;
	Phases phases;
	OpeningDeployment openingDeployment;
	GameInstructions gameInstructions;

	GameObject scriptHolder, GUI, clone;

	float r, g, b, a;

	int playerNumber, randomPlayerNum;
	int territoryCount, territoriesAllocated, territoriesLeft;
	int randomIndex, update, stroredValue;

	void Awake (){
		GUI = GameObject.FindGameObjectWithTag ("GUI");
		openingDeployment = GUI.GetComponent<OpeningDeployment> ();
		gameInstructions = GUI.GetComponent<GameInstructions> ();

		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		armyManagement = scriptHolder.GetComponent<ArmyManagement> ();
		soldierManagement = scriptHolder.GetComponent<SoldierManagement> ();
		allocateSoldiers = scriptHolder.GetComponent<AllocateSoldiers> ();
		phases = scriptHolder.GetComponent<Phases> ();

		gameStats = this.GetComponent<GameStats> ();
	}

	// TODO: change into restart game button after selected (inlc. confirmation is selected)
	// Starts the game - runs all required functions (Start Game button)
	public void StartGame(){
		// randomly distribute all territories
		PlayerLandBank (gameStats.numberOfPlayers);
		SetBoard ();
		// build game stats
		gameStats.SetUpGameStats ();
		// give players starting armies
		allocateSoldiers.BuildSoldierBank();
		// set up opening deployment
		openingDeployment.BuildDeployementTable();
		// instruction text
		gameInstructions.OpeningPhasePlacement();
	}

	// builds a bank with the number of territories allocated by each player at start of game
	public void PlayerLandBank(int numberOfPlayers) {
		landBank = new List<int[]> ();
		territoryCount = 42;
		territoriesAllocated = territoryCount / gameStats.numberOfPlayers;
		// build a list of players allocating the land among them equally
		for (int playerNumber = 0; playerNumber < gameStats.numberOfPlayers; playerNumber++) {
			landBank.Add (new int[]{ playerNumber, territoriesAllocated });
			territoryCount -= territoriesAllocated;
		}
		// distribute the remaining land starting with the last player
		for (int a = 1; a <= territoryCount; a++) {
			landBank [numberOfPlayers - a] [1] = landBank [numberOfPlayers - a] [1] + 1;
		}
	}
		
	// Randomly places a soldier on a players allocated piece of land
	void SetBoard(){
		// Loops through each country adding a soldier at the COM
		foreach (Transform continent in this.transform) {
			foreach (Transform country in continent.transform) {
				//Instantiate at countries COM soldier at the stored placer location
				addSoldier = country.GetComponent<AddSoldier>();
				clone = Instantiate (Resources.Load("Soldier"), addSoldier.findCOM(country), Quaternion.identity) as GameObject;
				clone.transform.parent = country.transform;
			
				// make the soldiers random colours
				randomPlayerNum = RandomPlayer();
				soldierManagement.SetSoldierColour (clone, randomPlayerNum);
				armyManagement.UpdateTroopNumbers (country.gameObject, 1);
			}
		}
		// selects Alaska as the default selected country - prevent +button at the start error
		GameObject defaultCountry = GameObject.Find ("Alaska");
		defaultCountry.gameObject.tag = "SelectedCountry";
		// start opening phase
		phases.startingPhase = false;
		phases.openingPhase = true;
	}
		
	// picks a random player from the landBank & updates landbank
	// Note: this code modifies landBank - re-run PlayerLandBank if the data table needs to be used
	int RandomPlayer(){
		// pick a random player
		randomIndex = Mathf.FloorToInt (Random.Range (0f, landBank.Count));

		// Remove one (land) from the landBank
		update = landBank [randomIndex][1] - 1;
		landBank [randomIndex][1] = update;
		// Random player number (zero index based)
		stroredValue = landBank [randomIndex] [0];

		// removes the player from the list if no more land can be allocated
		if (landBank [randomIndex][1] == 0) {
			landBank.RemoveAt (randomIndex);
		}
		return stroredValue;
	}

}
