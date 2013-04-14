using System;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(ZoomLocation))]
public class ZoomLocationEditor: Editor
{
	private ZoomLocation scriptTarget;
	private bool autoParent;
	private Vector3 rotationEuler;
	
	void OnEnable() {
		scriptTarget = (ZoomLocation) target;
		autoParent = scriptTarget.parent == null;
		
		rotationEuler = scriptTarget.rotation.eulerAngles;
		rotationEuler.x = (float) decimal.Round((decimal) rotationEuler.x, 2, MidpointRounding.AwayFromZero);
		rotationEuler.y = (float) decimal.Round((decimal) rotationEuler.y, 2, MidpointRounding.AwayFromZero);
		rotationEuler.z = (float) decimal.Round((decimal) rotationEuler.z, 2, MidpointRounding.AwayFromZero);
	}
	
	public override void OnInspectorGUI() {
		autoParent = EditorGUILayout.Toggle("Auto-find Parent", autoParent);
		
		if (!autoParent) {
			scriptTarget.parent = (Location) EditorGUILayout.ObjectField("Parent", scriptTarget.parent, typeof(Location), true);
		}
		
		rotationEuler = EditorGUILayout.Vector3Field("Rotation", rotationEuler);
		scriptTarget.rotation = Quaternion.Euler(rotationEuler);
	}
}

