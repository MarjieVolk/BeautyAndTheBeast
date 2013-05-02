using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	/// <summary>
	/// This class represents a puzzle as a set of constraints on the GameState.  When the GameState is changes, it
	/// checks whether the set of constraints are met.  If so, calls doSolve.  Also handles GameState & saving for puzzle state.
	/// </summary>
	public abstract class Puzzle: MonoBehaviour, GameStateListener
	{
		public static readonly int UNSOLVED_STATE = 0;
		public static readonly int SOLVED_STATE = 1;
		
		private PuzzleCondition[] conditions;
		private int state;
		private String gameStateKey;
		
		public Puzzle(PuzzleCondition[] conditions, String gameStateKey)
		{
			this.conditions = conditions;
			this.gameStateKey = gameStateKey;
		}
		
		void Start() {
			if (!GameState.getInstance().has(gameStateKey))
				GameState.getInstance().put(gameStateKey, UNSOLVED_STATE);
				
			state = (int) GameState.getInstance().get(gameStateKey);
			setSolved(state == SOLVED_STATE);
			if (state == UNSOLVED_STATE)
				GameState.getInstance().addListener(this);
		}
		
		public void stateChanged(String stateKey, object oldValue, object newValue) {
			if (state == SOLVED_STATE)
				return;
			
			bool found = false;
			foreach (PuzzleCondition p in conditions) {
				if (p.getKey().Equals(stateKey)) {
					found = true;
					break;
				}
			}
			
			if (!found)
				return;
			
			foreach (PuzzleCondition p in conditions) {
				if (!p.isMet())
					return;
			}
			
			// All conditions are met
			state = SOLVED_STATE;
			GameState.getInstance().put(gameStateKey, state);
			doSolve();
		}
		
		/// <summary>
		/// Called when the player has just solved the puzzle; play relevant animations and sounds, etc
		/// </summary>
		public abstract void doSolve();
		
		/// <summary>
		/// Called on load.  Set the visual
		/// state of any puzzle-related objects to either solved or unsolved
		/// </summary>
		public abstract void setSolved(bool isSolved);
	}
}

