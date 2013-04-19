using System;
using UnityEngine;
using AssemblyCSharp;

public class ClickAndDragRotateByMousePosition : ClickAndDragRotate
{
	protected override Vector3 getNewRotation(float dX, float dY, float dZ) {
		Vector3 r = transform.localRotation.eulerAngles;
		return new Vector3(r.x + dX, r.y + dY, r.z + dZ);
	}
}

