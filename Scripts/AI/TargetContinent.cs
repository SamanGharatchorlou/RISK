using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: is it mathematically possible to choose a contry that there are no friendly troops on?
public class TargetContinent : MonoBehaviour {

    GameObject territories;
    
    TeamChecker teamChecker;

    List<float[]> controlRatios;
    float[] contDiff;
    int[,,] continentTargets;

    int xCounter, playerIndex, countryCounter;
    int largestValIndex, secondLargestValIndex, largestValue, secondLargestValue;

    int targetContIndex, firstTarget, secondTarget;

    bool isTargetContested;

    private void Awake() {
        territories = GameObject.FindGameObjectWithTag("Territories");
        teamChecker = this.GetComponent<TeamChecker>();
    }

    void Start () {
        // represents the continent control difficulty
        //Index values:Continet  0:NA, 1:SA, 2:EU, 3:Asia, 4:Africa, 5:Aus
        contDiff = new float[] { 0.3f, 0.4f, 0.1f, 0.1f, 0.3f, 0.4f };
    }

    //selects a continent as its target depending on the state of the board
    void ChooseContTarget(int playerIndex, int[,,] targetData, int numberOfPlayers) {
        //checks if other players have its second target as their first
        firstTarget = targetData[0, playerIndex, 0];
        secondTarget = targetData[1, playerIndex, 0];

        for (int j = 0; j < numberOfPlayers; j++) {
            //doesn't check itself
            if (j == playerIndex)
                continue;

            //if selected players 2nd target is contested pick 1st target
            if (secondTarget == targetData[0, j, 0]) {
                targetContIndex = firstTarget;
                break;
            }
            //if selected players 1st target is contested make 2nd target viability check
            else if (firstTarget == targetData[0, j, 0]) {
                //if 2nd target viability score > 75% of 1st target then take 2nd target
                if (targetData[1, playerIndex, 1] > 0.75 * targetData[0, playerIndex, 1]) {
                    targetContIndex = secondTarget;
                    break;
                }
                else {
                    targetContIndex = firstTarget;
                    break;
                }
            }
            //if 1st target is not contested
            else
                targetContIndex = firstTarget;
        }
        /* testing
        print("First: " + firstTarget);
        print("Second " + secondTarget);
        print("Player " + (playerIndex + 1) + " target is cont index: " + targetContIndex);
        */
    }

    //buils an array (continentTargets) telling each player the best continent to go for at the start of the game
    public void FindContinentControl(int numberOfPlayers) {
        xCounter = 0;
        //build table numberOfPlayers x numberOfContinents all zeros representing prob to target the continent
        controlRatios = new List<float[]>();
        for (int i = 0; i < numberOfPlayers; i++) {
            controlRatios.Add(new float[] { 0, 0, 0, 0, 0, 0 });
        }

        //count how many territories each player has in each continent
        foreach (Transform continentTrans in territories.transform) {
            // tracks number of countries in each continent
            countryCounter = 0;
            foreach(Transform countryTrans in continentTrans) {
                playerIndex = teamChecker.GetPlayer(countryTrans.gameObject) - 1;
                controlRatios[playerIndex][xCounter] = controlRatios[playerIndex][xCounter] + 1;
                countryCounter++;
            }
            // find the % of how much of a contient a player owns
            for(int i = 0; i < numberOfPlayers; i++) {
                controlRatios[i][xCounter] = controlRatios[i][xCounter] / countryCounter;
            }
            // move to next continent
            xCounter++;
        }

        //for each cell in the controlRatio table add the corresponding contDiff value
        //for each player in table
        for (int y = 0; y < numberOfPlayers; y++) {
            //for each continent cell (x100 to keep information for future float -> int conversion)
            for (int x = 0; x < controlRatios[y].Length; x++) {
                controlRatios[y][x] = (controlRatios[y][x] + contDiff[x])*100;
            }
        }
        
        //3dim array 1st/2nd largest x playerIndex x continent index/value - see contDiff array for continent index reference
        continentTargets = new int[2,numberOfPlayers,2];
        //builds continentTargets[i,j,k]
        ContTargetTable(numberOfPlayers);
    }

    //builds the continent target table
    void ContTargetTable(int numberOfPlayers) {
        for (int j = 0; j < numberOfPlayers; j++) {
            //Larest value holders
            largestValIndex = 0;
            secondLargestValIndex = 1;
            // x100 to keep information after converting float -> int
            largestValue = (int)controlRatios[j][0];
            secondLargestValue = (int)controlRatios[j][1];

            //check each continent value for a given player index
            for (int i = 1; i < controlRatios[j].Length; i++) {
                //picks largest values
                if (controlRatios[j][i] > largestValue) {
                    secondLargestValIndex = largestValIndex;
                    secondLargestValue = largestValue;
                    largestValIndex = i;
                    largestValue = (int)controlRatios[j][i];
                }
            }
            //input values into table
            continentTargets[0, j, 0] = largestValIndex;
            continentTargets[1, j, 0] = secondLargestValIndex;
            continentTargets[0, j, 1] = largestValue;
            continentTargets[1, j, 1] = secondLargestValue;
        }

        /* print for testing
        for (int j = 0; j < numberOfPlayers; j++) {
            for (int i = 0; i < 2; i++) {
                for (int k = 0; k < 2; k++) {
                    print("Player "+j+" i,k: "+i+", "+k+": "+continentTargets[i, j, k]);
                }
            }
        }
        */
    }




}
