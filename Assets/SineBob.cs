using UnityEngine;
using System.Collections;

public class SineBob : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.localPosition += new Vector3 (0f, Mathf.Sin (Time.time / 1f) * .001f, 0f);
	}
}
