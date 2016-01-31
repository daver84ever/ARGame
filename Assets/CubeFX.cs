using UnityEngine;
using System.Collections;
using Vuforia;

public class CubeFX : MonoBehaviour {

	DefaultTrackableEventHandler eventer;

	void Awake(){
		eventer = GetComponentInParent<DefaultTrackableEventHandler>();
		eventer.TargetFoundCallback = (s)=>{OnDetectedCube();};
		eventer.TargetLostCallback = (s)=>{OnLoseCube();};
	}

	public void OnDetectedCube(){
	}

	void Update(){
	}

	public void OnLoseCube(){
	}

}
