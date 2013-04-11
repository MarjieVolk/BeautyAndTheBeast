using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class InventoryRenderer : MonoBehaviour, GameStateListener {
	
	public Texture inventoryBackground;
	public float percentScreenHeight = 0.13f;
	
	private Texture[] textures = new Texture[GameState.INVENTORY_SIZE];
	private Rect backgroundRect;
	
	void Start () {
		GameState.getInstance().addListener(this);
		
		textures = getTextures((String[]) GameState.getInstance().getInventory());
		
		float height = Screen.height * percentScreenHeight;
		float width = inventoryBackground.width * (height / inventoryBackground.height);
		float x = (Screen.width - width) / 2;
		float y = Screen.height - height;
		
		backgroundRect = new Rect(x, y, width, height);
	}
	
	void OnGUI() {
		GUI.DrawTexture(backgroundRect, inventoryBackground);
		
		for (int i = 0; i < textures.Length; i++) {
			Texture texture = textures[i];
			if (texture == null)
				continue;
			
			// Find item slot bounds
			float boundWidth = backgroundRect.width / textures.Length;
			float boundX = backgroundRect.x + (i * boundWidth);
			float boundY = backgroundRect.y;
			float boundHeight = backgroundRect.height;
			
			int width = texture.width;
			int height = texture.height;
			
			float aspect = width / height;
			float boundAspect = boundWidth / boundHeight;
			
			// Scale texture to fit inside bounds
			if (aspect > boundAspect) {
				// Restrict by width
				float scaledHeight = height * (boundWidth / width);
				boundY = boundY + ((boundHeight - scaledHeight) / 2);
				boundHeight = scaledHeight;
			} else {
				// Restrict by height
				float scaledWidth = width * (boundHeight / height);
				boundX = boundX + ((boundWidth - scaledWidth) / 2);
				boundWidth = scaledWidth;
			}
			
			Rect itemRect = new Rect(boundX, boundY, boundWidth, boundHeight);
			GUI.DrawTexture(itemRect, texture);
		}
	}
	
	public void stateChanged(string stateKey, object oldValue, object newValue) {
		if (stateKey.Equals(GameState.invKey)) {
			textures = getTextures((string[]) newValue);
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
