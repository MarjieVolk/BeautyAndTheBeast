using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public enum DirectionType
	{
		NORTH, NORTHEAST, EAST, SOUTHEAST, SOUTH, SOUTHWEST, WEST, NORTHWEST
	}
	
	public class DirectionUtil {
		
		private static Dictionary<DirectionType, float> defaultAngle;
		
		static DirectionUtil() {
			defaultAngle = new Dictionary<DirectionType, float>();
			defaultAngle.Add(DirectionType.NORTH, 0);
			defaultAngle.Add(DirectionType.NORTHEAST, 45);
			defaultAngle.Add(DirectionType.EAST, 90);
			defaultAngle.Add(DirectionType.SOUTHEAST, 135);
			defaultAngle.Add(DirectionType.SOUTH, 180);
			defaultAngle.Add(DirectionType.SOUTHWEST, 225);
			defaultAngle.Add(DirectionType.WEST, 270);
			defaultAngle.Add(DirectionType.NORTHWEST, 315);
		}
		
		public static Quaternion getDefaultRotation(DirectionType d) {
			return Quaternion.Euler(0, defaultAngle[d], 0);
		}
		
		public static Vector3 getDefaultEulerAngles(DirectionType d) {
			return new Vector3(0, defaultAngle[d], 0);
		}
		
		public static DirectionType getLeft(DirectionType d) {
			switch (d) {
			case DirectionType.NORTH: return DirectionType.NORTHWEST;
			case DirectionType.NORTHEAST: return DirectionType.NORTH;
			case DirectionType.EAST : return DirectionType.NORTHEAST;
			case DirectionType.SOUTHEAST : return DirectionType.EAST;
			case DirectionType.SOUTH : return DirectionType.SOUTHEAST;
			case DirectionType.SOUTHWEST : return DirectionType.SOUTH;
			case DirectionType.WEST : return DirectionType.SOUTHWEST;
			case DirectionType.NORTHWEST : return DirectionType.WEST;
			default : return DirectionType.NORTH;
			}
		}
		
		public static DirectionType getRight(DirectionType d) {
			switch (d) {
			case DirectionType.NORTH: return DirectionType.NORTHEAST;
			case DirectionType.NORTHEAST: return DirectionType.EAST;
			case DirectionType.EAST : return DirectionType.SOUTHEAST;
			case DirectionType.SOUTHEAST : return DirectionType.SOUTH;
			case DirectionType.SOUTH : return DirectionType.SOUTHWEST;
			case DirectionType.SOUTHWEST : return DirectionType.WEST;
			case DirectionType.WEST : return DirectionType.NORTHWEST;
			case DirectionType.NORTHWEST : return DirectionType.NORTH;
			default : return DirectionType.NORTH;
			}
		}
	}
}

