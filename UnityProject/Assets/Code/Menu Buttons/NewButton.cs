using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class NewButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown () {
		//TODO: get new game name from user
		string newGameName = "game";
		
		foreach (string save in GameState.getSavedGames()) {
			if (newGameName.Equals(save)) {
				//TODO: print file overwrite warning
			}
		}
		
		GameState.startNewGame(newGameName);
    	Application.LoadLevel ("CubeScene");
	}
	
}
