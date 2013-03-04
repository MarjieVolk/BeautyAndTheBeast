using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class CameraAction {
		private Nullable<Quaternion> rotation;
		private Nullable<Vector3> position;
		private Location location;
		
		public CameraAction(Location loc, Nullable<Vector3> position,
			Nullable<Quaternion> rotation) {
			
			this.rotation = rotation;
			this.position = position;
			this.location = loc;
		}
		
		public bool hasRotation() {
			return rotation.HasValue;
		}
		
		public bool hasPosition() {
			return position.HasValue;
		}
		
		public Quaternion getRotation() {
			return rotation.Value;
		}
		
		public Vector3 getPosition() {
			return position.Value;
		}
		
		public Location getLocation() {
			return location;
		}
	}
}

