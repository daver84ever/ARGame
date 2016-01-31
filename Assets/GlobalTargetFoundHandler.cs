using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Vuforia;
using System.Collections.Generic;

public class GlobalTargetFoundHandler : MonoBehaviour {
	public bool displayDebugText = true;
	public Text displayText;
	public DefaultTrackableEventHandler[] trackingEventHandlers;
	public Dictionary<DiceId, bool> currentlyTracking; 

	public Action<DiceId> OnGlobalTargetFoundCallback;
	public Action<DiceId> OnGlobalTargetLostCallback;

	// Use this for initialization
	void Start () {
		for(int i=0; i<trackingEventHandlers.Length; i++){
			trackingEventHandlers [i].TargetFoundCallback += OnTargetFound;
			trackingEventHandlers [i].TargetLostCallback += OnTargetLost;

		}

		currentlyTracking = new Dictionary<DiceId, bool>  ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTargetFound(DiceId found){
		if (OnGlobalTargetFoundCallback != null) {
			OnGlobalTargetFoundCallback (found);
		}
		Debug.Log ("OnTargetFound "+found.ToString());

		currentlyTracking.Add (found, true);



		if(displayDebugText){
			UpdateDebugText ();
		}
	}

	public void OnTargetLost(DiceId lost){
		if (OnGlobalTargetLostCallback != null) {
			OnGlobalTargetLostCallback (lost);
		}
		Debug.Log ("OnTargetLost "+lost.ToString());

		if (currentlyTracking.ContainsKey (lost)) {
			currentlyTracking.Remove (lost);
		} else {
			Debug.LogWarning ("weirdly doesnt contain  "+lost.ToString()+" as a tracked marker");

		}

		if(displayDebugText){
			UpdateDebugText ();
		}
	}

	public void UpdateDebugText(){
		string output = "";
		foreach(DiceId diceId in currentlyTracking.Keys){
			output += diceId.type + "," + diceId.diceIdx +"\n";
		}
		displayText.text = output;
		Debug.Log (output);
	}
}
