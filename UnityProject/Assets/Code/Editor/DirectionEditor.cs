using System;
using UnityEditor;
using UnityEngine;
using AssemblyCSharp;

[CanEditMultipleObjects]
[CustomEditor(typeof(Direction))]
public class DirectionEditor: Editor
{
	
	private Direction scriptTarget;
	private Vector3 rotationEuler;
	private bool useDefault;
	
	void OnEnable() {
		scriptTarget = (Direction) target;
		rotationEuler = toVector3(scriptTarget.rotation);
		
		useDefault = rotationEuler.Equals(DirectionUtil.getDefaultEulerAngles(scriptTarget.direction)) ||
			rotationEuler.magnitude == 0;
	}
	
	public override void OnInspectorGUI() {
		scriptTarget.direction = (DirectionType) EditorGUILayout.EnumPopup("Direction Type", scriptTarget.direction);
		useDefault = EditorGUILayout.Toggle("Use Default Rotation", useDefault);
		
		if (useDefault) {
			rotationEuler = DirectionUtil.getDefaultEulerAngles(scriptTarget.direction);
		} else {
			rotationEuler = EditorGUILayout.Vector3Field("Rotation", rotationEuler);
		}
		
		scriptTarget.rotation = toQuaternion(rotationEuler);
	}
	
	public static Vector3 toVector3(Quaternion q) {
		return q.eulerAngles;
	}
	
	public static Quaternion toQuaternion(Vector3 v) {
		return Quaternion.Euler(v.x, v.y, v.z);
	}
}

