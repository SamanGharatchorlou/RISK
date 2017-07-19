using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFunctions : MonoBehaviour {

	List<string> underControl;
	List<string> underPopulatedControl;
	List<string> enemyNeighbours;

	string[] neighbours;

	TeamChecker teamChecker;
	CountryManagement countryManagement;
	TargetingNetwork targetingNetwork;

	public float timeDelay;

	void Awake(){
		teamChecker = this.GetComponent<TeamChecker> ();
		countryManagement = this.GetComponent<CountryManagement> ();
		targetingNetwork = this.GetComponent<TargetingNetwork> ();
	}

	void Start(){
		// default time delay is 1s
		timeDelay = 1;
	}

	public void ChangeDelay(float newDelayTime){
		timeDelay = newDelayTime;
	}

	// builds a list of countries under player control with a soldier count >= a given value
	public List<string> ControlledCountryList(int soldierCount){
		underControl = new List<string>();
		for (int i = 0; i < 42; i++) {
			string someCountry = targetingNetwork.network [i] [0];
			if (teamChecker.UnderControlName (someCountry) & countryManagement.GetArmySize (someCountry) >= soldierCount)
				underControl.Add (someCountry);
		}
		return underControl;
	}

	// builds a list of the given country's enemy neighbours
	public List<string> EnemyNeighbourList(string country){
		enemyNeighbours = new List<string> ();
		// all the given country's neighbours
		neighbours = targetingNetwork.Neighbours (country);
		// add all enemy neighbours into enemyNeighbour list
		foreach(string neighbour in neighbours){
			if (!teamChecker.UnderControlName (neighbour)) {
				enemyNeighbours.Add (neighbour);
			}
		}
		return enemyNeighbours;
	}

	// sorts a list of countries by army size
	public List<string> SortList(List<string> countries){
		List<int[]> valueIndexes;
		valueIndexes = new List<int[]> ();
		// build list of army sizes and an index tracker
		for(int i=0;i<countries.Count;i++){
			// [ army size, country index ]
			valueIndexes.Add (new int[]{ countryManagement.GetArmySize (countries [i]), i });
		}

		List<int> armySizes = new List<int> ();
		for (int j = 0; j < countries.Count; j++)
			armySizes.Add (valueIndexes [j] [0]);
		// sort highest to lowest by army size
		armySizes.Sort();
		armySizes.Reverse ();

		// list of countries sorted by army sizes
		List<string> sortedList;
		sortedList = new List<string> ();

		// iterate through ordered armySizes
		for (int k = 0; k < armySizes.Count; k++) {
			// iterate through valueIndex
			for (int l = 0; l < valueIndexes.Count; l++) {
				if (armySizes [k] == valueIndexes [l] [0]) {
					// add country if its respective army size is matched
					sortedList.Add (countries [valueIndexes [l] [1]]);
					// remove entry to prevent duplicates
					valueIndexes.RemoveAt (l);
					// jump out of loop and move onto next k
					l = countries.Count;
				}
			}
		}
		return sortedList;
	}

}
