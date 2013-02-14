using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

	public class ClickAndDragAnimation : ClickAndDrag
	{
		
		public String animName;
		public float[] snapTo;
		public float dAnimationTimePerDMouseY;
		
		private float dragStartAnimPos;
		
		protected override void setVisualState(int snapToIndex) {
			animation[animName].time = snapTo[snapToIndex];
			animation[animName].speed = 0;
			animation.Play (animName);
		}
		
		protected override void initDrag(Vector3 dragStartMousePosition) {
			dragStartAnimPos = animation[animName].time;
			
			// Reset animation speed to 0, just in case it is playing
			animation[animName].speed = 0;
			animation.Play(animName);
		}
		
		protected override int initSnap() {
			float minDistance = float.MaxValue;
			int index = -1;
			float time = animation[animName].time;
			for (int i = 0; i < snapTo.Length; i++) {
				float distance = Math.Abs(snapTo[i] - time);
				if (distance < minDistance) {
					minDistance = distance;
					index = i;
				}
			}
		
			if (animation[animName].time < snapTo[index]) {
				animation[animName].speed = 1;
			} else {
				animation[animName].speed = -1;
			}
		
			animation.Play(animName);
		
			return index;
		}
		
		protected override void doDrag(Vector3 dragStartMousePosition, Vector3 currentMousePosition) {
			float dY = (currentMousePosition - dragStartMousePosition).y / ((float) Screen.height);
			float dTime = dAnimationTimePerDMouseY * dY * animation[animName].length;
			animation[animName].time = Mathf.Clamp(dragStartAnimPos + dTime, 0.0f, animation[animName].length);
		
			animation[animName].speed = 0;
			animation.Play(animName);
		}
		
		protected override bool doSnap(int snapToIndex) {
			float t = animation[animName].time;
			
			bool done = (animation[animName].speed > 0 && (t >= snapTo[snapToIndex] || t == 0))
				|| (animation[animName].speed < 0 && t <= snapTo[snapToIndex]);
			
			if (done) {
				animation[animName].time = snapTo[snapToIndex];
				animation[animName].speed = 0;
				animation.Play(animName);
			}
			
			return done;
		}
}

