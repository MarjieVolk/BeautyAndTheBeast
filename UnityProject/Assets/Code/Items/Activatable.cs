using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public abstract class Activatable: MonoBehaviour
	{
		public abstract void setActive(bool active);
	}
}

