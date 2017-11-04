﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour {
	private EnableChocolateOnTrigger firstSmore;
	private EnableChocolateOnTrigger secondSmore;
	private bool hasFinishedlevel = false;
	private int numUniqueMellowsFinished = 0;

	void Start() {
		EnableChocolateOnTrigger[] smores = GetComponentsInChildren<EnableChocolateOnTrigger> ();
		if (smores.Length == 2) {
			firstSmore = smores [0];
			firstSmore.registeredMellow += AddMellow;
			firstSmore.lostMellow += SubtractMellow;

			secondSmore = smores [1];
			secondSmore.registeredMellow += AddMellow;
			secondSmore.lostMellow += SubtractMellow;
		}
	}

	void AddMellow() {
		numUniqueMellowsFinished += 1;
		if (numUniqueMellowsFinished == 2) {
			CheckFinishCondition ();
		}
	}

	void SubtractMellow() {
		numUniqueMellowsFinished -= 1;
	}

	void CheckFinishCondition() {
		if (!hasFinishedlevel && firstSmore.mellowName != secondSmore.mellowName) {
			Invoke ("BackToMenu", 3.0f);

			hasFinishedlevel = true;
			firstSmore.EnableChocolate ();
			secondSmore.EnableChocolate ();
		}
	}

    private void BackToMenu() {
        SceneLoader.instance.LoadMenu();
    }

}
