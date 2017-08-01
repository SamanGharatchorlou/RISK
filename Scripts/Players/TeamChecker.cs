using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// returns the colour or player number of a country
public class TeamChecker : MonoBehaviour {

	PlayerTurn playerTurn;

	GameObject scriptHolder;
	Transform childObject;

	Renderer rend;
	Color colour, playerColour;

	float r, g, b;
	int playerIndex;
	int indexPlayer, a;


	void Awake () {
		scriptHolder = GameObject.FindGameObjectWithTag ("ScriptHolder");
		playerTurn = scriptHolder.GetComponent<PlayerTurn> ();
	}

	// returns the player number of the "SelectedCountry"
	public int GetPlayer(GameObject country) {
		return ColourToPlayer (PlayerColour (country));
	}

	// returns the player number of a particular colour
	public int ColourToPlayer(Color countryColour) {
		r = countryColour.r;
		g = countryColour.g;
		b = countryColour.b;

		// Creates a new list to store the possible i's
		List<int> indexListOne = new List<int> ();
		List<int> indexListTwo = new List<int> ();
		playerIndex = 0;

		for (int i = 0; i < playerTurn.playerColourList.Length; i++) {
			// store i's with the correct r value
			if (playerTurn.playerColourList [i] [0] == r)
				indexListOne.Add (i);
		}
		if (indexListOne.Count == 1)
			return indexListOne [0]+1;

		// stores i's with the correct r and g colour
		for (int j = 0; j < indexListOne.Count; j++) {
			if (playerTurn.playerColourList [indexListOne [j]] [1] == g)
				indexListTwo.Add (indexListOne [j]);
		}
		if (indexListTwo.Count == 1)
			return indexListTwo [0]+1;

		// finds the correct player
		for (int k = 0; k < indexListTwo.Count; k++) {
			if (playerTurn.playerColourList [indexListTwo [k]] [2] == b)
				playerIndex = indexListTwo [k];
		}
		return playerIndex+1;
	}

	// returns the colour of the "selected country"
	public Color PlayerColour(GameObject country){
		// returns default colour if there is no country - this only to prevent errors crashing game
		if (country == null) {
			colour = GetColour (6);
			return colour;
		}
		// searches for the first soldier in a country
		for (int i = 0; i < country.transform.childCount; i++) {
			childObject = country.transform.GetChild (i);
			if (childObject.name == "Soldier(Clone)")
				break;
		}
		rend = childObject.GetComponent<Renderer> ();
		colour = rend.material.color;

		return colour;
	}
		
	// returns the colour of a player
	public Color GetColour(int player){
		indexPlayer = player - 1;
		r = playerTurn.playerColourList [indexPlayer] [0];
		g = playerTurn.playerColourList [indexPlayer] [1];
		b = playerTurn.playerColourList [indexPlayer] [2];
		a = 1;

		playerColour = new Color (r, g, b, a);
		return playerColour;
	}

	// checks if gameobject is under current player control
	public bool UnderControl(GameObject item){
		if (playerTurn.CurrentPlayer () == GetPlayer (item))
			return true;
		else
			return false;
	}

	// checks if gameobject is under current player control
	public bool UnderControlName(string item){
		GameObject someItem = GameObject.Find (item);
		if (playerTurn.CurrentPlayer () == GetPlayer (someItem))
			return true;
		else
			return false;
	}

}
