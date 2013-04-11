using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class ClickToPickUp : MonoBehaviour {
	
	public String itemId;
	public String gameStateKey = "";
	
	// Use this for initialization
	void Start () {
		if (!GameState.getInstance().has(getKey()))
			GameState.getInstance().put(getKey(), true);
		
		if (!((bool) GameState.getInstance().get(getKey())))
			Destroy(transform.root.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseUpAsButton() {
		GameState.getInstance().addItem(itemId);
		GameState.getInstance().put(getKey(), false);
		Destroy(transform.root.gameObject);
	}
	
	private String getKey() {
		String key = "pickUp." + itemId;
		if (gameStateKey.Length > 0)
			key += "." + gameStateKey;
		
		return key;
	}
}
