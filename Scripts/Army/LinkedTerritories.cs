using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// only allows troops to be transfered between connected territories
public class LinkedTerritories : MonoBehaviour {

	string[][] network;

	List<string> directNeighbours;
	List<string> historyList;
	List<string> endingCountries;

	TargetingNetwork targetingNetwork;
	TeamChecker teamChecker;
	PlayerTurn playerTurn;
	GameInstructions gameInstructions;

	GameObject GUI;
	GameObject toCountry;

	bool safePassage, runWhileLoop;


	void Awake(){
		targetingNetwork = this.GetComponent<TargetingNetwork> ();
		teamChecker = this.GetComponent<TeamChecker> ();
		playerTurn = this.GetComponent<PlayerTurn> ();

		GUI = GameObject.FindGameObjectWithTag ("GUI");
		gameInstructions = GUI.GetComponent<GameInstructions> ();
	}

	void Start(){
		network = targetingNetwork.network;

		historyList = new List<string> ();
		safePassage = false;
		runWhileLoop = true;
	}

	// checks for safe paths for a given country - returns the end results of the path
	public bool SafePath(GameObject fromCountry, GameObject toCountry){

		List<string> safePathList = new List<string> ();
		safePathList.Add (fromCountry.name);

		while (runWhileLoop) {

			safePathList = ExpandRoutes (safePathList);

			// checks if the required country has been reached
			foreach (string safeCountry in safePathList) {

				if (safeCountry == toCountry.name) {

					gameInstructions.MoveTroopButtons (fromCountry.name, toCountry.name);

					// end loop
					EndCheck();
					safePassage = true;
					return safePassage;
				}
			}

			// ends loop if there are no more safe routes to take
			if (safePathList.Count == 0) {

				gameInstructions.NotConnect (fromCountry.name, toCountry.name);

				// reset values for next time code is run - sets safePassage = false
				EndCheck ();
				safePassage = false;
				return safePassage;
			}

		}

		gameInstructions.NotConnect (fromCountry.name, toCountry.name);

		// returns false if toCountry is not found
		EndCheck();
		return false;
	}

	// returns a list of friendly neighbouring countries for a given list of countries
	List<string> ExpandRoutes(List<string> startingCountries){

		// reset list
		endingCountries = new List<string> ();

		// build temp list to hold values to add into endingCountry list
		List <string> tempList = new List<string> ();

		// check available routes for all countries in startingCountries
		for (int i = 0; i < startingCountries.Count; i++) {

			tempList = AvailableRoutes (startingCountries [i]);

			// add all new possible country routes to endingCountries
			foreach (string land in tempList) {
				endingCountries.Add (land);
			}
		}

		// ends loop if there are no more available paths
		if (endingCountries.Count == 0)
			EndCheck ();

		return endingCountries;
	}

	// returns a list of friendly neighbouring countries for a given country
	List<string> AvailableRoutes(string country){

		directNeighbours = new List<string> ();

		foreach (string[] subNetwork in network) {

			// searches for the given country in the network
			if (country == subNetwork [0]) {

				// searches the subNetwork (excl. the given country)
				foreach (string neighbourName in subNetwork.Skip(1)) {

					// need a GameObject for GetPlayer function
					GameObject neighbourObject = GameObject.Find (neighbourName);

					// if country is undercontrol add to list of friendlyNeighbours
					if (teamChecker.GetPlayer (neighbourObject) == playerTurn.CurrentPlayer ())
						directNeighbours.Add (neighbourName);
				}
			}
		}
		// update history list with selected country
		historyList.Add (country);

		// removes previously visited countries
		RemoveOldContries (historyList, directNeighbours);

		// update histroy list
		foreach (string neighbour in directNeighbours)
			historyList.Add (neighbour);

		return directNeighbours;
	}

	// ensures forwards expansion - removes countries from the given list that are present in the history list
	void RemoveOldContries(List<string> historyList, List<string> givenList){

		foreach (string oldCountry in historyList) {

			for (int i = givenList.Count - 1; i >= 0; i--) {

				if (givenList [i] == oldCountry)
					givenList.RemoveAt (i);
			}
		}
	}

	// reset history list
	void EndCheck(){

		// ends loop searching for new paths if no more paths are available
		runWhileLoop = true;

		// reset values for next time code is run
		historyList = new List<string> ();
	}

}
