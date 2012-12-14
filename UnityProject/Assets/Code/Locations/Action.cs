using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public abstract class Action {
		public abstract void doAction();
	}
	
	public class ToggleVarAction : Action {
	
		protected Boolean isOn;
		protected Action ifOn;
		protected Action ifOff;
		private String key;
		
		//TODO: replace startsOn with reference to boolean var in game state
		public ToggleVarAction(String boolVarKey, bool defaultVal, Action ifOn, Action ifOff) {
			isOn = GameState.getInstance().has(boolVarKey) ?
				(Boolean) GameState.getInstance().get(boolVarKey) :
				defaultVal;
			this.ifOn = ifOn;
			this.ifOff = ifOff;
			key = boolVarKey;
		}
		
		override public void doAction() {
			if (isOn) {
				ifOn.doAction();
			} else {
				ifOff.doAction();
			}
			isOn = !isOn;
			GameState.getInstance().put(key, isOn);
			GameState.saveState();
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
		
		public void setToCurrentFrame() {
			if (reverse) {
				a[name].time = a[name].length;
			} else {
				a[name].time = 0;
			}
			a[name].speed = 0;
			a.Play(name);
		}
	}
	
	public class ToggleAnimationAction : ToggleVarAction {
		public ToggleAnimationAction(String boolVarKey, bool defaultVal, Animation a, String name) :
			base(boolVarKey, defaultVal, new AnimationAction(a, name, false),
			new AnimationAction(a, name, true)) {
			
			if (isOn) {
				((AnimationAction) ifOn).setToCurrentFrame();
			} else {
				((AnimationAction) ifOff).setToCurrentFrame();
			}
		}
	}
}

