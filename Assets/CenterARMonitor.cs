using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CenterARMonitor : MonoBehaviour {
	[SerializeField] GameObject LeftCenter;
	[SerializeField] GameObject RightCenter;
	[SerializeField] LineRenderer lineRenderer;
	[SerializeField] LineRenderer skwiggleRenderer;
	[SerializeField] Camera arcam;
	[SerializeField] Collider backgroundCollider;

	private Vector3 LeftCenterSmooth;
	private Vector3 RightCenterSmooth;

	public Color c1 = Color.yellow;
	public Color c2 = Color.red;
	Vector3 cornerVector;

	// Use this for initialization
	void Start () {
		lineRenderer.SetVertexCount(2);
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(c1, c2);
		lineRenderer.SetWidth(0.2F, 0.2F);
	}

	List<Vector3> touchpoints;
	int counter = 0;
	bool release = false;
	// Update is called once per frame
	void Update () {
		LeftCenterSmooth = LeftCenter.transform.position;
		RightCenterSmooth = RightCenter.transform.position;

		Vector3 newCenterInfo = Vector3.Lerp (LeftCenterSmooth, RightCenterSmooth, .5f);
		cornerVector = (RightCenter.transform.position - LeftCenter.transform.position).normalized;

		float yFilter = (this.transform.position.y * .99f) + (newCenterInfo.y * .01f);
		float zFilter = (this.transform.position.z * .99f) + (newCenterInfo.z * .01f);

		lineRenderer.SetPosition (0,LeftCenterSmooth);
		lineRenderer.SetPosition (1,RightCenterSmooth);

		//backgroundCollider.transform.position = newCenterInfo;
		//backgroundCollider.transform.right = cornerVector;
		//this.transform.position = newCenterInfo; 
		//this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, zFilter);


		//this.transform.right = cornerVector.normalized;
		//this.transform.rotation = Quaternion.Euler (0f,this.transform.rotation.eulerAngles.y,this.transform.rotation.eulerAngles.z);
		/*
		if (Input.GetMouseButtonDown (0)) {
			release = false;
			touchpoints = new List<Vector3> ();
		}

		if (Input.GetMouseButton (0)) {
			touchpoints = new List<Vector3> ();
			Ray ray = arcam.ScreenPointToRay ( Input.mousePosition );

			RaycastHit hitInfo;
			if (Physics.Raycast (ray, out hitInfo)) {
				touchpoints.Add (hitInfo.point);
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			release = true;
			skwiggleRenderer.SetVertexCount (touchpoints.Count);
			skwiggleRenderer.material = new Material(Shader.Find("Particles/Additive"));
			skwiggleRenderer.SetColors(c1, c2);
			skwiggleRenderer.SetWidth(0.1F, 0.1F);
			//skwiggleRenderer.SetPositions (touchpoints);
		}

		if(release){
			counter++;
			int index = counter%touchpoints.Count;
			skwiggleRenderer.SetPosition (index, (touchpoints [index]+ (cornerVector * Time.deltaTime)));
		}
		*/
	}
}
