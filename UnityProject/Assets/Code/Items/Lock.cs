using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Lock: MonoBehaviour
	{
		public string keyId;
		public Activatable locked;
		public bool consumeItem = true;
		
		void OnMouseUpAsButton() {
			if (GameState.getInstance().has(InventoryRenderer.ACTIVE_ITEM_GAME_STATE_KEY)) {
				string item = (string) GameState.getInstance().get(InventoryRenderer.ACTIVE_ITEM_GAME_STATE_KEY);
				if (item != null && item.Equals(keyId)) {
					locked.setActive(true);
					
					if (consumeItem) {
						GameState.getInstance().removeItem(keyId);
					}
				}
			}	
		}
	}
}

