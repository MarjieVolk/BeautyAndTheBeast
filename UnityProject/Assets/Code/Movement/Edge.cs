using System;
using UnityEngine;

namespace AssemblyCSharp
{
	[RequireComponent(typeof(LocationGraph))]
	public class Edge: MonoBehaviour
	{
		public Location one;
		public Location two;
	}
}

