using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class StandardLocation : Location
	{
		private static readonly Dictionary<Direction, Quaternion> dirs;
		
		static StandardLocation() {
			dirs = new Dictionary<Direction, Quaternion>();
			dirs.Add(Direction.NORTH, Quaternion.Euler(0, 0, 0));
			dirs.Add(Direction.EAST, Quaternion.Euler(0, 90, 0));
			dirs.Add(Direction.SOUTH, Quaternion.Euler(0, 180, 0));
			dirs.Add(Direction.WEST, Quaternion.Euler(0, -90, 0));
		}
		
		public StandardLocation (Vector3 position) : base (position, dirs)
		{
		}
	}
}

