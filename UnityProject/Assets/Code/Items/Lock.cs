using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Lock: MonoBehaviour
	{
		public string keyId;
		public Activatable locked;
		public bool consumeItem = true;
		public AudioClip unlockSound = null;
		public string gameStateKey = null;
		
		private AudioSource lockAudio;
		
		void Awake() {
			if (unlockSound == null)
				unlockSound = Resources.Load("Sound Effects/Lock") as AudioClip;
			
			lockAudio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			
			if (!GameState.getInstance().has(getKey()))
				GameState.getInstance().put(getKey(), false);
			
			locked.setActive((bool) GameState.getInstance().get(getKey()));
		}
		
		void OnMouseUpAsButton() {
			if ((bool) GameState.getInstance().get(getKey()))
				return;
			
			if (GameState.getInstance().has(InventoryRenderer.ACTIVE_ITEM_GAME_STATE_KEY)) {
				string item = (string) GameState.getInstance().get(InventoryRenderer.ACTIVE_ITEM_GAME_STATE_KEY);
				
				if (item != null && item.Equals(keyId)) {
					GameState.getInstance().put(getKey(), true);
					locked.setActive(true);
					
					lockAudio.clip = unlockSound;
					lockAudio.Play();
					
					if (consumeItem) {
						GameState.getInstance().removeItem(keyId);
					}
				}
			}	
		}
		
		public string getKey() {
			return "lock." + keyId + (gameStateKey == null ? "" : "." + gameStateKey);
		}
	}
}

