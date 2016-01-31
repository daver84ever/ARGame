using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Vuforia;
using System.Collections.Generic;
using System.Linq;

public class GlobalTargetFoundHandler : MonoBehaviour {
	public bool displayDebugText = true;
	public Text displayText;
	public DefaultTrackableEventHandler[] trackingEventHandlers;
	private Dictionary<int,DiceId> currentlyTracking; 

	public Action<DiceId> OnGlobalTargetFoundCallback;
	public Action<DiceId> OnGlobalTargetLostCallback;

	// Use this for initialization
	void Start () {
		for(int i=0; i<trackingEventHandlers.Length; i++){
			trackingEventHandlers [i].TargetFoundCallback += OnTargetFound;
			trackingEventHandlers [i].TargetLostCallback += OnTargetLost;

		}

		currentlyTracking = new Dictionary<int,DiceId>  ();
	}

	public void SetTrackableMarkers(bool value){
		for(int i=0; i<trackingEventHandlers.Length; i++){
			trackingEventHandlers [i].enabled = value;
			trackingEventHandlers [i].MarkerBehaviour.enabled = value;
		}

	}

	public DefaultTrackableEventHandler FindMarkerObject(int markerID){
		for(int i=0; i<trackingEventHandlers.Length; i++){
			if (trackingEventHandlers [i].MarkerID == markerID) {
				return trackingEventHandlers [i];
			}
		}

		return null;
	}

	public void OnTargetFound(DiceId found){
		if (OnGlobalTargetFoundCallback != null) {
			OnGlobalTargetFoundCallback (found);
		}
		Debug.Log ("OnTargetFound "+found.ToString());

		//if we already tracking the dice (compare markers to determine the best match [i.e. the marker on top])
		if (currentlyTracking.ContainsKey (found.diceIdx)) {
			//yes we do a quick search every time we find a new marker already in the system (we could optimize this search by caching or passing in the refs)
			DefaultTrackableEventHandler newlyFoundMarkerObj = FindMarkerObject (found.diceIdx);
			DefaultTrackableEventHandler alreadyCachedMarkerObj = FindMarkerObject (currentlyTracking [found.diceIdx].markerId);

			if(newlyFoundMarkerObj.MarkerID == alreadyCachedMarkerObj.MarkerID){
				Debug.LogError ("We are treating a marker we already found and cached as a new marker never seen before... ERROR. This is a sanity check.");
			} else {

				//udpate the found image we are tracking for a give die
				//FOR THE MOMENT NEW ONE ALWAYS WINS//
				currentlyTracking[found.diceIdx] = found;
				Debug.Log ("Update die[" + found.diceIdx + "] w/ "+found.type);
			}
				
		//if we are not tracking the die yet just ad it to the tracking dictionary
		} else {
			currentlyTracking.Add (found.diceIdx, found);
		}

		if(displayDebugText){
			UpdateDebugText ();
		}
	}

	public void OnTargetLost(DiceId lost){
		if (OnGlobalTargetLostCallback != null) {
			OnGlobalTargetLostCallback (lost);
		}
		Debug.Log ("OnTargetLost "+lost.ToString());

		if (currentlyTracking.ContainsKey (lost.diceIdx)) {
			currentlyTracking.Remove (lost.diceIdx);
		} else {
			Debug.LogWarning ("weirdly doesnt contain  "+lost.ToString()+" as a tracked marker");

		}

		if(displayDebugText){
			UpdateDebugText ();
		}
	}

	public void UpdateDebugText(){
		string output = "\n";
		foreach(DiceId diceId in currentlyTracking.Values){
			//output += diceId.type + "," + diceId.diceIdx +"\n";
			output += "Die["+diceId.diceIdx + "] shows a " + diceId.type +"\n";
		}
		displayText.text = output;
		Debug.Log (output);
	}


	public int NumberOfTrackedDie(){
		return currentlyTracking.Count;
	}

	public List<DiceId> GetTrackedDie(){
		return currentlyTracking.Values.ToList();;
	}

	public static Dictionary<DiceImageType,int> GetDiceTypeCounts(List<DiceId> cast){
		Dictionary<DiceImageType,int> castDic = new Dictionary<DiceImageType,int> ();

		for(int i=0; i<cast.Count; i++){
			DiceImageType castType = cast [i].type;
			if (castDic.ContainsKey (castType)) {
				castDic [castType]++;
			} else {
				castDic.Add (castType,1);
			}
		}
		return castDic;
	}

	public static bool CompareCasts(Dictionary<DiceImageType,int> a, Dictionary<DiceImageType,int> b){
		if(!CompareOnType(a,b,DiceImageType.BUG)){
			return false;
		}

		if(!CompareOnType(a,b,DiceImageType.BUNNY)){
			return false;
		}

		if(!CompareOnType(a,b,DiceImageType.CAT)){
			return false;
		}

		if(!CompareOnType(a,b,DiceImageType.CRAB)){
			return false;
		}

		if(!CompareOnType(a,b,DiceImageType.FROG)){
			return false;
		}

		if(!CompareOnType(a,b,DiceImageType.GHOST)){
			return false;
		}
		return true;
	}

	static bool CompareOnType(Dictionary<DiceImageType,int> a, Dictionary<DiceImageType,int> b, DiceImageType type ){
		int a_count = 0;
		int b_count = 0;
		if (a.ContainsKey (type)) {
			a_count = a[type];
		}

		if (b.ContainsKey (type)) {
			b_count = b[type];
		}

		if(a_count == b_count){
			return true;
		}
		return false;
	}
}

//List<TouchMarker>
// Update is called once per frame
//void Update () {
//not using touch controls this way//
/*
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
		*/
//}
