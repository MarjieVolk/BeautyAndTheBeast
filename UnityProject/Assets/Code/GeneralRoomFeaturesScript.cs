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
	 */
	
	private static readonly float saveWaitTime = 5; //save every x seconds
	
	private Boolean showEscapeMenu = false;
	private float prevSaveTime;
	
	// Use this for initialization
	void Start () {
		prevSaveTime = Time.time;
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
		if (showEscapeMenu) {
			int menuWidth = Screen.width / 3;
			int menuHeight = (int) (Screen.height * 0.9);
			int x = (Screen.width - menuWidth) / 2;
			int y = (Screen.height - menuHeight) / 2;
			GUI.Window(0, new Rect(x, y, menuWidth, menuHeight), layoutEscapeMenu, "Menu");
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
