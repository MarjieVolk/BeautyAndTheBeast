using UnityEngine;
using System.Collections;
using AssemblyCSharp;

[RequireComponent(typeof(Location))]
public class Direction : MonoBehaviour {
	
	public DirectionType direction;
	public Quaternion rotation;
	
}
