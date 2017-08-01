using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// gives a soldier the colour of the territory its about to be placed on
public class SoldierManagement : MonoBehaviour {

	PlayerTurn playerTurn;

	Color playerColour;
	Renderer rend;
    
	float r, g, b, a;

	void Awake(){
		playerTurn = this.GetComponent<PlayerTurn> ();
	}

	// gets the currently active players rgb colour values
	void ActiveColour(int playerIndex, out float r, out float g, out float b, out float a){
		r = playerTurn.playerColourList [playerIndex] [0];
		g = playerTurn.playerColourList [playerIndex] [1];
		b = playerTurn.playerColourList [playerIndex] [2];
		a = 1;
	}

	// sets the soldier(or any GameObject) a given players colour
	public void SetSoldierColour(GameObject soldier, int playerIndex) {
		r = 0;
		g = 0;
		b = 0;
		a = 0;

		ActiveColour (playerIndex, out r, out g, out b, out a);

		playerColour = new Color (r, g, b, a);
		rend = soldier.GetComponent<Renderer>();
		rend.material.shader = Shader.Find("Standard");
		rend.material.SetColor ("_Color", playerColour);
	}
}