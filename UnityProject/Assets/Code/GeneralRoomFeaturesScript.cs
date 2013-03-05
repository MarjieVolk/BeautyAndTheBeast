using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class GeneralRoomFeaturesScript : MonoBehaviour {
	
	/*
	 * Handled in this class:
	 * 
	 * - Escape menu
	 * - Autosave
	 * - Turning buttons
	 * - Debug
	 */
	
	private static readonly float saveWaitTime = 5; //save every x seconds
	
	private Boolean showEscapeMenu = false;
	private float prevSaveTime;
	private Rect leftTurnButton;
	private Rect rightTurnButton;
	
	// Debug stuff
	private static readonly Boolean debug = true;
	private static readonly Rect screenRect = new Rect(10, 10, 200, 100);
	public static string debugText = "...";
	
	// Use this for initialization
	void Start () {
		prevSaveTime = Time.time;
		float turnButtonWidth = Screen.width * 0.1f;
		leftTurnButton = new Rect(0, 0, turnButtonWidth, Screen.height);
		rightTurnButton = new Rect(Screen.width - turnButtonWidth, 0, turnButtonWidth, Screen.height);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("escape")) {
			showEscapeMenu = true;
		}
		
		if (Time.time >= prevSaveTime + saveWaitTime) {
			GameState.saveCurrentGame();
			prevSaveTime = Time.time;
		}
	}
	
	void OnGUI() {
		// Turning buttons
		if (Location.activeLocation != null) {
			if (GUI.Button(leftTurnButton, "turn left", GUIStyle.none)) {
				Location.activeLocation.turnLeft();
			} else if (GUI.Button(rightTurnButton, "turn right", GUIStyle.none)) {
				Location.activeLocation.turnRight();
			}
		}
		
		if (showEscapeMenu) {
			int menuWidth = Screen.width / 3;
			int menuHeight = (int) (Screen.height * 0.9);
			int x = (Screen.width - menuWidth) / 2;
			int y = (Screen.height - menuHeight) / 2;
			GUI.Window(0, new Rect(x, y, menuWidth, menuHeight), layoutEscapeMenu, "Menu");
		}
		
		if (debug) {
    		GUI.Label(screenRect, debugText);
		}
	}
	
	void layoutEscapeMenu(int menuID) {
		if (GUI.Button(new Rect(80, 800, 80, 20), "Main Menu")) {
			Application.LoadLevel("MenuScene");
		}
		
		if (GUI.Button(new Rect(80, 800, 120, 20), "Exit")) {
			Application.Quit();
		}
	}
}
