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
	
	private static readonly float saveWaitTime = 30; //save every x seconds
	
	public static bool allowTurning = true;
	
	private Boolean showEscapeMenu = false;
	private float prevSaveTime;
	private Rect leftTurnButton;
	private Rect rightTurnButton;
	
	private Texture2D leftCursor;
	private Texture2D rightCursor;
	private Vector2 leftHotSpot;
	private Vector2 rightHotSpot;
	
	// Debug stuff
	private static readonly Boolean debug = true;
	private static readonly Rect screenRect = new Rect(10, 10, 200, 100);
	public static string debugText = "...";
	
	// Use this for initialization
	void Start () {
		prevSaveTime = Time.time;
		float turnButtonWidth = Screen.width * 0.06f;
		leftTurnButton = new Rect(0, 0, turnButtonWidth, Screen.height);
		rightTurnButton = new Rect(Screen.width - turnButtonWidth, 0, turnButtonWidth, Screen.height);
		
		leftCursor = Resources.Load("Cursors/Turn Left") as Texture2D;
		rightCursor = Resources.Load("Cursors/Turn Right") as Texture2D;
		leftHotSpot = new Vector2(0, 9);
		rightHotSpot = new Vector2(22, 9);
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
		
		Vector2 mouseP = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		if (isTurningEnabled() && leftTurnButton.Contains(mouseP)) {
			CursorManager.takeCursorFocus(this, leftCursor, leftHotSpot);
		} else if (isTurningEnabled() && rightTurnButton.Contains(mouseP)) {
			CursorManager.takeCursorFocus(this, rightCursor, rightHotSpot);
		} else {
			CursorManager.giveUpCursorFocus(this);
		}
	}
	
	void OnGUI() {
		// Turning buttons
		if (isTurningEnabled()) {
			if (GUI.Button(leftTurnButton, "", GUIStyle.none)) {
				Location.activeLocation.turnLeft();
			} else if (GUI.Button(rightTurnButton, "", GUIStyle.none)) {
				Location.activeLocation.turnRight();
			}
		}
		
		// Pause menu
		if (showEscapeMenu) {
			int menuWidth = Screen.width / 3;
			int menuHeight = (int) (Screen.height * 0.6);
			int x = (Screen.width - menuWidth) / 2;
			int y = (Screen.height - menuHeight) / 2;
			
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", GUIStyle.none);			
			GUILayout.Window(0, new Rect(x, y, menuWidth, menuHeight), layoutEscapeMenu, "Menu");
		}
		
		if (debug) {
    		GUI.Label(screenRect, debugText);
		}
	}
	
	void layoutEscapeMenu(int menuID) {
		if (GUILayout.Button("Main Menu")) {
			GameState.saveCurrentGame();
			Application.LoadLevel("MenuScene");
		}
		
		if (GUILayout.Button("Exit")) {
			GameState.saveCurrentGame();
			Application.Quit();
		}
		
		if (GUILayout.Button("Resume")) {
			showEscapeMenu = false;
		}
	}
	
	private bool isTurningEnabled() {
		return Location.activeLocation != null && !showEscapeMenu && allowTurning;
	}
}
