using System;
using UnityEngine;

namespace AssemblyCSharp
{
	[RequireComponent(typeof(AudioSource))]
	public class OpenDeskTop: Puzzle
	{
		private static readonly String KEY_EARTH = "drawerLowerRight";
		private static readonly String KEY_AIR = "drawerUpperLeft";
		private static readonly String KEY_FIRE = "drawerUpperRight";
		private static readonly String KEY_WATER = "drawerLowerLeft";
		
		private static readonly int EARTH = 1;
		private static readonly int AIR = 3;
		private static readonly int FIRE = 2;
		private static readonly int WATER = 2;
		
		private static readonly float TIME_TO_OPEN = 3;
		
		public static readonly String GAME_STATE_KEY = "openDeskTop";
		
		public Location deskLocation;
		
		private bool opening = false;
		private float startTime;
		
		private Quaternion closedRotation = Quaternion.Euler(new Vector3(0, 0, 90));
		private Quaternion openRotation = Quaternion.Euler(new Vector3(0, 0, 0));
		
		public OpenDeskTop(): base (new PuzzleCondition[] { new PuzzleCondition(KEY_AIR, AIR), new PuzzleCondition(KEY_EARTH, EARTH),
			new PuzzleCondition(KEY_FIRE, FIRE), new PuzzleCondition(KEY_WATER, WATER) }, GAME_STATE_KEY) {
			
		}
		
		void Update() {
			if (opening) {
				if (Time.time - startTime >= TIME_TO_OPEN) {
					transform.localRotation = openRotation;
					opening = false;
				} else {
					transform.localRotation = Quaternion.Slerp(closedRotation, openRotation, (Time.time - startTime) / TIME_TO_OPEN);
				}
			}
		}
		
		public override void doSolve() {
			opening = true;
			startTime = Time.time;
						
			audio.Play();
			
			changeMovementStuff();
			deskLocation.moveHere();
		}
		
		public override void setSolved(bool isSolved) {
			if (isSolved) {
				transform.localRotation = openRotation;
				changeMovementStuff();
			} else {
				transform.localRotation = closedRotation;
			}
		}
		
		private void changeMovementStuff() {
			Component[] directions = deskLocation.GetComponents(typeof(Direction));
			foreach (Component c in directions) {
				Direction d = (Direction) c;
				if (d.direction == DirectionType.WEST) {
					Vector3 r = d.rotation.eulerAngles;
					//r.y += 5;
					r.x -= 25;
					d.rotation = Quaternion.Euler(r);
					break;
				}
			}
			
			UnityEngine.Object zoom = Resources.Load("Zoom to Desk Contraption");
			Instantiate(zoom);
		}
	}
}

