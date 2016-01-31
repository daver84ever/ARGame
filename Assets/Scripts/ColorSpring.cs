
using UnityEngine;
using System.Collections;

public class ColorSpring : MonoBehaviour {

	new Renderer renderer;
	public Vector4 velocity = Vector3.zero;
	public Vector4 target = Vector3.one;
	public Vector4 current{get{return renderer.sharedMaterial.color;}set{renderer.sharedMaterial.color = value;}}
	public float strength = 8.0f;
	public float dampingRatio = .8f;
	void Awake(){
		renderer = GetComponent<Renderer>();
	}

	// Update is called once per frame
	void Update () {
		current = Color.Lerp(current, Color.red, Time.deltaTime*.5f);
//		current = Utilities.SimpleHarmonicMotion(current, target, ref velocity, strength, dampingRatio);
	}
}
