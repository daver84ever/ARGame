using UnityEngine;
using System.Collections;
using System;

public class GlobalTargetFoundHandler : MonoBehaviour {

	public Action<DiceImageType> OnGlobalTargetFoundCallback;
	public Action<DiceImageType> OnGlobalTargetLostCallback;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTargetFound(DiceImageType found){
		if (OnGlobalTargetFoundCallback == null) {
			OnGlobalTargetFoundCallback (found);
		}
	}

	public void OnTargetLost(DiceImageType last){
		if (OnGlobalTargetLostCallback == null) {
			OnGlobalTargetLostCallback (last);
		}
	}
}
