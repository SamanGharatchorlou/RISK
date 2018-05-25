using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingNetwork : MonoBehaviour {

    //the first item in each list item (i.e. in each sub list) represents the country
    //the otehr items in the sub list represent all of its neighbouring countries, i.e. countries it can attack
	public string[][] network;

	void Awake(){
		BuildNetwork ();
	}

	void BuildNetwork() {

		// Build territory network
		network = new string[42][] {

			// North America (9)
			new string[]{ "Alaska", "Alberta", "North West Territory", "Kamchatka" },
			new string[]{ "Alberta", "Alaska", "Ontario", "North West Territory", "Western United States" },
			new string[]{ "Central America", "Eastern United States", "Western United States", "Venezuela" },
			new string[]{ "Eastern United States", "Central America", "Western United States", "Ontario", "Quebec" },
			new string[]{ "Greenland", "North West Territory", "Ontario", "Quebec", "Iceland" },
			new string[]{ "North West Territory", "Alaska", "Alberta", "Greenland", "Ontario" },
			new string[]{ "Ontario","Alberta","Eastern United States","Greenland","North West Territory","Quebec","Western United States"},
			new string[]{ "Quebec", "Greenland", "Ontario", "Eastern United States" },
			new string[]{ "Western United States", "Alberta", "Central America", "Eastern United States", "Ontario" },
				
			// South America (4)
			new string[] { "Argentina", "Brazil", "Peru" },
			new string[] { "Brazil", "Argentina", "Peru", "Venezuela", "North Africa" },
			new string[] { "Peru", "Argentina", "Brazil", "Venezuela" },
			new string[] { "Venezuela", "Brazil", "Peru", "Central America" },

			//Africa (6)
			new string[] { "Congo", "East Africa", "North Africa", "South Africa" },
			new string[] { "East Africa", "Congo", "Egypt", "Madagascar", "North Africa", "South Africa", "Middle East" },
			new string[] { "Egypt", "East Africa", "North Africa", "Southern Europe", "Middle East" },
			new string[] { "Madagascar", "East Africa", "South Africa" },
			new string[] { "North Africa", "Congo", "East Africa", "Egypt", "Brazil", "Southern Europe", "Western Europe" },
			new string[] { "South Africa", "Congo", "East Africa", "Madagascar" },

			//Europe (7)
			new string[] { "Great Britain", "Iceland", "Northern Europe", "Scandinavia", "Western Europe" },
			new string[] { "Iceland", "Great Britain", "Scandinavia", "Greenland" },
			new string[] { "Northern Europe", "Great Britain", "Scandinavia", "Southern Europe", "Ukraine", "Western Europe" },
			new string[] { "Scandinavia", "Great Britain", "Iceland", "Northern Europe", "Ukraine" },
			new string[] { "Southern Europe", "Northern Europe", "Ukraine", "Western Europe", "Egypt", "North Africa", "Middle East"},
			new string[] { "Ukraine", "Northern Europe", "Scandinavia", "Southern Europe", "Afghanistan", "Middle East", "Ural" },
			new string[] { "Western Europe", "Great Britain", "Northern Europe", "Southern Europe", "North Africa" },

			//Asia (12)
			new string[] { "Afghanistan", "China", "India", "Middle East", "Ural", "Ukraine" },
			new string[] { "China", "Afghanistan", "India", "Mongolia", "Siam", "Siberia", "Ural" },
			new string[] { "India", "Afghanistan", "China", "Middle East", "Siam" },
			new string[] { "Irkutsk", "Kamchatka", "Mongolia", "Siberia", "Yakutsk" },
			new string[] { "Japan", "Kamchatka", "Mongolia" },
			new string[] { "Kamchatka", "Irkutsk", "Japan", "Mongolia", "Yakutsk", "Alaska" },
			new string[] { "Middle East", "Afghanistan", "India", "East Africa", "Egypt", "Southern Europe", "Ukraine" },
			new string[] { "Mongolia", "China", "Irkutsk", "Japan", "Kamchatka", "Siberia" },
			new string[] { "Siam", "China", "India", "Indonesia" },
			new string[] { "Siberia", "China", "Irkutsk", "Mongolia", "Ural", "Yakutsk" },
			new string[] { "Ural", "Afghanistan", "China", "Siberia", "Ukraine" },
			new string[] { "Yakutsk", "Irkutsk", "Kamchatka", "Siberia" },

			// Australia (4)
			new string[] { "Eastern Australia", "New Guinea", "Western Australia" },
			new string[] { "Indonesia", "New Guinea", "Western Australia", "Siam" },
			new string[] { "New Guinea", "Eastern Australia", "Indonesia", "Western Australia" },
			new string[] { "Western Australia", "Eastern Australia", "Indonesia", "New Guinea" }
		
		};  
	}

	// returns true if the countries are neighbours
	public bool isNeighbour(string attacker, string defender){

		foreach (string[] subNetwork in network) {

			if (subNetwork [0] == attacker) {

				foreach (string country in subNetwork) {

					if (country == defender)
						return true;
				}
			}
		}
		return false;
	}

	// returns a given country's neighbours (incl. itself at index 0)
	public string[] Neighbours(string someCountry){

		foreach(string[] subNetwork in network){

			if (someCountry == subNetwork [0])
				return subNetwork;

		}
		return network[0];
	}

}







