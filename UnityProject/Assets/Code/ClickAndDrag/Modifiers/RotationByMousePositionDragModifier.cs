using System;
using UnityEngine;

public class RotationByMousePositionDragModifier: RotationDragModifier
{
	protected override Vector3 getNewRotation(float dX, float dY, float dZ) {
		Vector3 r = toRotate.transform.localRotation.eulerAngles;
		return new Vector3(r.x + dX, r.y + dY, r.z + dZ);
	}

}

