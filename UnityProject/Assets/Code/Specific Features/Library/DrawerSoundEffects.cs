using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class DrawerSoundEffects : DragModifier {
	
	private static readonly string DIR = "Sound Effects/Library/Desk/";
	private static readonly float MIN_V_TRIGGER = 20;
	private static readonly float MIN_REST_TRIGGER = 0.2f;
	
	private AudioSource handleBang;
	private AudioSource drawerBang;
	
	private AudioSource[] slide = new AudioSource[5];
	private AudioClip[] slideClips = new AudioClip[2];
	
	private System.Random r = new System.Random();
	private GameObject handle;
	
	private float startRest = -1;
	private Vector3 prevForce;
	
	// Use this for initialization
	void Start () {
		handle = (gameObject.GetComponent(typeof(ForceByVelocityDragModifier)) as ForceByVelocityDragModifier).child;
		
		handleBang = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		handleBang.clip = Resources.Load(DIR + "Handle") as AudioClip;
		handleBang.volume = 0.3f;
		
		drawerBang = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		drawerBang.clip = Resources.Load(DIR + "Close") as AudioClip;
		
		for (int i = 0; i < slide.Length; i++) {
			slide[i] = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		}
		
		slideClips[0] = Resources.Load(DIR + "Slide 1") as AudioClip;
		slideClips[1] = Resources.Load(DIR + "Slide 2") as AudioClip;
	}
	
	public override void startDrag() {
		strengthenSlide();
	}
	
	public override void endDrag() {
		
	}
	
	public override void startSnap() {
		
	}
	
	public override void endSnap() {
		stopSlide();
		if (prevForce.x > 0)
			handleBang.Play();
	}
	
	public override void handleDragEvent(DragEvent e) {
		if (e.state == DragState.DRAG) {
			if (handle.constantForce.force.x < 0 && prevForce.x > 0)
				handleBang.Play();
			
			prevForce = handle.constantForce.force;
		}
		
		Vector3 v = (Vector3) e.getParam(ClickAndDragTranslate.PARAM_VELOCITY);
		if (v.magnitude >= MIN_V_TRIGGER) {
			strengthenSlide();
		}
		
		if (v.magnitude == 0) {
			if (startRest == -1) {
				startRest = Time.time;
			} else if (Time.time - startRest >= MIN_REST_TRIGGER) {
				stopSlide();
			}
				
		} else {
			startRest = -1;
			
			// check for "impact" in the x direction
			Vector3 newP = (Vector3) e.getParam(ClickAndDragTranslate.PARAM_NEW_P);
			Vector3 desiredNewP = (Vector3) e.getParam(ClickAndDragTranslate.PARAM_DESIRED_NEW_P);
			if (newP.x != desiredNewP.x) {
				drawerBang.volume = Mathf.Clamp(v.magnitude / 50, 0.0f, 1.0f);
				drawerBang.Play();
			}
		}
	}
	
	private void strengthenSlide() {
		foreach (AudioSource s in slide) {
			if (!s.isPlaying) {
				s.clip = slideClips[r.Next(0, slideClips.Length)];
				s.Play();
				return;
			}
		}
	}
	
	private void stopSlide() {
		foreach (AudioSource s in slide) {
			s.Stop();
		}
	}
}
