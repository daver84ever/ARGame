using UnityEngine;
using System.Collections;

public class ModelController : MonoBehaviour {

	public GameObject prefab;
	private ScaleSpring sspring;
	private LocalPositionSpring posspring;
	public Vector3 scale = new Vector3(5f,5f,5f);
	public Vector3 pos = new Vector3 (0f, .5f, .0f);

	void Awake(){
		var g = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
		g.transform.SetParentZeroed (this.transform);
		g.transform.localScale = scale;
		g.transform.localPosition = new Vector3 (0f, .5f, 0f);
		sspring = GetComponent<ScaleSpring> ();
		sspring.target = Vector3.zero;
		posspring = GetComponent<LocalPositionSpring> ();
	}

//	void OnEnable(){
//		FadeIn ();
////		Invoke ("FadeOut", 1f);
//	}

	public void FadeIn(){
		sspring.transform.localScale = Vector3.zero;
		sspring.current = Vector3.zero;
		sspring.target = Vector3.one * 2f;
		posspring.current = Vector3.zero;
		posspring.target = Vector3.zero;
		Debug.Log (sspring.current);
	}

	public void FadeOut(){
		sspring.target = Vector3.zero;
		sspring.velocity += Vector3.one * 10f;
		posspring.target = new Vector3 (0f, 1f, 0f) * 5f;
	}


	void Update(){
		if(Input.GetKeyDown("space")){
			FadeIn ();
		}
		if(Input.GetKeyDown("u")){
			FadeOut ();
		}
	}

}
