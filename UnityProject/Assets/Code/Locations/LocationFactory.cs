using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class LocationFactory
	{
		private static float N_ANG = 0;
		private static float E_ANG = 90;
		private static float S_ANG = 180;
		private static float W_ANG = -90;
		
//		private static Quaternion N = Quaternion.Euler(0, N_ANG, 0);
//		private static Quaternion E = Quaternion.Euler(0, E_ANG, 0);
//		private static Quaternion S = Quaternion.Euler(0, S_ANG, 0);
//		private static Quaternion W = Quaternion.Euler(0, W_ANG, 0);
		
		public static Location getLocation(Vector3 position, float northAngle, float eastAngle,
			float southAngle, float westAngle)
		{
			Dictionary<Direction, Quaternion> directions = new Dictionary<Direction, Quaternion>();
			directions.Add(Direction.NORTH, Quaternion.Euler(northAngle, N_ANG, 0));
			directions.Add(Direction.EAST, Quaternion.Euler(eastAngle, E_ANG, 0));
			directions.Add(Direction.SOUTH, Quaternion.Euler(southAngle, S_ANG, 0));
			directions.Add(Direction.WEST, Quaternion.Euler(westAngle, W_ANG, 0));
			
			return new Location(position, directions);
		}
	}
}

