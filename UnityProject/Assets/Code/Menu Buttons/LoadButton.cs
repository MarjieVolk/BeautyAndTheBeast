using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class LoadButton : MonoBehaviour {
	
	public GUISkin skin;
	
	private Boolean showLoadMenu = false;
	private string[] saves;
	private int selected = 0;
	
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
    	Application.LoadLevel ("CubeScene");
	}
	
	void OnGUI() {
		GUI.skin = skin;
		
		if (showLoadMenu) {
			int menuWidth = Screen.width / 3;
			int menuHeight = (int) (Screen.height * 0.9);
			int x = (Screen.width - menuWidth) / 2;
			int y = (Screen.height - menuHeight) / 2;
			GUI.Window(0, new Rect(x, y, menuWidth, menuHeight), layoutLoadWindow, "Load Game");
		}
	}
	
	void layoutLoadWindow(int windowID) {
		selected = GUI.SelectionGrid(new Rect(60, 40, 200, 300), selected, saves, 1);
		
		if (GUI.Button(new Rect(60, 400, 50, 20), "Load")) {
			loadGame(saves[selected]);
		}
		
		if (GUI.Button(new Rect(130, 400, 60, 20), "Cancel")) {
			showLoadMenu = false;
		}
	}
}
