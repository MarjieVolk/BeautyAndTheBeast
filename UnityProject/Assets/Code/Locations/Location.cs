using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp {
	
	public class Location {
		
		private Vector3 position;
		private Dictionary<Direction, Quaternion> directions;
		private List<Transition> transitions = new List<Transition>();
		
		public Location (Vector3 position, Dictionary<Direction, Quaternion> directions) {
			this.position = position;
			this.directions = directions;
		}
		
		public Vector3 getPosition() {
			return position;
		}
		
		public Boolean hasDirection(Direction d) {
			return directions.ContainsKey(d);
		}
		
		public Quaternion getRotation(Direction d) {
			return directions[d];
		}
		
		public void addTransition(Transition t) {
			transitions.Add(t);
		}
		
		public Transition getTransitionFor(Direction facing, Vector3 mouseClicked) {
			foreach (Transition t in transitions) {
				if (t.facing == facing &&
					t.screenArea.Contains(mouseClicked)) {
					return t;
				}
			}
			return null;
		}
		
		public Direction getNextLeft(Direction d) {
			Direction newD = getLeft(d);
			while (!hasDirection(newD) && newD != d) {
				newD = getLeft (newD);
			}
			return newD;
		}
		
		public Direction getNextRight(Direction d) {
			Direction newD = getRight(d);
			while (!hasDirection(newD) && newD != d) {
				newD = getRight (newD);
			}
			return newD;
		}
		
		public static Direction getLeft(Direction d) {
			switch (d) {
			case Direction.NORTH: return Direction.NORTHWEST;
			case Direction.NORTHEAST: return Direction.NORTH;
			case Direction.EAST : return Direction.NORTHEAST;
			case Direction.SOUTHEAST : return Direction.EAST;
			case Direction.SOUTH : return Direction.SOUTHEAST;
			case Direction.SOUTHWEST : return Direction.SOUTH;
			case Direction.WEST : return Direction.SOUTHWEST;
			case Direction.NORTHWEST : return Direction.WEST;
			default : return Direction.NORTH;
			}
		}
		
		public static Direction getRight(Direction d) {
			switch (d) {
			case Direction.NORTH: return Direction.NORTHEAST;
			case Direction.NORTHEAST: return Direction.EAST;
			case Direction.EAST : return Direction.SOUTHEAST;
			case Direction.SOUTHEAST : return Direction.SOUTH;
			case Direction.SOUTH : return Direction.SOUTHWEST;
			case Direction.SOUTHWEST : return Direction.WEST;
			case Direction.WEST : return Direction.NORTHWEST;
			case Direction.NORTHWEST : return Direction.NORTH;
			default : return Direction.NORTH;
			}
		}
	}
	
	public enum Direction {
		NORTH, NORTHEAST, EAST, SOUTHEAST, SOUTH, SOUTHWEST, WEST, NORTHWEST
	};
	
	public class Transition {
		public readonly Direction facing;
		public readonly Rect screenArea;
		public readonly Location moveTo;
		public readonly Direction turnTo;
		
		public Transition(Direction facing, Rect screenArea, Location moveTo, Direction turnTo) {
			this.facing = facing;
			this.screenArea = screenArea;
			this.moveTo = moveTo;
			this.turnTo = turnTo;
		}
		
		public Transition(Direction facing, Rect screenArea, Location moveTo) {
			this.facing = facing;
			this.screenArea = screenArea;
			this.moveTo = moveTo;
			this.turnTo = facing;
		}
	}
	
}

