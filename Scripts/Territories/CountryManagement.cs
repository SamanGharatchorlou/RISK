using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryManagement : MonoBehaviour {

	// dictionary of all 42 territories
    // this will be updated with the number of troops each country holds during the game
	Dictionary<string,int> armySizes = new Dictionary<string, int> ();

	string countryName ;

	void Awake () {

		//South America
		armySizes.Add ("Argentina", 0);
		armySizes.Add ("Brazil", 0);
		armySizes.Add ("Peru", 0);
		armySizes.Add ("Venezuela", 0);

		//North America
		armySizes.Add ("Alaska", 0);
		armySizes.Add ("Alberta", 0);
		armySizes.Add ("Central America", 0);
		armySizes.Add ("Eastern United States", 0);
		armySizes.Add ("Greenland", 0);
		armySizes.Add ("North West Territory", 0);
		armySizes.Add ("Ontario", 0);
		armySizes.Add ("Quebec", 0);
		armySizes.Add ("Western United States", 0);

		//Afica
		armySizes.Add ("Egypt", 0);
		armySizes.Add ("North Africa", 0);
		armySizes.Add ("South Africa", 0);
		armySizes.Add ("Congo", 0);
		armySizes.Add ("East Africa", 0);
		armySizes.Add ("Madagascar", 0);

		//Europe
		armySizes.Add ("Iceland", 0);
		armySizes.Add ("Great Britain", 0);
		armySizes.Add ("Scandinavia", 0);
		armySizes.Add ("Northern Europe", 0);
		armySizes.Add ("Western Europe", 0);
		armySizes.Add ("Southern Europe", 0);
		armySizes.Add ("Ukraine", 0);

		//Asia
		armySizes.Add ("Afghanistan", 0);
		armySizes.Add ("China", 0);
		armySizes.Add ("India", 0);
		armySizes.Add ("Irkutsk", 0);
		armySizes.Add ("Japan", 0);
		armySizes.Add ("Kamchatka", 0);
		armySizes.Add ("Middle East", 0);
		armySizes.Add ("Mongolia", 0);
		armySizes.Add ("Siam", 0);
		armySizes.Add ("Siberia", 0);
		armySizes.Add ("Ural", 0);
		armySizes.Add ("Yakutsk", 0);

		//Austrailia
		armySizes.Add ("Eastern Australia", 0);
		armySizes.Add ("Indonesia", 0);
		armySizes.Add ("New Guinea", 0);
		armySizes.Add ("Western Australia", 0);
	}

	public void ChangeArmySize(GameObject country, int change){
		countryName = country.name;
		armySizes [countryName] = armySizes[countryName] + change;
	}

	public int GetArmySize(string country){
		return armySizes [country];
	}

}
