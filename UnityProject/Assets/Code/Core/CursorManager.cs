using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class CursorManager
	{
		
		private static object hasCursorFocus = null;
		
		public static void takeCursorFocus(object obj, Texture2D cursorTexture, Vector2 hotSpot) {
			hasCursorFocus = obj;
			Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
		}
		
		public static void giveUpCursorFocus(object obj) {
			if (hasCursorFocus == obj) {
				hasCursorFocus = null;
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			}
		}
	}
}

