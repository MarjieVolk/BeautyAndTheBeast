using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class NewButton : MonoBehaviour {
	
	private bool showNewMenu = false;
	private bool showConfirmMenu = false;
	private String gameName = "game";
	
	private Rect menuRect;
	
	// Use this for initialization
	void Start () {
		int menuWidth = (int) (Screen.width * 0.2);
		int menuHeight = (int) (Screen.height * 0.3);
		int x = (Screen.width - menuWidth) / 2;
		int y = (Screen.height - menuHeight) / 2;
		menuRect = new Rect(x, y, menuWidth, menuHeight);
		
		string[] games = GameState.getSavedGames();
		if (contains(games, gameName)) {
			int i = 1;
			while(contains(games, gameName + i)) {
				i++;
			}
			gameName += i;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown () {
		showNewMenu = true;
	}
	
	void OnGUI() {
		if (showNewMenu || showConfirmMenu)
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", GUIStyle.none);
		
		if (showNewMenu) {
			GUILayout.Window(0, menuRect, layoutNewWindow, "New Game");
		}
		
		if (showConfirmMenu) {
			GUILayout.Window(0, menuRect, layoutConfirmWindow, "Overrwrite?");
		}
	}
	
	void layoutNewWindow(int windowID) {
		gameName = GUILayout.TextField(gameName);
		
		GUILayout.BeginHorizontal();
		 
		if (GUILayout.Button("Start")) {
			showNewMenu = false;
			
			foreach (string save in GameState.getSavedGames()) {
				if (gameName.Equals(save)) {
					showConfirmMenu = true;
					return;
				}
			}
			
			start();
		}
		
		if (GUILayout.Button("Cancel")) {
			showNewMenu = false;
		}
		
		GUILayout.EndHorizontal();
	}
	
	void layoutConfirmWindow(int windowID) {
		GUILayout.Label("There is already a game named \"" + gameName + "\".  Overrwite?");
		
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Continue")) {
			start();
		}
		
		if (GUILayout.Button("Cancel")) {
			showConfirmMenu = false;
			showNewMenu = true;
		}
		
		GUILayout.EndHorizontal();
	}
	
	private void start() {
		GameState.startNewGame(gameName);
    	Application.LoadLevel("Library");
	}
	
	private bool contains(string[] array, string val) {
		foreach (string s in array) {
			if (s.Equals(val))
				return true;
		}
		
		return false;
	}
}
