using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class InventoryRenderer : MonoBehaviour, GameStateListener {
	
	private static readonly float BUTTON_WIDTH = 50;
	private static readonly float BUTTON_HEIGHT = 30;
	private static readonly float BUTTON_MARGIN = 20;
	
	public static readonly String ACTIVE_ITEM_GAME_STATE_KEY = "inventory.active";
	
	public Texture inventoryBackground;
	public float percentScreenHeight = 0.13f;
	
	private Texture[] textures = new Texture[GameState.INVENTORY_SIZE];
	private string[] ids = new string[GameState.INVENTORY_SIZE];
	private Rect backgroundRect;
	private int activeItem = -1;
	
	private GameObject closeUp = null;
	private int lastClicked = -1;
	private float timeOfLastClick = -1;
	
	private float xAxis = 0;
	private float yAxis = 0;
	
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
	
	void Update() {
		if (Input.GetMouseButton(0) && closeUp != null) {
			xAxis += Input.GetAxis("Mouse Y") * 15;
			yAxis -= Input.GetAxis("Mouse X") * 15;
			closeUp.transform.rotation = Quaternion.Euler(xAxis, yAxis, 0);
		}
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
			
			// Listen for mouse events
			if (GUI.Button(itemRect, "", GUIStyle.none)) {				
				if (lastClicked == i && Time.time - timeOfLastClick < 0.3f) {
					// Double click: open item close-up
					activeItem = i;
					if (closeUp != null) {
						Destroy(closeUp);
						closeUp = null;
					}
					
					Transform camera = CameraController.instance.camera.transform;
					Vector3 pos = camera.position + (camera.forward * 0.4f);
					closeUp = (GameObject) Instantiate(Item.getItemModel(ids[i]), pos, camera.rotation);
					xAxis = closeUp.transform.rotation.eulerAngles.x;
					yAxis = closeUp.transform.rotation.eulerAngles.y;
					
					GeneralRoomFeaturesScript.allowTurning = false;
					
				} else {
					// Single click: activate/deactivate item
					activeItem = (activeItem == i) ? -1 : i;
				}
				
				lastClicked = i;
				timeOfLastClick = Time.time;
			}
		}
		
		// Highlight active item in inventory
		if (activeItem >= 0 && activeItem < textures.Length && textures[activeItem] != null) {
			GUI.Box(getBounds(activeItem), "");
			GameState.getInstance().put(ACTIVE_ITEM_GAME_STATE_KEY, ids[activeItem]);
		} else {
			activeItem = -1;
			GameState.getInstance().put(ACTIVE_ITEM_GAME_STATE_KEY, null);
		}
		
		// If showing close-up, show back button & prevent all other interaction
		if (closeUp != null) {
			if (GUI.Button(new Rect((Screen.width - BUTTON_WIDTH) / 2.0f, BUTTON_MARGIN, BUTTON_WIDTH, BUTTON_HEIGHT), "Back")) {
				GeneralRoomFeaturesScript.allowTurning = true;
				Destroy(closeUp);
				closeUp = null;
			} else {
				GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", GUIStyle.none);
			}
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
