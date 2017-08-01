using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterPhase : MonoBehaviour {

	List<List<GameObject>> SelectionList;
	List<GameObject> player2Armies;
	List<GameObject> player3Armies;
	List<GameObject> player4Armies;
	List<GameObject> player5Armies;

	PlayerTurn playerTurn;
	SetupPhase setupPhase;
	AllocateSoldiers allocateSoldiers;
	BoardSetUp boardSetUp;

	GameObject territories;

	GameObject selectedCountry;

	int enemies, currentPlayer, currentPlayerIndex, randomListIndex;
	bool randomPicker;

	void Awake () {
		playerTurn = this.GetComponent<PlayerTurn> ();
		setupPhase = this.GetComponent<SetupPhase> ();
		allocateSoldiers = this.GetComponent<AllocateSoldiers> ();

		territories = GameObject.Find ("Territories");
		boardSetUp = territories.GetComponent<BoardSetUp> ();
	}

	void Start(){
		// lists to hold countries previously placed on
		SelectionList= new List<List<GameObject>>();
		player2Armies = new List<GameObject> ();
		player3Armies = new List<GameObject> ();
		player4Armies = new List<GameObject> ();
		player5Armies = new List<GameObject> ();

		SelectionList.Add (player2Armies);
		SelectionList.Add (player3Armies);
		SelectionList.Add (player4Armies);
		SelectionList.Add (player5Armies);

	}

	// 50/50 chance to place troop on country already populated or onto a new one (the larger the army the more likely it will place on the country)
	public void AIDeployTroop(){
		currentPlayer = playerTurn.CurrentPlayer ();
		currentPlayerIndex = currentPlayer - 2;
		if (currentPlayer != 1) {
			// only runs once and places a country in the list for the below code to work
			if (playerTurn.turn == 1)
				PrePopulateLists ();
			// take a random country from within the built list of populated countries
			if (PickfromList ()) {
				randomListIndex = Mathf.FloorToInt (Random.Range (0f, SelectionList [currentPlayerIndex].Count));
				selectedCountry = SelectionList [currentPlayerIndex] [randomListIndex];
			} 
			// or choose another front line country
			else {
				setupPhase.SelectFrontLineCountry ();
				selectedCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
				// adds the new selection to the list
				SelectionList [currentPlayerIndex].Add (selectedCountry);
			}
			// places a soldier onto country
			allocateSoldiers.DropSoldier (selectedCountry);
		}
	}
		
	// removes the unneeded lists depending on the number of players
	void SetUpLists(){
		enemies = boardSetUp.numberOfPlayers - 1;
		if (enemies < 4)
			SelectionList.RemoveRange (enemies, SelectionList.Count - enemies);
	}

	// for each players first turn pre populate the list with an entry
	void PrePopulateLists(){
		// selects front line country
		setupPhase.SelectFrontLineCountry ();
		selectedCountry = GameObject.FindGameObjectWithTag ("SelectedCountry");
		// adds this to the players list
		SelectionList [currentPlayerIndex].Add (selectedCountry);
	}

	// 50/50 chance to return true or false
	bool PickfromList(){
		int randomNumber = Mathf.FloorToInt (Random.Range (0f, 2f));
		// 1=true 2 = false
		if (randomNumber == 0)
			randomPicker = true;
		else if (randomNumber == 1)
			randomPicker = false;
	
		return randomPicker;
	}

}
