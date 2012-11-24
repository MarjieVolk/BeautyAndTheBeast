using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp {

	public class RoomData {
		
		private static readonly int TURN_MARGIN = Screen.width / 5;
		
		//private ArrayList<Location> locations;
		Location currentLoc;
		Direction currentDir;
		
		public RoomData(Location startLoc, Direction startDir) {
			currentLoc = startLoc;
			currentDir = startDir;
		}
		
		//Returns whether or not the camera should update
		public bool clicked(Vector3 mouseLoc) {
			mouseLoc.y = Screen.height - mouseLoc.y;
			HotPoint hp = currentLoc.getHotPointFor(currentDir, mouseLoc);
			
			if (hp != null) {
				hp.doAction();
				return false;
			}
			
			Transition moveTo = currentLoc.getTransitionFor(currentDir, mouseLoc);
			
			if (moveTo != null) {
				currentLoc = moveTo.moveTo;
				if (moveTo.turnTo != Direction.NONE) {
					currentDir = moveTo.turnTo;
				}
				return true;
				
			} else if (mouseLoc.x <= TURN_MARGIN) {
				//Turn left
				currentDir = currentLoc.getNextLeft(currentDir);
				return true;
				
			} else if (mouseLoc.x >= Screen.width - TURN_MARGIN) {
				//Turn right
				currentDir = currentLoc.getNextRight(currentDir);
				return true;
			}
			
			return false;
		}
		
		public List<Transition> getTransitions() {
			return currentLoc.getAllTransitions(currentDir);
		}
		
		public List<HotPoint> getHotPoints() {
			return currentLoc.getAllHotPoints(currentDir);
		}
		
		public Vector3 getPosition() {
			return currentLoc.getPosition();
		}
		
		public Quaternion getRotation() {
			return currentLoc.getRotation(currentDir);
		}
	}
	
}
