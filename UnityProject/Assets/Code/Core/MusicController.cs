using System;
using UnityEngine;

public class MusicController : MonoBehaviour {
	
	public AudioClip[] clips;
	
	private static readonly int MIN_WAIT_TIME = 10;
	private static readonly int MAX_WAIT_TIME = 180;
	
	private System.Random r;
	private float timeOfLastSong = -1;
		
	void Start() {
		r = new System.Random();
	}
	
	void Update() {
		if (audio.isPlaying) {
			timeOfLastSong = Time.time;
		} else {
			if (startSong()) {
				audio.clip = getSong();
				audio.Play();
			}
		}
	}
	
	private AudioClip getSong() {
		return clips[r.Next(clips.Length)];
	}
	
	private bool startSong() {
		if (timeOfLastSong == -1)
			return true;
		
		float diff = Time.time - timeOfLastSong;
		double probability = 0.05;
		return r.NextDouble() < probability;
	}
}

