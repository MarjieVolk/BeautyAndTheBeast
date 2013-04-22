using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class DrawerKeyOnUnlock: MonoBehaviour, GameStateListener
	{
		public GameObject keyModel;
		
		private Lock lck;
		private bool isUnlocked;
		
		void Start() {
			lck = gameObject.GetComponents(typeof(Lock))[0] as Lock;
			isUnlocked = (bool) GameState.getInstance().get(lck.getKey());
			
			if (isUnlocked)
				addKeyModel();
			else
				GameState.getInstance().addListener(this);
		}
		
		public void stateChanged(String stateKey, object oldValue, object newValue) {
			if (stateKey.Equals(lck.getKey()) && ((bool) newValue) == true) {
				addKeyModel();
			}
		}
		
		private void addKeyModel() {
			GameObject key = (GameObject) Instantiate(keyModel, keyModel.transform.position, keyModel.transform.rotation);
			Vector3 p = key.transform.position;
			key.transform.parent = this.transform;
			key.transform.localPosition = p;
		}
	}
}

