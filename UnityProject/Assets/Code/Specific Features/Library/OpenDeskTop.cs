using System;
using UnityEngine;

namespace AssemblyCSharp
{
	[RequireComponent(typeof(AudioSource))]
	public class OpenDeskTop: MonoBehaviour, GameStateListener
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
		
		private bool opened = false;
		private bool opening = false;
		private float startTime;
		
		private Quaternion closedRotation;
		private Quaternion openRotation;
		
		void Start() {
			GameState.getInstance().addListener(this);
			
			if (!GameState.getInstance().has(GAME_STATE_KEY))
				GameState.getInstance().put(GAME_STATE_KEY, false);
			
			opened = (bool) GameState.getInstance().get(GAME_STATE_KEY);
			
			openRotation = Quaternion.Euler(new Vector3(0, 0, 0));
			closedRotation = Quaternion.Euler(new Vector3(0, 0, 90));
			
			if (opened) {
				transform.localRotation = openRotation;
				setStateOpen();
			} else {
				transform.localRotation = closedRotation;
			}
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
		
		public void stateChanged(String stateKey, object oldValue, object newValue) {
			if (!opened && (stateKey.Equals(KEY_AIR) || stateKey.Equals(KEY_EARTH) ||
				stateKey.Equals(KEY_FIRE) || stateKey.Equals(KEY_WATER))) {
				
				try {
					int earth = (int) GameState.getInstance().get (KEY_EARTH);
					int air = (int) GameState.getInstance().get (KEY_AIR);
					int fire = (int) GameState.getInstance().get (KEY_FIRE);
					int water = (int) GameState.getInstance().get (KEY_WATER);
					
					if (earth == EARTH && air == AIR && fire == FIRE && water == WATER) {
						doOpen();
					}
				} catch {
					// This may happen during initialization, before all of the drawers have registered their states
				}
			}
		}
		
		private void doOpen() {
			opened = true;
			opening = true;
			startTime = Time.time;
						
			GameState.getInstance().put(GAME_STATE_KEY, true);
			audio.Play();
			
			setStateOpen();
			
			deskLocation.moveHere();
		}
		
		private void setStateOpen() {
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

