using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// add or remove soldiers from the battle field
public class ArmyManagement : MonoBehaviour {

	public AudioSource click;

	AddSoldier addSoldier;
	CountryManagement countryManagement;
	TeamChecker teamChecker;
	PlayerTurn playerTurn;
	DisplayEditor displayEditor;
	TroopCount troopCount;
	Phases phases;
	DeploySoldiers deploySoldiers;
	ReceiveBonus receiveBonus;
	Attack attack;
	ButtonColour buttonColour;
	AudioFadeOut audioFadeOut;

	GameObject country, soldierToDelete;
	GameObject GUI, territories;

	List<GameObject> soldiers;

	int rmDeadPlayerNum;

	void Awake(){
		countryManagement = this.GetComponent<CountryManagement> ();
		phases = this.GetComponent<Phases> ();
		teamChecker = this.GetComponent<TeamChecker> ();
		playerTurn = this.GetComponent<PlayerTurn> ();
		deploySoldiers = this.GetComponent<DeploySoldiers> ();
		attack = this.GetComponent<Attack> ();
		audioFadeOut = this.GetComponent<AudioFadeOut> ();

		territories = GameObject.FindGameObjectWithTag ("Territories");
		troopCount = territories.GetComponent<TroopCount> ();

		GUI = GameObject.FindGameObjectWithTag ("GUI");
		displayEditor = GUI.GetComponent<DisplayEditor> ();
		receiveBonus = GUI.GetComponent<ReceiveBonus> ();
		buttonColour = GUI.GetComponent<ButtonColour> ();
	}

	// Add a soldier to the selected country - setup phase only
	public void Add(){

		if (phases.setupPhase) {

			audioFadeOut.MoreTroopsAudio ();
			country = GameObject.FindGameObjectWithTag ("SelectedCountry");

			if (teamChecker.UnderControl (country) & deploySoldiers.CanAddSoldier()) {

				addSoldier = country.GetComponent<AddSoldier> ();
				addSoldier.PlaceSoldier ();
				UpdateNumbers(country,1);
			}

			buttonColour.SetupPlusMinusColour ();
		}
	}

	// Remove a soldier from the selected country - setup phase only
	public void Remove(){

		if (phases.setupPhase) {
			click.Play ();
			country = GameObject.FindGameObjectWithTag ("SelectedCountry");

			// Creates a list of the country's soldiers
			soldiers = new List<GameObject> ();

			if (teamChecker.UnderControl (country) & deploySoldiers.CanRemoveSoldier()) {

				foreach (Transform child in country.transform) {

					if (child.name == "Soldier(Clone)")
						soldiers.Add (child.gameObject);
				}

				// Find and delete the last soldier in the list
				if (soldiers.Count > 1) {

					soldierToDelete = soldiers [soldiers.Count - 1];

					if (soldierToDelete.tag == "DeployedSoldier") {

						DestroyImmediate (soldierToDelete);
						UpdateNumbers (country, -1);
						troopCount.UpdateTroopBankV2 (playerTurn.CurrentPlayer (), -1);
					}
				}
			}

			buttonColour.SetupPlusMinusColour ();
		}
	}

	// Removes the dead after a battle (same as Remove() function)
	public void RemoveDead(string countryTag, int numberDead){

		// Creates a list of the country's soldiers
		country = GameObject.FindGameObjectWithTag(countryTag);
		soldiers = new List<GameObject>();

		foreach (Transform child in country.transform) {

			if (child.name == "Soldier(Clone)")
				soldiers.Add (child.gameObject);
		}

		// Find and delete the last soldier in the list
		for (int i = 1; i <= numberDead; i++) {

				soldierToDelete = soldiers [soldiers.Count - i];
				if (soldierToDelete != null)

				// DesIm used for defaultTrans in SoldierTrans to work after Claim in TakeControl
				DestroyImmediate (soldierToDelete);
		}

		if (countryTag == "AttackingCountry")
			rmDeadPlayerNum = attack.attackingPlayer;

		else if (countryTag == "DefendingCountry")
			rmDeadPlayerNum = attack.defendingPlayer;
		
		//UpdateTroopNumbers (country, -numberDead);
		countryManagement.ChangeArmySize (country, -numberDead);
		troopCount.UpdateTroopBankV2 (rmDeadPlayerNum, -numberDead);
	}

	// Update various stats
	public void UpdateNumbers(GameObject country, int ChangeArmyBy){

		// update numbers
		deploySoldiers.soldiersLeft = deploySoldiers.soldiersLeft - ChangeArmyBy;
		receiveBonus.SoldierBonusDisplay(deploySoldiers.soldiersLeft);
		countryManagement.ChangeArmySize (country, ChangeArmyBy);

		// display number of soldiers added during turn
		displayEditor.SetupDeploySoldier (country);
	}
		
}
