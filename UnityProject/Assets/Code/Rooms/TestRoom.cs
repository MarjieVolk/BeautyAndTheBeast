using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp {
	
	[Serializable]
	public class TestRoom : RoomData {
		private static readonly int headY = 8;
		
		public TestRoom() : base (buildLocations()) {
			var anim = GameObject.Find("Lever").animation;
			var action = new ToggleAnimationAction("Test.Lever", false, anim, "LeverPull");
			var hp = new HotPoint(0.16f, 0.43f, 0.16f, 0.518f, new Action[] { action });
			getLocation("bottomCenter").addHotPoint(hp, Direction.EAST);
		}
		
		private static Dictionary<String, Location> buildLocations() {
			Rect button = new Rect(Screen.width / 4, Screen.height / 4,
				Screen.width / 2, Screen.height / 2);
			
			Location bottomRight = new StandardLocation(new Vector3(-24, headY, -19));
			Location bottomCenter = new StandardLocation(new Vector3(-24, headY, 3));
			Location bottomLeft = new StandardLocation(new Vector3(-24, headY, 24));
			Location topCenter = new StandardLocation(new Vector3(3, headY, 3));
			Location topLeft = new StandardLocation(new Vector3(3, headY, 24));
			Location topRight = new StandardLocation(new Vector3(3, headY, -19));
			
			bottomRight.addTransition(new Transition(button, "bottomCenter"), Direction.NORTH);
			bottomRight.addTransition(new Transition(button, "topRight"), Direction.EAST);
			
			bottomCenter.addTransition(new Transition(button, "bottomLeft"), Direction.NORTH);
			bottomCenter.addTransition(new Transition(button, "topCenter"), Direction.EAST);
			bottomCenter.addTransition(new Transition(button, "bottomRight"), Direction.SOUTH);
			
			bottomLeft.addTransition(new Transition(button, "topLeft"), Direction.EAST);
			bottomLeft.addTransition(new Transition(button, "bottomCenter"), Direction.SOUTH);
			
			topLeft.addTransition(new Transition(button, "bottomLeft"), Direction.WEST);
			topLeft.addTransition(new Transition(button, "topCenter"), Direction.SOUTH);
			
			topCenter.addTransition(new Transition(button, "topLeft"), Direction.NORTH);
			topCenter.addTransition(new Transition(button, "topRight"), Direction.SOUTH);
			topCenter.addTransition(new Transition(button, "bottomCenter"), Direction.WEST);
			
			topRight.addTransition(new Transition(button, "topCenter"), Direction.NORTH);
			topRight.addTransition(new Transition(button, "bottomRight"), Direction.WEST);
			
			Dictionary<String, Location> locations = new Dictionary<String, Location>();
			locations.Add("bottomRight", bottomRight);
			locations.Add("bottomCenter", bottomCenter);
			locations.Add("bottomLeft", bottomLeft);
			locations.Add("topRight", topRight);
			locations.Add("topCenter", topCenter);
			locations.Add("topLeft", topLeft);
			
			return locations;
		}
		
	}
}

