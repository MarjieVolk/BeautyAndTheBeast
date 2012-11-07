using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp {
	public class TestRoom : RoomData {
		
		private static readonly Location start = new StandardLocation(new Vector3(-24, 5, -19));
		private static readonly Direction startDir = Direction.NORTH;
		
		public TestRoom() : base (start, startDir) {
			Rect button = new Rect(Screen.width / 4, Screen.height / 4,
				Screen.width / 2, Screen.height / 2);
			
			Location bottomCenter = new StandardLocation(new Vector3(-24, 5, 3));
			Location bottomLeft = new StandardLocation(new Vector3(-24, 5, 24));
			Location topCenter = new StandardLocation(new Vector3(3, 5, 3));
			Location topLeft = new StandardLocation(new Vector3(3, 5, 24));
			Location topRight = new StandardLocation(new Vector3(3, 5, -19));
			
			start.addTransition(new Transition(Direction.NORTH, button, bottomCenter));
			start.addTransition(new Transition(Direction.EAST, button, topRight));
			
			bottomCenter.addTransition(new Transition(Direction.NORTH, button, bottomLeft));
			bottomCenter.addTransition(new Transition(Direction.EAST, button, topCenter));
			bottomCenter.addTransition(new Transition(Direction.SOUTH, button, start));
			
			bottomLeft.addTransition(new Transition(Direction.EAST, button, topLeft));
			bottomLeft.addTransition(new Transition(Direction.SOUTH, button, bottomCenter));
			
			topLeft.addTransition(new Transition(Direction.WEST, button, bottomLeft));
			topLeft.addTransition(new Transition(Direction.SOUTH, button, topCenter));
			
			topCenter.addTransition(new Transition(Direction.NORTH, button, topLeft));
			topCenter.addTransition(new Transition(Direction.SOUTH, button, topRight));
			topCenter.addTransition(new Transition(Direction.WEST, button, bottomCenter));
			
			topRight.addTransition(new Transition(Direction.NORTH, button, topCenter));
			topRight.addTransition(new Transition(Direction.WEST, button, start));
		}
		
	}
}

