using System;
using UnityEngine;
using AssemblyCSharp;

[RequireComponent(typeof(AudioSource))]
public class ReleaseSunKey: Puzzle
{
	private static readonly String KEY_MERCURY = "mercury";
	private static readonly String KEY_VENUS = "venus";
	private static readonly String KEY_EARTH = "earth";
	private static readonly String KEY_MARS = "mars";
	private static readonly String KEY_JUPITER = "jupiter";
	private static readonly String KEY_SATURN = "saturn";
	
	private static readonly int MERCURY = 6;
	private static readonly int VENUS = 2;
	private static readonly int EARTH = 4;
	private static readonly int MARS = 0;
	private static readonly int JUPITER = 7;
	private static readonly int SATURN = 3;
	
	private static readonly float TIME_TO_OPEN = 1;
	
	public static readonly String GAME_STATE_KEY = "sunKeyState";

	private bool opening = false;
	private float startTime;
		
	private Vector3 closedPos = new Vector3(0, -0.087f, 0);
	private Vector3 openPos = new Vector3(0, 0, 0);
	
	public ReleaseSunKey(): base(new PuzzleCondition[] { new PuzzleCondition(KEY_MERCURY, MERCURY),
		new PuzzleCondition(KEY_VENUS, VENUS), new PuzzleCondition(KEY_EARTH, EARTH), new PuzzleCondition(KEY_MARS, MARS),
		new PuzzleCondition(KEY_JUPITER, JUPITER), new PuzzleCondition(KEY_SATURN, SATURN) }, GAME_STATE_KEY) {
		
	}
	
	void Update() {
		if (opening) {
			if (Time.time - startTime >= TIME_TO_OPEN) {
				opening = false;
				setSolved(true);
			} else {
				transform.localPosition = Vector3.Lerp(closedPos, openPos, (Time.time - startTime) / TIME_TO_OPEN);
			}
		}
	}
	
	public override void doSolve() {
		opening = true;
		startTime = Time.time;
		audio.Play();
	}
		
	public override void setSolved(bool isSolved) {
		if (isSolved) {
			transform.localPosition = openPos;
			ClickToPickUp component = (ClickToPickUp) this.gameObject.AddComponent(typeof(ClickToPickUp));
			component.itemId = "roseDoorKey.sun";
		} else {
			transform.localPosition = closedPos;
		}
	}
}

