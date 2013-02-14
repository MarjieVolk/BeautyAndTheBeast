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
		
		public CameraMovement(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot) {
			this.startRot = startRot;
			this.startPos = startPos;
			this.endRot = endRot;
			this.endPos = endPos;
			this.startTime = Time.time;
		}
		
		public void update(Transform trans) {
			float t = Time.time;
			trans.position = getPosition(t);
			trans.rotation = getRotation(t);
		}
		
		public abstract Vector3 getPosition(float time);
		public abstract Quaternion getRotation(float time);
		public abstract Boolean isDone(float time);
		public abstract float percentDone(float time);
	}
	
	public abstract class PercentBasedMovement : CameraMovement {
		public PercentBasedMovement(Vector3 startPos, Vector3 endPos, Quaternion startRot,
			Quaternion endRot) : base (startPos, endPos, startRot, endRot) {}
		
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
	}
	
	/// <summary>
	/// Moves the camera linearly at a constant speed.
	/// </summary>
	public class ConstantTimeMovement : PercentBasedMovement {
		private static readonly float MOVE_TIME = 0.8f;
		
		public ConstantTimeMovement(Vector3 startPos, Vector3 endPos, Quaternion startRot,
			Quaternion endRot) : base (startPos, endPos, startRot, endRot) {}
		
		override public float percentDone(float time) {
			return (time - startTime) / MOVE_TIME;
		}
	}
	
	public class ConstantSpeedMovement : PercentBasedMovement {
		private static readonly float UNITS_PER_SECOND = 30;
		private static readonly float RADIANS_PER_SECOND = 160;
		private float duration;
		
		public ConstantSpeedMovement(Vector3 startPos, Vector3 endPos, Quaternion startRot,
			Quaternion endRot) : base (startPos, endPos, startRot, endRot) {
			duration = Math.Max(Vector3.Distance(startPos, endPos) / UNITS_PER_SECOND,
				Quaternion.Angle(startRot, endRot) / RADIANS_PER_SECOND);
		}
		
		override public float percentDone(float time) {
			return (time - startTime) / duration;
		}
	}
	
	/// <summary>
	/// Moves the camera linearly, but at a speed that starts at zero, increases to a calculated
	/// constant, and then decreases to zero again, according to the sine function
	/// </summary>
	public class SineMovement : PercentBasedMovement {
		private static readonly float UNITS_PER_SECOND = 30;
		private static readonly float RADIANS_PER_SECOND = 160;
		private float duration;
		
		public SineMovement(Vector3 startPos, Vector3 endPos, Quaternion startRot,
			Quaternion endRot) : base (startPos, endPos, startRot, endRot) {
			duration = Math.Max(Vector3.Distance(startPos, endPos) / UNITS_PER_SECOND,
				Quaternion.Angle(startRot, endRot) / RADIANS_PER_SECOND);
		}
		
		override public float percentDone(float time) {
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
		
		public BobbingMovement(Vector3 startPos, Vector3 endPos, Quaternion startRot,
			Quaternion endRot) : base (startPos, endPos, startRot, endRot) {
			
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
		
		override public float percentDone(float time) {
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
	
	/// <summary>
	/// Starts the camera at a velocity of 0, linearly accelerating it for 'tAccel' seconds,
	/// then holding it at max velocity ('maxV') for 'tMid' seconds, and finally decelerating
	/// it for 'tAccel' seconds again, stopping at velocity zero (and also at the intended
	/// location).
	/// 
	/// A graph of the velocity forms a trapezoid.  Distance is calculated by geometrically
	/// taking the integral of this shape.
	/// </summary>
	public class AccelerationMovement : PercentBasedMovement {
		private static readonly float UNITS_PER_SECOND = 30;
		private static readonly float RADIANS_PER_SECOND = 160;
		//The ratio between time spent accelerating and time spent at constant velocity
		private static readonly float TMID_OVER_TACCEL = 1.8f;

		private float maxV, tMid, tAccel, distance, duration;
		
		public AccelerationMovement(Vector3 startPos, Vector3 endPos, Quaternion startRot,
			Quaternion endRot) : base (startPos, endPos, startRot, endRot) {
			
			duration = Math.Max(Vector3.Distance(startPos, endPos) / UNITS_PER_SECOND,
				Quaternion.Angle(startRot, endRot) / RADIANS_PER_SECOND);
			distance = Vector3.Distance(startPos, endPos);
			
			tAccel = (duration - tMid) / (2 * TMID_OVER_TACCEL);
			tMid = duration - (2 * tAccel);
			maxV = distance / (tAccel + tMid);
		}
		
		override public float percentDone(float time) {
			float t = time - startTime;
			float d; //distance
			if (t <= tAccel) {
				//During inital acceleration----------------------
				var y = (t / tAccel) * maxV;
				d = t * y / 2;
				
			} else if (t >= duration - tAccel) {
				if (t < duration) {
					//During final deceleration-------------------
					var inverseT = duration - t;
					var smallTriangleY = (inverseT / tAccel) * maxV;
					var smallTriangle = inverseT * smallTriangleY / 2; 
					//Take area of the whole trapezoid, then subtract the small triangle
					//which has not yet been reached
					d = (tAccel * maxV) + (tMid * maxV) - smallTriangle;
				} else {
					//Already done--------------------------------
					return 1;
				}
				
			} else {
				//At constant velocity----------------------------
				d = (tAccel * maxV) / 2 + maxV * (t - tAccel);
			}
			return d / distance;
		}
	}
	
	public class SineAccelerationMovement : PercentBasedMovement {
		private static readonly float UNITS_PER_SECOND = 30;
		private static readonly float RADIANS_PER_SECOND = 160;
		//The ratio between time spent accelerating and time spent at constant velocity
		private static readonly float TMID_OVER_TACCEL = 1.8f;

		private float maxV, tMid, tAccel, distance, duration;
		
		public SineAccelerationMovement(Vector3 startPos, Vector3 endPos, Quaternion startRot,
			Quaternion endRot) : base (startPos, endPos, startRot, endRot) {
			
			duration = Math.Max(Vector3.Distance(startPos, endPos) / UNITS_PER_SECOND,
				Quaternion.Angle(startRot, endRot) / RADIANS_PER_SECOND);
			distance = Vector3.Distance(startPos, endPos);
			
			tAccel = (duration - tMid) / (2 * TMID_OVER_TACCEL);
			tMid = duration - (2 * tAccel);
			maxV = distance / (tAccel + tMid);
		}
		
		override public float percentDone(float time) {
			float t = time - startTime;
			float d; //distance
			if (t <= tAccel) {
				//During inital acceleration----------------------
				var x = t * Math.PI / tAccel;
				d = (float) ((x - Math.Sin(x)) * maxV * tAccel / (2*Math.PI));
				
			} else if (t >= duration - tAccel) {
				if (t < duration) {
					//During final deceleration-------------------
					var inverseT = duration - t;
					var x = inverseT * Math.PI / tAccel;
					var smallTriangle = (x - Math.Sin(x)) * maxV * tAccel / (2*Math.PI);
					//Take area of the whole trapezoid, then subtract the small portion
					//which has not yet been reached
					d = (float) ((tAccel * maxV) + (tMid * maxV) - smallTriangle);
				} else {
					//Already done--------------------------------
					return 1;
				}
				
			} else {
				//At constant velocity----------------------------
				d = (tAccel * maxV) / 2 + maxV * (t - tAccel);
			}
			return d / distance;
		}
	}
}

