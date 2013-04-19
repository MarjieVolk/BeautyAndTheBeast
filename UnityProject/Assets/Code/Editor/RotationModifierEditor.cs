using System;
using AssemblyCSharp;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RotationDragModifier))]
public class RotationModifierEditor: Editor
{
	
	RotationDragModifier scriptTarget;
	
	private bool rotateX;
	private bool rotateY;
	private bool rotateZ;
	
	private Vector3 x;
	private Vector3 y;
	private Vector3 z;
	
	public void OnEnable() {
		scriptTarget = (RotationDragModifier) target;
		rotateX = scriptTarget.dXPerDBaseX != 0 || scriptTarget.dXPerDBaseY != 0 || scriptTarget.dXPerDBaseZ != 0;
		rotateY = scriptTarget.dYPerDBaseX != 0 || scriptTarget.dYPerDBaseY != 0 || scriptTarget.dYPerDBaseZ != 0;
		rotateZ = scriptTarget.dZPerDBaseX != 0 || scriptTarget.dZPerDBaseY != 0 || scriptTarget.dZPerDBaseZ != 0;
		
		x = new Vector3(scriptTarget.dXPerDBaseX, scriptTarget.dXPerDBaseY, scriptTarget.dXPerDBaseZ);
		y = new Vector3(scriptTarget.dYPerDBaseX, scriptTarget.dYPerDBaseY, scriptTarget.dYPerDBaseZ);
		z = new Vector3(scriptTarget.dZPerDBaseX, scriptTarget.dZPerDBaseY, scriptTarget.dZPerDBaseZ);
	}
	
	public override void OnInspectorGUI() {
		scriptTarget.toRotate = (GameObject) EditorGUILayout.ObjectField("To Rotate", scriptTarget.toRotate,
			typeof(GameObject), true);
		
		rotateX = EditorGUILayout.Toggle("Rotate X", rotateX);
		if (rotateX) {
			x = EditorGUILayout.Vector3Field("X Rotation Per Base Rotation", x);
			scriptTarget.dXPerDBaseX = x.x;
			scriptTarget.dXPerDBaseY = x.y;
			scriptTarget.dXPerDBaseZ = x.z;
		} else {
			scriptTarget.dXPerDBaseX = 0;
			scriptTarget.dXPerDBaseY = 0;
			scriptTarget.dXPerDBaseZ = 0;
		}
		
		rotateY = EditorGUILayout.Toggle("Rotate Y", rotateY);
		if (rotateY) {
			y = EditorGUILayout.Vector3Field("Y Rotation Per Base Rotation", y);
			scriptTarget.dYPerDBaseX = y.x;
			scriptTarget.dYPerDBaseY = y.y;
			scriptTarget.dYPerDBaseZ = y.z;
		} else {
			scriptTarget.dYPerDBaseX = 0;
			scriptTarget.dYPerDBaseY = 0;
			scriptTarget.dYPerDBaseZ = 0;
		}
		
		rotateZ = EditorGUILayout.Toggle("Rotate Z", rotateZ);
		if (rotateZ) {
			z = EditorGUILayout.Vector3Field("Z Rotation Per Base Rotation", z);
			scriptTarget.dZPerDBaseX = z.x;
			scriptTarget.dZPerDBaseY = z.y;
			scriptTarget.dZPerDBaseZ = z.z;
		} else {
			scriptTarget.dZPerDBaseX = 0;
			scriptTarget.dZPerDBaseY = 0;
			scriptTarget.dZPerDBaseZ = 0;
		}
	}
}

