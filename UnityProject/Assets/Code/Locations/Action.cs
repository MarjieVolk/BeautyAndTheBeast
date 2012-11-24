using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public abstract class Action {
		public abstract void doAction();
	}
	
	public class ToggleVarAction : Action {
	
		private Boolean isOn;
		private Action ifOn;
		private Action ifOff;
		
		//TODO: replace startsOn with reference to boolean var in game state
		public ToggleVarAction(Boolean startsOn, Action ifOn, Action ifOff) {
			isOn = startsOn;
			this.ifOn = ifOn;
			this.ifOff = ifOff;
		}
		
		override public void doAction() {
			if (isOn) {
				ifOn.doAction();
			} else {
				ifOff.doAction();
			}
			isOn = !isOn;
		}
		
	}
	
	public class AnimationAction : Action {
		
		private Animation a;
		private String name;
		private Boolean reverse;
		
		public AnimationAction(Animation a, String name, Boolean reverse) {
			this.a = a;
			this.name = name;
			this.reverse = reverse;
		}
		
		override public void doAction() {
			a[name].speed = reverse ? -1 : 1;
			if (reverse && a[name].time == 0) {
				a[name].time = a[name].length;
			}
			a.Play(name);
		}
	}
	
	public class ToggleAnimationAction : ToggleVarAction {
		public ToggleAnimationAction(Boolean startsForward, Animation a, String name) :
			base(startsForward, new AnimationAction(a, name, false),
			new AnimationAction(a, name, true)) {}
	}
}

