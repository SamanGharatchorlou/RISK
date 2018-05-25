using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// rolls a given number of dice and compares winners/loosers
public class DiceRoll : MonoBehaviour {

	public List<int> attackerValues;
	List<int> defenderValues;
	List<int> rollValues;

	int roll;

	// Roll dice
	List<int> Roll(int repeat){
		rollValues = new List<int> ();
		for (int i = 0; i < repeat; i++) {
			roll = Mathf.FloorToInt (Random.Range (1f, 7f));
			rollValues.Add (roll);
		}
		return rollValues;
	}

	// Input attacking & defending army sizes. Outputs number of dead attackers and defenders.
	public void CalculateBattle(int attackers, int defenders, out int deadAttackers, out int deadDefenders){
		deadAttackers = 0;
		deadDefenders = 0;

		// Roll for attackers
		if (attackers > 3)
			attackers = 3;
		attackerValues = new List<int> (Roll (attackers));

		// Roll for defenders
		if (defenders > 2)
			defenders = 2;
		defenderValues = new List<int> (Roll (defenders));

		attackerValues.Sort ();
		defenderValues.Sort ();

		// Calculates how many dead attackers and defenders
		for (int i = 1; i <= defenders & i<=attackers; i++) {

			// breaks script if all attackers are dead
			if (attackers == deadAttackers)
				break;

			if (attackerValues [attackers - i] > defenderValues [defenders - i])
				deadDefenders = deadDefenders + 1;

			else
				deadAttackers = deadAttackers + 1;
		}
	}

}
		