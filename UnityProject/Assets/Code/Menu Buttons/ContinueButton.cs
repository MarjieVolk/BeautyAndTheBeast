using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ContinueButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown () {
		GameState.loadLastPlayedGame();
    	Application.LoadLevel("CubeScene");
	}
}
