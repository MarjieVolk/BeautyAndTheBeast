using System;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(ForceByVelocityDragModifier))]
public class ForceByVelocityEditor : Editor
{
	
    public override void OnInspectorGUI() {	
		ForceByVelocityDragModifier scriptTarget = (ForceByVelocityDragModifier) target;
		
		scriptTarget.child = (GameObject) EditorGUILayout.ObjectField("Object", scriptTarget.child, typeof(GameObject), true);
		scriptTarget.maxVelocity = EditorGUILayout.Vector3Field("Direction and Strength", scriptTarget.maxVelocity);
			
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}

