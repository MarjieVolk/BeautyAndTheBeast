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
		scriptTarget.useFavoredDirection = EditorGUILayout.Toggle("Use Favored Direction", scriptTarget.useFavoredDirection);
		if (scriptTarget.useFavoredDirection) {
			scriptTarget.favoredDirection = (DirectionType) 
				EditorGUILayout.EnumPopup("Favored Direction", scriptTarget.favoredDirection);
		}
		
		scriptTarget.offset = EditorGUILayout.Vector3Field("Offset", scriptTarget.offset);
		
        if (GUI.changed)
            EditorUtility.SetDirty(target);
	}
}