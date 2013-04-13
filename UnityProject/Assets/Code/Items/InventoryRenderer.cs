using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class InventoryRenderer : MonoBehaviour, GameStateListener {
	
	public static readonly String ACTIVE_ITEM_GAME_STATE_KEY = "inventory.active";
	
	public Texture inventoryBackground;
	public float percentScreenHeight = 0.13f;
	
	private Texture[] textures = new Texture[GameState.INVENTORY_SIZE];
	private string[] ids = new string[GameState.INVENTORY_SIZE];
	private Rect backgroundRect;
	private int activeItem = -1;
	
	void Start () {
		GameState.getInstance().addListener(this);
		
		ids = (String[]) GameState.getInstance().getInventory();
		textures = getTextures(ids);
		
		float height = Screen.height * percentScreenHeight;
		float width = inventoryBackground.width * (height / inventoryBackground.height);
		float x = (Screen.width - width) / 2;
		float y = Screen.height - height;
		
		backgroundRect = new Rect(x, y, width, height);
	}
	
	void OnGUI() {
		GUI.DrawTexture(backgroundRect, inventoryBackground);
		
		// For each item in inventory
		for (int i = 0; i < textures.Length; i++) {
			Texture texture = textures[i];
			if (texture == null)
				continue;
			
			// Draw texture...
			
			// Find item slot bounds
			Rect itemRect = getBounds(i);
			float boundWidth = itemRect.width;
			float boundX = itemRect.x;
			float boundY = itemRect.y;
			float boundHeight = itemRect.height;
			
			// Scale texture to fit inside bounds
			if (texture.width / texture.height > boundWidth / boundHeight) {
				// Restrict by width
				float scaledHeight = texture.height * (boundWidth / texture.width);
				boundY = boundY + ((boundHeight - scaledHeight) / 2);
				boundHeight = scaledHeight;
			} else {
				// Restrict by height
				float scaledWidth = texture.width * (boundHeight / texture.height);
				boundX = boundX + ((boundWidth - scaledWidth) / 2);
				boundWidth = scaledWidth;
			}
			
			GUI.DrawTexture(new Rect(boundX, boundY, boundWidth, boundHeight), texture);
			
			// Listen for activation
			if (GUI.Button(itemRect, "", GUIStyle.none)) {
				activeItem = (activeItem == i) ? -1 : i;
			}
		}
		
		// Do special thing for active one
		if (activeItem >= 0 && activeItem < textures.Length && textures[activeItem] != null) {
			GUI.Box(getBounds(activeItem), "");
			GameState.getInstance().put(ACTIVE_ITEM_GAME_STATE_KEY, ids[activeItem]);
		} else {
			activeItem = -1;
			GameState.getInstance().put(ACTIVE_ITEM_GAME_STATE_KEY, null);
		}
	}
	
	private Rect getBounds(int i) {
		float width = backgroundRect.width / textures.Length;
		float x = backgroundRect.x + (i * width);
		float y = backgroundRect.y;
		float height = backgroundRect.height;
		return new Rect(x, y, width, height);
	}
	
	public void stateChanged(string stateKey, object oldValue, object newValue) {
		if (stateKey.Equals(GameState.invKey)) {
			ids = (string[]) newValue;
			textures = getTextures(ids);
		}
	}
	
	private Texture[] getTextures(string[] ids) {
		Texture[] textures = new Texture[ids.Length];
		for (int i = 0; i < textures.Length; i++) {
			textures[i] = ids[i] == null ? null : Item.getItemTexture(ids[i]);
		}
		
		return textures;
	}
}
