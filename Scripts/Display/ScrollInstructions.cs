using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollInstructions : MonoBehaviour {

    ButtonColour buttonColour;

    public Text scrollHeader,scrollNavigation, scrollMainBody;
    public GameObject scroll;
    public Button startButton;
    public InputField inputBar;

    bool gameStarted;
    string homePageText;

    private void Awake() {
        buttonColour = this.GetComponent<ButtonColour>();
    }

    void Start() {

        gameStarted = false;

        // header text
        scrollHeader.text = "<b>RISK</b> - Game Instructions";

        // navigation text
        scrollNavigation.text = "Page navigation\n" +
            "Home Page: 'H'\n" +
            "Opening Phase: 'O'\n" +
            "Setup Phase: 'S'\n" +
            "Attack Phase: 'A'\n" +
            "Movement Phase: 'M'\n" +
            "Game Interface: 'I'\n" +
            "Start Game: 'Enter'";

        //  default main body text
        homePageText = "Aim: Take control of the whole map\n" +
            "\n" +
            "The game starts with an Opening Phase('O') where the game board is setup. After the Player (that's you) is allocated soldiers and will begin their turn.\n" +
            "\n" +
            "Every player turn is made up of 3 phases:\n" +
            "Setup Phase ('S') - Placing troops on your countries\n" +
            "Attack Phase ('A') - Attacking enemies\n" +
            "Movement Phase ('M') - Moving troops\n" +
            "\n" +
            "Game Interface ('I') - All the different  buttons and information displayed on the screen\n" +
            "\n" +
            "Press the (letters in the brackets) to find out more, recommended\n" +
            "\n" +
            "To start the game press 'Enter'";

        scrollMainBody.text = homePageText +
            "\n\n" +
            "Press 'O' to move onto the Opening Phase instructions";
    }

    private void Update() {

        // home page
        if (Input.GetKeyDown(KeyCode.H))
            scrollMainBody.text = homePageText;

        // opening phase
        if (Input.GetKeyDown(KeyCode.O)) {

            scrollMainBody.text = "OPENING PHASE\n" +
                "\n" +
                "To setup the game board each player is allocated a number of armies. Player 1 gets to place one army on a country under their control.\n" +
                "\n" +
                "To place an army select that country you wish to place it on. Then player 2 does the same, then 3 etc. and back to player 1.\n" +
                "This process repeats untill you have no more armies to place\n" +
                "\n" +
                "The number of armies each player has left to place is shown on the right side of the screen\n" +
                "\n" +
                "Note: if you think its taking too long, speed up the AI for this phase using the slider at the bottom right of the screen (you can bring it back down later)\n" +
                "\n" +
                "Press 'S' to move onto the Setup Phase instructions";
        }

        // setup phase
        if (Input.GetKeyDown(KeyCode.S)) {
            scrollMainBody.text = "SETUP PHASE\n" +
                "\n" +
                "At the start of your turn you will receive bonus troops. By selecting a country under your control and using the '+/-' buttons you can place them on any of your contries.\n" +
                "\n" +
                "The number of bonus troops you will receive per go = number of territories owned / 3 + continent bonus (minimum 3)\n" +
                "\n" +
                "Continent bonus - the map is split up into 6 continents, made up of 42 countries. If you control all the countries within a continent at the START of your turn you will be awarded additional troops:\n" +
                "Asia: 7\n" +
                "Europe: 5\n" +
                "North America: 5\n" +
                "Africa: 3\n" +
                "South America: 2\n" +
                "Australia: 2\n" +
                "\n" +
                "Unsurprisingly the greater the bonus the greater the difficulty of holding the continent...dont even bother with asia until later\n" +
                "\n" +
                "The minimun number of troops received per go is 3\n" +
                "\n" +
                "Press 'A' to move onto the Attack Phase instructions";
        }

        // attack phase
        if (Input.GetKeyDown(KeyCode.A)) {
            scrollMainBody.text = "ATTACK PHASE\n" +
                "\n" +
                "If you have more than 2 armies on a country you can attack any neighbouring enemy country\n" +
                "\n" +
                "To begin an attack select the country you wish to attack with and select the 'Attack' button, then select the country you'd like to attack.\n" +
                "To attack them press the 'Battle' button (this can be pressed repeatedly until either you or the enemy run out of troops).\n" +
                "\n" +
                "Press 'M' to move onto the Movement Phase instructions";
        }

        // movement phase
        if (Input.GetKeyDown(KeyCode.M)) {
            scrollMainBody.text = "MOVEMENT PHASE\n" +
                "\n" +
                "You can move one set of troops across your connected territories i.e. if there is a path of countries connected under your control you can move any number of troops between them.\n" +
                "This can only be done once per turn\n" +
                "\n" +
                "To move troops select the country you want to move troops from and select the move button, this is your 'from country'.\n" +
                "Following this select the country you want to move your troops to and select the move button to confirm the selection, this is your 'to country'.\n" +
                "\n" +
                "Using the +/- buttons you can move your troops in any direction\n" +
                "\n" +
                "Press 'I' to move onto the Game Interface explanations";
        }

        // buttons
        if (Input.GetKeyDown(KeyCode.I)) {
            scrollMainBody.text = "There's lots of differnt things on the screen here's what it all means\n" +
                "\n" +
                "Top left - Opens up this game instruction scroll, becasue you probably didnt read it the first time\n" +
                "\n" +
                "Top middle - Shows which players turn it is and how many turns you've had\n" +
                "\n" +
                "Right middle - Ranking system shows various stats and ranks the players on either:\n" +
                "Total number of Troops, Number of territories undercontrol or Troop bonuses\n" +
                "Press the category button to change the category at any time\n" +
                "\n" +
                "Bottom Right - AI speed controller. Use this at any time to speed up or slow down the AI's 'thinking' time\n" +
                "\n" +
                "Bottom Middle - Gives basic information on who you've currently got selected, attacking, moving etc.\n" +
                "\n" +
                "Bottom left - All the game buttons: Add, Remove, Attack, Battle, Movement and End Phase\n" +
                "\n" +
                "Left middle - Instructions on what to do next if you get stuck";
        }

        if (Input.GetKeyDown("return")) {

            // only activates once
            if (!gameStarted) {
                startButton.gameObject.SetActive(true);
                buttonColour.UnlockButton("start");
                inputBar.gameObject.SetActive(true);
                buttonColour.UnlockButton("input");
                gameStarted = true;
            }

            // remove instruction scroll
            scrollHeader.gameObject.SetActive(false);
            scrollNavigation.gameObject.SetActive(false);
            scrollMainBody.gameObject.SetActive(false);
            scroll.gameObject.SetActive(false);
        }
    }

    // show instruction scroll
    public void ShowScroll() {
        scrollHeader.gameObject.SetActive(true);
        scrollNavigation.gameObject.SetActive(true);
        scrollMainBody.gameObject.SetActive(true);
        scroll.gameObject.SetActive(true);
    }

}
