using System;
using UnityEditor;
using UnityEngine;
using AssemblyCSharp;
	
[CanEditMultipleObjects]
[CustomEditor(typeof(Location))]
public class LocationEditor : Editor
{
	private Location scriptTarget;
	
	void OnEnable() {
		scriptTarget = (Location) target;
	}
	
	public override void OnInspectorGUI() {		
		scriptTarget.maxDistance = EditorGUILayout.FloatField("Max Trigger Distance", scriptTarget.maxDistance);
		scriptTarget.useFavoredDirection = EditorGUILayout.Toggle("Use Favored Direction", scriptTarget.useFavoredDirection);
		if (scriptTarget.useFavoredDirection) {
			scriptTarget.favoredDirection = (DirectionType) 
				EditorGUILayout.EnumPopup("Favored Direction", scriptTarget.favoredDirection);
		}
	}
}