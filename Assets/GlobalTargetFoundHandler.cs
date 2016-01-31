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
	public Dictionary<DiceImageType, int> currentlyTracking; 

	public Action<DiceImageType> OnGlobalTargetFoundCallback;
	public Action<DiceImageType> OnGlobalTargetLostCallback;

	// Use this for initialization
	void Start () {
		for(int i=0; i<trackingEventHandlers.Length; i++){
			trackingEventHandlers [i].TargetFoundCallback += OnTargetFound;
			trackingEventHandlers [i].TargetLostCallback += OnTargetLost;

		}

		currentlyTracking = new Dictionary<DiceImageType, int>  ();
	}

	//List<TouchMarker>
	// Update is called once per frame
	void Update () {
		for (var i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch(i).phase == TouchPhase.Began ||Input.GetTouch(i).phase == TouchPhase.Moved  ) {

				// Construct a ray from the current touch coordinates
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
				// Create a particle if hit
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit)){
					//Instantiate(particle, transform.position, transform.rotation);
					Debug.Log(hit.transform.parent.name);
					TouchMarker touchMarker = hit.collider.gameObject.GetComponent<TouchMarker> ();
					touchMarker.Highlight(true);
				}
			}
				

			//if (Input.GetTouch(i).phase == TouchPhase.Moved) {}
		}

		if (Input.GetMouseButton (0)) {
			// Construct a ray from the current touch coordinates
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// Create a particle if hit
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				//Instantiate(particle, transform.position, transform.rotation);
				Debug.Log(hit.transform.parent.name);
				TouchMarker touchMarker = hit.collider.gameObject.GetComponent<TouchMarker> ();
				touchMarker.Highlight(true);
			}
		}
	}

	public void OnTargetFound(DiceImageType found){
		if (OnGlobalTargetFoundCallback != null) {
			OnGlobalTargetFoundCallback (found);
		}
		Debug.Log ("OnTargetFound "+found.ToString());

		if (currentlyTracking.ContainsKey (found)) {
			currentlyTracking [found]++;
		} else {
			currentlyTracking.Add (found,1);
		}

		if(displayDebugText){
			UpdateDebugText ();
		}
	}

	public void OnTargetLost(DiceImageType lost){
		if (OnGlobalTargetLostCallback != null) {
			OnGlobalTargetLostCallback (lost);
		}
		Debug.Log ("OnTargetLost "+lost.ToString());

		if (currentlyTracking.ContainsKey (lost)) {
			if (currentlyTracking [lost] < 2) {
				currentlyTracking.Remove (lost);
			} else {
				currentlyTracking [lost]--;
			}
		} else {
			Debug.LogWarning ("weirdly doesnt contain  "+lost.ToString()+" as a tracked marker");

		}

		if(displayDebugText){
			UpdateDebugText ();
		}
	}

	public void UpdateDebugText(){
		string output = "";
		foreach(KeyValuePair<DiceImageType,int> entry in currentlyTracking){
			output += entry.Key+","+entry.Value.ToString()+"\n";
		}
		displayText.text = output;
		Debug.Log (output);
	}
}
