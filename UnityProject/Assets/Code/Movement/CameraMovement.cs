using System;
using UnityEngine;

namespace AssemblyCSharp
{
	/// <summary>
	/// Given a starting and ending position / rotation, along with duration parameters, etc,
	/// calculates the position & rotation of the camera at some time during the transition.
	/// </summary>
	public abstract class CameraMovement
	{
		protected Quaternion startRot, endRot;
		protected Vector3 startPos, endPos;
		protected float startTime;
		
		public CameraMovement(Vector3 startPos, Quaternion startRot) {
			this.endRot = startRot;
			this.endPos = startPos;
			this.startTime = Time.time;
		}
		
		public virtual void reset(Vector3 goalPos, Quaternion goalLoc) {
			this.startPos = this.endPos;
			this.endPos = goalPos;
			this.startRot = this.endRot;
			this.endRot = goalLoc;
			this.startTime = Time.time;
		}
		
		public void update(Transform trans, float t) {
			trans.position = getPosition(t);
			trans.rotation = getRotation(t);
		}
		
		public abstract Vector3 getPosition(float time);
		public abstract Quaternion getRotation(float time);
		public abstract Boolean isDone(float time);
	}
	
	public abstract class PercentBasedMovement : CameraMovement {
		public PercentBasedMovement(Vector3 startPos, Quaternion startRot):
			base (startPos, startRot) {}
		
		override public Vector3 getPosition(float time) {
			return Vector3.Lerp(startPos, endPos, percentDone(time));
		}
		
		override public Quaternion getRotation(float time) {
			float per = percentDone(time);			
			return Quaternion.Lerp(startRot, endRot, per);
		}
		
		override public  Boolean isDone(float time) {
			return percentDone(time) >= 1.0f;
		}
		
		public abstract float percentDone(float time);
	}
	
	/// <summary>
	/// Moves the camera linearly at a constant speed.
	/// </summary>
	public class ConstantTimeMovement : PercentBasedMovement {
		private static readonly float MOVE_TIME = 0.8f;
		
		public ConstantTimeMovement(Vector3 startPos, Quaternion startRot):
			base (startPos, startRot) {}
		
		override public float percentDone(float time) {
			return (time - startTime) / MOVE_TIME;
		}
	}
	
	public class ConstantSpeedMovement : PercentBasedMovement {
		private static readonly float UNITS_PER_SECOND = 3;
		private static readonly float RADIANS_PER_SECOND = 160;
		protected float duration;
		
		public ConstantSpeedMovement(Vector3 startPos, Quaternion startRot):
			base (startPos, startRot) {}
		
		public override void reset(Vector3 goalPos, Quaternion goalLoc) {
			base.reset(goalPos, goalLoc);
			
			duration = Math.Max(Vector3.Distance(startPos, endPos) / UNITS_PER_SECOND,
				Quaternion.Angle(startRot, endRot) / RADIANS_PER_SECOND);
		}
		
		override public float percentDone(float time) {
			return duration == 0 ? 1.0f : (time - startTime) / duration;
		}
	}
	
	/// <summary>
	/// Moves the camera linearly, but at a speed that starts at zero, increases to a calculated
	/// constant, and then decreases to zero again, according to the sine function
	/// </summary>
	public class SineMovement : ConstantSpeedMovement {
		public SineMovement(Vector3 startPos, Quaternion startRot):
			base (startPos, startRot) {}
		
		override public float percentDone(float time) {
			if (duration == 0)
				return 1;
			
			float x = ((time -startTime) / duration) * 2 * (float) Math.PI;
			return (x - (float) Math.Sin(x)) / (2 * (float) Math.PI);
		}
	}
	
	/// <summary>
	/// Moves the camera in a bobbing motion, imitating human steps.
	/// </summary>
	public class BobbingMovement : CameraMovement {
		private static readonly float UNITS_PER_SECOND = 15;
		private static readonly float RADIANS_PER_SECOND = 160;
		
		//Max angle between legs, in degrees
		private static readonly float STEP_ANGLE = (float) (0.3 * Math.PI);
		private static readonly float LEG_LENGTH = 8.0f;
		
		//Lolz Law of Cosines
		private static readonly float CENTER_SPACING = (float) Math.Sqrt(2
			* Math.Pow(LEG_LENGTH, 2) * (1 - Math.Cos(STEP_ANGLE)));
		
		private float distance, duration;
		
		public BobbingMovement(Vector3 startPos, Quaternion startRot):
			base (startPos, startRot) {}
		
		override public void reset(Vector3 goalPos, Quaternion goalLoc) {
			base.reset(goalPos, goalLoc);
			
			duration = Math.Max(Vector3.Distance(startPos, endPos) / UNITS_PER_SECOND,
				Quaternion.Angle(startRot, endRot) / RADIANS_PER_SECOND);
			distance = Vector3.Distance(endPos, startPos);
		}
		
		override public Vector3 getPosition(float time) {
			Vector3 pos = Vector3.Lerp(startPos, endPos, percentDone(time));
			pos.y += currDY(distance * percentDone(time));
			return pos;
		}
		
		override public Quaternion getRotation(float time) {
			return Quaternion.Lerp(startRot, endRot, percentDone(time));
		}
		
		override public  Boolean isDone(float time) {
			return percentDone(time) >= 1.0f;
		}
		
		public float percentDone(float time) {
			return (time - startTime) / duration;
		}
		
		private float currDY(float x) {
			float dFromCenter = x % CENTER_SPACING;
			
			float xNextStep = CENTER_SPACING;
			float lastNormalStep = CENTER_SPACING * (float) Math.Floor(distance / CENTER_SPACING);
			//Special case: the spacing between the last two steps is adjusted
			//so that the last step falls at the ending point
			if (x > lastNormalStep) {
				xNextStep = distance - lastNormalStep;
			}
			
			//
			if (dFromCenter > xNextStep / 2) {
				dFromCenter = xNextStep - dFromCenter;
			}
			
			return (float) Math.Sqrt(Math.Pow(LEG_LENGTH, 2) + Math.Pow(dFromCenter, 2)) - LEG_LENGTH;
		}
	}
}

