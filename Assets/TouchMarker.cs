using UnityEngine;
using System.Collections;

public class TouchMarker : MonoBehaviour {
	[SerializeField] Renderer render;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void Highlight(bool value){
		if(render.enabled != value){
			StartCoroutine (HighLightRoutine(value));
		}
	}

	IEnumerator HighLightRoutine(bool value){
		render.enabled = value;
		yield return new WaitForSeconds(1f);
		render.enabled = !value;
	}
}
