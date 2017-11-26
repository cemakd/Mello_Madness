﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour {
	private EnableChocolateOnTrigger firstSmore;
	private EnableChocolateOnTrigger secondSmore;
	private bool hasFinishedlevel = false;
	private int numUniqueMellowsFinished = 0;

    public GameObject finishPanel;
    public AudioSource finishSound;
    public AudioSource mainTheme;
    public GameObject candyWave;
    private GameObject firstPlayer;

    //void OnTriggerEnter2D(Collider2D other) {
    //    if (other.gameObject.GetComponent<MellowStates>() != null) {
    //        if (firstPlayer == null) {
    //            firstPlayer = other.gameObject;
    //            firstFlag.sprite = greenFlagSprite;
    //        }
    //        else if (other.gameObject != firstPlayer) {
    //            secondFlag.sprite = greenFlagSprite;

    //            Invoke("BackToMenu", 3.0f);
    //        }
    //    }
    //}

    private void Awake()
    {
        if (mainTheme == null || finishSound == null || candyWave == null) {
            var cam = GameObject.Find("GameCamera");
            mainTheme = cam.GetComponent<AudioSource>();
            finishSound = cam.transform.Find("SFX").Find("Finish").GetComponent<AudioSource>();
            candyWave = cam.transform.Find("CandyWave").gameObject;
        }
        
    }

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
            finishPanel.SetActive(true);
            mainTheme.Pause();
            finishSound.Play();
            StartCoroutine("StartDecay");
        }
	}

    private IEnumerator StartDecay() {
        yield return new WaitForSeconds(1);
        candyWave.GetComponent<ReverseDecay>().ReverseWaveDecay();
    }

    private void BackToMenu() {
        SceneLoader.instance.LevelCompleted();
        SceneLoader.instance.LoadMenu();
    }

}
