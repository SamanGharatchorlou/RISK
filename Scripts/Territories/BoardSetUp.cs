using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Sets up the board - call most opening scripts here that begin the game
public class BoardSetUp : MonoBehaviour {

	public List <int[]> landBank;
	public InputField inputData;
    public Button startButton;

	public AudioSource openingSound;
	public AudioSource backgroundAudio;

	AddSoldier addSoldier;
	CountryManagement countryManagement;
	SoldierManagement soldierManagement;
	GameStats gameStats;
	AllocateSoldiers allocateSoldiers;
	Phases phases;
	OpeningDeployment openingDeployment;
	GameInstructions gameInstructions;
	PlayerTurn playerTurn;
	AudioFadeOut audioFadeOut;

	GameObject scriptHolder, GUI, clone;

	public int numberOfPlayers;
	int randomPlayerNum;
	int territoryCount, territoriesAllocated;
	int randomIndex, update, stroredValue;

	GameObject inputBox;

	void Awake (){
		GUI = GameObject.FindGameObjectWithTag ("GUI");
		openingDeployment = GUI.GetComponent<OpeningDeployment> ();
		gameInstructions = GUI.GetComponent<GameInstructions> ();

		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		soldierManagement = scriptHolder.GetComponent<SoldierManagement> ();
		allocateSoldiers = scriptHolder.GetComponent<AllocateSoldiers> ();
		phases = scriptHolder.GetComponent<Phases> ();
		playerTurn = scriptHolder.GetComponent<PlayerTurn> ();
		countryManagement = scriptHolder.GetComponent<CountryManagement> ();
		audioFadeOut = scriptHolder.GetComponent<AudioFadeOut> ();

		gameStats = this.GetComponentInChildren<GameStats> ();

		inputBox = GameObject.Find ("InputField");
	}
		
	// Starts the game - runs all required functions (Start Game button)
	public void StartGame(){
		if (numberOfPlayers <= 5 & numberOfPlayers >= 3) {
			// set up playerTurn list
			playerTurn.ChangePlayerCount (numberOfPlayers);
			// randomly distribute all territories to players by placing 1 soldier on it
			PlayerLandBank (numberOfPlayers);
			SetBoard ();
			// build game stats
			gameStats.SetUpGameStats (numberOfPlayers);
			// give players starting armies
			allocateSoldiers.BuildSoldierBank (numberOfPlayers);
			// set up opening deployment
			openingDeployment.BuildDeployementTable (numberOfPlayers);
			// instruction text
			gameInstructions.OpeningPhasePlacement ();
			// remove input box from game
			Destroy (inputBox);
			// fades out audio over 4s
			StartCoroutine(audioFadeOut.FadeOut(openingSound,2f));
			backgroundAudio.Play ();
			// removes all accidental country selections before game starts
			ClearSelections();
            // locks start button after single use
            //TODO: this doesnt work
            startButton.gameObject.SetActive(false);
            inputData.gameObject.SetActive(false);
		}
	}

	// player inputs number of enemies
	public void InputPlayerNumbers(string playerNumbers){
		if (playerNumbers != "")
			// number of players = enemies + human player
			numberOfPlayers = int.Parse (playerNumbers) + 1;
	}

	// builds a bank with the number of territories allocated by each player at start of game
	public void PlayerLandBank(int numberOfPlayers) {
		landBank = new List<int[]> ();
		territoryCount = 42;
		territoriesAllocated = territoryCount / numberOfPlayers;
		// build a list of players allocating the land among them equally
		for (int playerNumber = 0; playerNumber < numberOfPlayers; playerNumber++) {
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
				countryManagement.ChangeArmySize (country.gameObject, 1);
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

	// removes all 'selectedCountry' tags for start of game
	public void ClearSelections(){
		GameObject[] countries;
		countries = GameObject.FindGameObjectsWithTag ("SelectedCountry");
		foreach (GameObject country in countries) {
			country.gameObject.tag = "Untagged";
		}
	}

}