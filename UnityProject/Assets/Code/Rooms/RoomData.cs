using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp {
	
	[Serializable]
	public class RoomData {
		
		private static readonly int TURN_MARGIN = Screen.width / 5;
		
		//private ArrayList<Location> locations;
		private Location currentLoc;
		private Direction currentDir = Direction.NONE;
		private Dictionary<String, Location> locations;
		
		public RoomData(Dictionary<String, Location> locations) {
			this.locations = locations;
		}
		
		//Returns whether or not the camera should update
		public bool clicked(Vector3 mouseLoc) {
			mouseLoc.y = Screen.height - mouseLoc.y;
			HotPoint hp = getLocation().getHotPointFor(getDirection(), mouseLoc);
			
			if (hp != null) {
				hp.doAction();
				return false;
			}
			
			Transition moveTo = getLocation().getTransitionFor(getDirection(), mouseLoc);
			
			if (moveTo != null) {
				setLocation(moveTo.moveTo);
				if (moveTo.turnTo != Direction.NONE) {
					setDirection(moveTo.turnTo);
				}
				return true;
				
			} else if (mouseLoc.x <= TURN_MARGIN) {
				//Turn left
				setDirection(getLocation().getNextLeft(getDirection()));
				return true;
				
			} else if (mouseLoc.x >= Screen.width - TURN_MARGIN) {
				//Turn right
				setDirection(getLocation().getNextRight(getDirection()));
				return true;
			}
			
			return false;
		}
		
		public List<Transition> getTransitions() {
			return getLocation().getAllTransitions(getDirection());
		}
		
		public List<HotPoint> getHotPoints() {
			return getLocation().getAllHotPoints(getDirection());
		}
		
		public Vector3 getPosition() {
			return getLocation().getPosition();
		}
		
		public Quaternion getRotation() {
			return getLocation().getRotation(getDirection());
		}
		
		protected Location getLocation(String key) {
			return locations[key];
		}
		
		private void setLocation(String loc) {
			currentLoc = locations[loc];
			GameState.getInstance().setLocation(loc);
		}
		
		private void setDirection(Direction d) {
			currentDir = d;
			GameState.getInstance().setDirection(d);
		}
		
		private Location getLocation() {
			if (currentLoc == null) {
				currentLoc = locations[GameState.getInstance().getLocation()];
			}
			return currentLoc;
		}
		
		private Direction getDirection() {
			if (currentDir == Direction.NONE) {
				currentDir = GameState.getInstance().getDirection();
			}
			return currentDir;
		}
	}
	
}
