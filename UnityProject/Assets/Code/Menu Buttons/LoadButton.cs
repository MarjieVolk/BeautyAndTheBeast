using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class LoadButton : MonoBehaviour {
	
	public GUISkin skin;
	
	private Boolean showLoadMenu = false;
	private string[] saves;
	private int selected = 0;
	private Vector2 scrollPos = new Vector2(0, 0);
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown () {
		showLoadMenu = true;
		saves = GameState.getSavedGames();
	}
	
	private void loadGame(string gameName) {
		GameState.loadGame(gameName);
    	Application.LoadLevel("Library");
	}
	
	void OnGUI() {
		GUI.skin = skin;
		
		if (showLoadMenu) {
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", GUIStyle.none);
			
			int menuWidth = Screen.width / 3;
			int menuHeight = (int) (Screen.height * 0.9);
			int x = (Screen.width - menuWidth) / 2;
			int y = (Screen.height - menuHeight) / 2;
			GUILayout.Window(0, new Rect(x, y, menuWidth, menuHeight), layoutLoadWindow, "Load Game");
		}
	}
	
	void layoutLoadWindow(int windowID) {
		GUILayout.BeginScrollView(scrollPos, false, true);
		selected = GUILayout.SelectionGrid(selected, saves, 1);
		GUILayout.EndScrollView();
		
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Load")) {
			loadGame(saves[selected]);
		}
		
		if (GUILayout.Button("Delete")) {
			GameState.deleteGame(saves[selected]);
			saves = GameState.getSavedGames();
		}
		
		if (GUILayout.Button("Cancel")) {
			showLoadMenu = false;
		}
		
		GUILayout.EndHorizontal();
	}
}
