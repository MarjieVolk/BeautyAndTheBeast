using System;

namespace AssemblyCSharp
{
	public class PuzzleCondition
	{
		private String gameStateKey;
		private object val;
		
		public PuzzleCondition(String gameStateKey, object val)
		{
			this.gameStateKey = gameStateKey;
			this.val = val;
		}
		
		public String getKey() {
			return gameStateKey;
		}
		
		public bool isMet() {
			if (!GameState.getInstance().has(gameStateKey))
				return false;
			
			return val.Equals(GameState.getInstance().get(gameStateKey));
		}
	}
}

