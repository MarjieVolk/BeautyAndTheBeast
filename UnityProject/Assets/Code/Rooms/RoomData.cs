using UnityEngine;
using System;
using System.Collections;

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
		
		public bool clicked(Vector3 mouseLoc) {			
			Transition moveTo = currentLoc.getTransitionFor(currentDir, mouseLoc);
			
			if (moveTo != null) {
				currentLoc = moveTo.moveTo;
				currentDir = moveTo.turnTo;
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
		
		public Vector3 getPosition() {
			return currentLoc.getPosition();
		}
		
		public Quaternion getRotation() {
			return currentLoc.getRotation(currentDir);
		}
	}
	
}
