﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDirectional : MonoBehaviour {

	private int playerID = 0;
    private PlayerDeviceManager deviceManager;
    private float deadZone = 0.2f;
    private PlayerActions controls;

    // Use this for initialization
    void Awake () {
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();
		if (deviceManager != null)
		{
			controls = deviceManager.GetControls(playerID);
		}

		if (GetComponentInParent<StateMachineForJack> () != null) {
			playerID = GetComponentInParent<StateMachineForJack> ().GetPlayerID();
		}
    }

    public float GetCurrentHorzAxis() {
		if (deviceManager != null)
		{
			controls = deviceManager.GetControls(playerID);
		}

        if (controls != null && Mathf.Abs(controls.Move.X) > deadZone) {
            return controls.Move.X;
        }
        return 0.0f;
    }

    public float GetCurrentVertAxis() {
		if (deviceManager != null)
		{
			controls = deviceManager.GetControls(playerID);
		}
		
        if (controls != null && Mathf.Abs(controls.Move.Y) > deadZone) {
            return controls.Move.Y;
        }
        return 0.0f;
    }
}
