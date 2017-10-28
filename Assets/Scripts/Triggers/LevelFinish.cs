﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour {

    public SpriteRenderer firstFlag;
    public SpriteRenderer secondFlag;
    public Sprite greenFlagSprite;
    public SceneLoader sceneLoader;

    private GameObject firstPlayer;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<MellowStates>() != null) {
            if (firstPlayer == null) {
                firstPlayer = other.gameObject;
                firstFlag.sprite = greenFlagSprite;
            }
            else if (other.gameObject != firstPlayer) {
                secondFlag.sprite = greenFlagSprite;
                Invoke("BackToMenu", 3.0f);
            }
        }
    }

    private void BackToMenu() {
        sceneLoader.LoadMenu();
    }

}
