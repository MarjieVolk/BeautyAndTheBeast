using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Item
	{
		private static Dictionary<String, ItemData> items = new Dictionary<String, ItemData>();
		
		public static void registerItem(String id, Texture invTexture, GameObject model) {
			if (items.ContainsKey(id))
				throw new Exception("Cannot have two items with the same id (" + id + ")");
			
			items[id] = new ItemData(invTexture, model);
		}
		
		public static Texture getItemTexture(String id) {
			if (!items.ContainsKey(id))
				throw new Exception("No item with id \"" + id + "\"");
			
			return items[id].invTexture;
		}
		
		public static GameObject getItemModel(String id) {
			if (!items.ContainsKey(id))
				throw new Exception("No item with id \"" + id + "\"");
			
			return items[id].model;
		}
		
		static Item() {
			registerItem("key.water", Resources.Load("Inventory Textures/Key - Water") as Texture,
				Resources.Load("Models/Key - Water") as GameObject);
			registerItem("key.fire", Resources.Load("Inventory Textures/Key - Fire") as Texture,
				Resources.Load("Models/Key - Fire") as GameObject);
			registerItem("key.earth", Resources.Load("Inventory Textures/Key - Earth") as Texture,
				Resources.Load("Models/Key - Earth") as GameObject);
			registerItem("key.air", Resources.Load("Inventory Textures/Key - Air") as Texture,
				Resources.Load("Models/Key - Air") as GameObject);
			registerItem("roseDoorKey.sun", Resources.Load("Inventory Textures/Rose Door Key - Sun") as Texture,
				Resources.Load("Models/Rose Door Key - Sun") as GameObject);
		}
		
		private class ItemData {
			public readonly Texture invTexture;
			public readonly GameObject model;
			
			public ItemData(Texture invTexture, GameObject model) {
				this.invTexture = invTexture;
				this.model = model;
			}
		}
	}
}

