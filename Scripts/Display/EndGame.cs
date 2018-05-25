using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

    TerritoryRank territoryRank;
    TerritoryCount territoryCount;
    ButtonColour buttonColour;
    PlayerTurn playerTurn;

    GameObject territories, scriptHolder;
    public GameObject scroll;

    public Text endGameText;

    private void Awake() {
        territories = GameObject.FindGameObjectWithTag("Territories");
        territoryRank = territories.GetComponent<TerritoryRank>();
        territoryCount = territories.GetComponent<TerritoryCount>();

        scriptHolder = GameObject.FindGameObjectWithTag("ScriptHolder");
        playerTurn = scriptHolder.GetComponent<PlayerTurn>();

        buttonColour = this.GetComponent<ButtonColour>();
    }

    public void DoesGameEnd() {

        // gets the players ranked 1st and 2nd by territory count
        string playerRank1 = "Player" + territoryRank.TerrCountPlayerRanks[0];
        string playerRank2 = "Player" + territoryRank.TerrCountPlayerRanks[1];

        // if player ranked 2nd has no territories game is over
        if (territoryCount.landCounter[playerRank2] == 0) {

            // show end game text
            scroll.gameObject.SetActive(true);
            endGameText.gameObject.SetActive(true);

            // determins player win or player lose text
            if (playerRank1 == "Player1")
                PlayerWins();

            else
                ComputerWins();

            // prevents player interacting with game buttons
            CloseGame();
        }
    }

    // player wins text
    public void PlayerWins() {
        endGameText.text = "<b>Congratulations you WON!</b>";
        endGameText.fontSize = 26;
    }

    // player loses text
    public void ComputerWins() {
        endGameText.text = "<b>You Lost, better luck next time!</b>";
        endGameText.fontSize = 26;
    }

    void CloseGame() {

        // locks all buttons
        buttonColour.LockAllButtons();
        buttonColour.LockButton("instructions");

        // changes player turn so that its not player 1 - deactivating OnMouseDown countrySelector
        if (playerTurn.CurrentPlayer() == 1)
            playerTurn.NextPlayer(false);
    }
    
}
