using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class ClickToPickUp : MonoBehaviour {
	
	public String itemId;
	public String gameStateKey = "";
	
	private AudioSource pickUpSound;
	private Texture2D cursor;
	
	// Use this for initialization
	void Start () {
		if (!GameState.getInstance().has(getKey()))
			GameState.getInstance().put(getKey(), true);
		
		if (!((bool) GameState.getInstance().get(getKey())))
			Destroy(transform.gameObject);
		
		GameObject obj = new GameObject();
		obj.transform.position = this.transform.position;
		pickUpSound = obj.AddComponent(typeof(AudioSource)) as AudioSource;
		pickUpSound.clip = Resources.Load("Sound Effects/Pick Up") as AudioClip;
		
		cursor = Resources.Load("Cursors/Tap") as Texture2D;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseEnter() {
		Cursor.SetCursor(cursor, new Vector2(6, 1), CursorMode.Auto);
	}
	
	void OnMouseExit() {
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
	
	void OnMouseUpAsButton() {
		GameState.getInstance().addItem(itemId);
		GameState.getInstance().put(getKey(), false);
		
		pickUpSound.Play();
		
		Destroy(transform.gameObject);
	}
	
	private String getKey() {
		String key = "pickUp." + itemId;
		if (gameStateKey.Length > 0)
			key += "." + gameStateKey;
		
		return key;
	}
}
