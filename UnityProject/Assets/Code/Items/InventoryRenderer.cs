using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

[RequireComponent(typeof(AudioSource))]
public class InventoryRenderer : MonoBehaviour, GameStateListener {
	
	private static readonly float BACK_BUTTON_WIDTH = 50;
	private static readonly float BACK_BUTTON_HEIGHT = 30;
	private static readonly float BACK_BUTTON_MARGIN = 20;
	
	private static readonly float OPEN_SPEED = 320;
	private static readonly float TIME_BEFORE_CLOSE = 0.3f;
	
	public static readonly String ACTIVE_ITEM_GAME_STATE_KEY = "inventory.active";
	
	public Texture inventoryBackground;
	public Texture selectedImage;
	public float percentScreenHeight = 0.13f;
	public float topMargin = 0;
	public float leftMargin = 0;
	public float rightMargin = 0;
	public float slotMargin = 0;
	
	private Texture[] textures;
	private string[] ids;
	private Rect backgroundRect;
	private int activeItem = -1;
	
	private GameObject closeUp = null;
	private int lastClicked = -1;
	private float timeOfLastClick = -1;
	
	private float xAxis = 0;
	private float yAxis = 0;
	
	private bool isOpen = false;
	private float openY;
	private float closedY;
	private float timeOfLastHover = 0;
	
	void Start () {
		GameState.getInstance().addListener(this);
		
		ids = (String[]) GameState.getInstance().getInventory();
		textures = getTextures(ids);
		
		float scale = (Screen.height * percentScreenHeight) / (inventoryBackground.height - topMargin);
		
		float height = inventoryBackground.height * scale;
		float width = inventoryBackground.width * scale;
		float x = (Screen.width - width) / 2;
		
		topMargin *= scale;
		leftMargin *= scale;
		rightMargin *= scale;
		
		openY = Screen.height - height;
		closedY = Screen.height - topMargin;
		
		backgroundRect = new Rect(x, closedY, width, height);
	}
	
	void Update() {
		if (Input.GetMouseButton(0) && closeUp != null) {
			xAxis += Input.GetAxis("Mouse Y") * 15;
			yAxis -= Input.GetAxis("Mouse X") * 15;
			closeUp.transform.rotation = Quaternion.Euler(xAxis, yAxis, 0);
		}
		
		Vector3 mouseP = Input.mousePosition;
		mouseP.y = Screen.height - mouseP.y;
		if (backgroundRect.Contains(mouseP)) {
			isOpen = true;
			timeOfLastHover = Time.time;
		} else {			
			if ((Time.time - timeOfLastHover) >= TIME_BEFORE_CLOSE && activeItem == -1)
				isOpen = false;
		}
		
		if (isOpen) {
			if (backgroundRect.y > openY) {
				// It should be open, but its not all the way out yet -- keep pulling it out
				backgroundRect.y = backgroundRect.y - OPEN_SPEED * Time.deltaTime;
			}
			
			if (backgroundRect.y < openY)
				backgroundRect.y = openY;
			
		} else {
			if (backgroundRect.y < closedY) {
				// It should be closed, but its not all the way back yet -- keep pushing it in
				backgroundRect.y = backgroundRect.y + OPEN_SPEED * Time.deltaTime;
			}
			
			if (backgroundRect.y > closedY)
				backgroundRect.y = closedY;
		}
	}
	
	void OnGUI() {
		GUI.DrawTexture(backgroundRect, inventoryBackground);
		
		// Highlight active item in inventory
		if (activeItem >= 0 && activeItem < textures.Length && textures[activeItem] != null) {
			GUI.DrawTexture(getBounds(activeItem), selectedImage);
			GameState.getInstance().put(ACTIVE_ITEM_GAME_STATE_KEY, ids[activeItem]);
		} else {
			activeItem = -1;
			GameState.getInstance().put(ACTIVE_ITEM_GAME_STATE_KEY, null);
		}
		
		// For each item in inventory
		for (int i = 0; i < textures.Length; i++) {
			Texture texture = textures[i];
			if (texture == null)
				continue;
			
			// Draw texture...
			
			// Find item slot bounds
			Rect itemRect = getBoundsMinusMargin(i);
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
			if (GUI.Button(getBounds(i), "", GUIStyle.none)) {	
				audio.Play();
				
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
		
		// If showing close-up, show back button & prevent all other interaction
		if (closeUp != null) {
			if (GUI.Button(new Rect((Screen.width - BACK_BUTTON_WIDTH) / 2.0f, BACK_BUTTON_MARGIN, BACK_BUTTON_WIDTH, BACK_BUTTON_HEIGHT), "Back")) {
				GeneralRoomFeaturesScript.allowTurning = true;
				Destroy(closeUp);
				closeUp = null;
			} else {
				GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", GUIStyle.none);
			}
		}
	}
	
	private Rect getBounds(int i) {
		float width = (backgroundRect.width - leftMargin - rightMargin) / textures.Length;
		float x = backgroundRect.x + leftMargin + (i * width);
		float y = backgroundRect.y + topMargin;
		float height = backgroundRect.height - topMargin;
		return new Rect(x, y, width, height);
	}
	
	private Rect getBoundsMinusMargin(int i) {
		Rect r = getBounds(i);
		float x = r.x + slotMargin;
		float y = r.y + slotMargin;
		float width = r.width - (slotMargin * 2);
		float height = r.height - (slotMargin * 2);
		return new Rect(x, y, width, height);
	}
	
	public void stateChanged(string stateKey, object oldValue, object newValue) {
		if (stateKey.Equals(GameState.invKey)) {
			ids = (string[]) newValue;
			textures = getTextures(ids);
			isOpen = true;
			timeOfLastHover = Time.time + 0.5f;
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
