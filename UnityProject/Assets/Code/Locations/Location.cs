using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp {
	
	[Serializable]
	public class Location {
		
		private Vector3 position;
		private Dictionary<Direction, DirData> dirs;
		
		public Location (Vector3 position, Dictionary<Direction, Quaternion> directions) {
			this.position = position;
			dirs = new Dictionary<Direction, DirData>();
			foreach (Direction d in directions.Keys) {
				dirs.Add(d, new DirData(directions[d]));
			}
		}
		
		public Vector3 getPosition() {
			return position;
		}
		
		public Boolean hasDirection(Direction d) {
			return dirs.ContainsKey(d);
		}
		
		public Quaternion getRotation(Direction d) {
			return dirs[d].rotation;
		}
		
		public void addTransition(Transition t, Direction facing) {
			dirs[facing].transitions.Add(t);
		}
		
		public Transition getTransitionFor(Direction facing, Vector3 mouseClicked) {
			foreach (Transition t in dirs[facing].transitions) {
				if (t.screenArea.Contains(mouseClicked)) {
					return t;
				}
			}
			return null;
		}
		
		public List<Transition> getAllTransitions(Direction facing) {
			return dirs[facing].transitions;
		}
		
		public void addHotPoint(HotPoint pt, Direction facing) {
			dirs[facing].hotPoints.Add(pt);
		}
		
		public HotPoint getHotPointFor(Direction facing, Vector3 mouseClicked) {
			foreach (HotPoint hp in dirs[facing].hotPoints) {
				if (hp.screenArea.Contains(mouseClicked)) {
					return hp;
				}
			}
			return null;
		}
		
		public List<HotPoint> getAllHotPoints(Direction facing) {
			return dirs[facing].hotPoints;
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
		
		private class DirData {
			public readonly List<Transition> transitions = new List<Transition>();
			public readonly List<HotPoint> hotPoints = new List<HotPoint>();
			public readonly Quaternion rotation;
		
			public DirData(Quaternion rotation) {
				this.rotation = rotation;
			}
		}
	}
	
	[Serializable]
	public enum Direction {
		NORTH, NORTHEAST, EAST, SOUTHEAST, SOUTH, SOUTHWEST, WEST, NORTHWEST, NONE
	};
	
	public class Transition {
		public readonly Rect screenArea;
		public readonly String moveTo;
		public readonly Direction turnTo;
		
		public Transition(float xPer, float yPer, float widthPer, float heightPer,
			String moveTo, Direction turnTo) {
			
			this.screenArea = new Rect(xPer * Screen.width, yPer * Screen.height,
				widthPer * Screen.width, heightPer * Screen.height);
			this.moveTo = moveTo;
			this.turnTo = turnTo;
		}
		
		public Transition(Rect screenArea, String moveTo, Direction turnTo) {
			this.screenArea = screenArea;
			this.moveTo = moveTo;
			this.turnTo = turnTo;
		}
		
		public Transition(Rect screenArea, String moveTo) {
			this.screenArea = screenArea;
			this.moveTo = moveTo;
			this.turnTo = Direction.NONE;
		}
	}
	
	public class HotPoint {
		public readonly Rect screenArea;
		private Action[] thingsToDo;
		
		public HotPoint(float xPer, float yPer, float widthPer, float heightPer,
			Action[] actionsToTake) {
			
			thingsToDo = actionsToTake;
			this.screenArea = new Rect(xPer * Screen.width, yPer * Screen.height,
				widthPer * Screen.width, heightPer * Screen.height);
		}
		
		public void doAction() {
			foreach (Action am in thingsToDo) {
				am.doAction();
			}
		}		
	}
}

