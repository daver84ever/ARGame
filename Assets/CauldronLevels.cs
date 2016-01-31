using UnityEngine;
using System.Collections;

public class CauldronLevels : MonoBehaviour {

	public ParticleSystem fire;
	public ParticleSystem bubles;
	public LocalPositionSpring spring;


	void Update(){

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			SetLevel (1);
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			SetLevel (2);

		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			SetLevel (3);

		} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
			SetLevel (4);

		}
	}

	public void SetLevel(int level){
		switch (level) {
		case 0:
		case 1:
			Level1 ();
			break;
		case 2:
			Level2 ();
			break;
		case 3:
			Level3 ();
			break;
		case 4:
		case 5:
			Level4 ();
			break;
			
		}
	}

	void ChangeEmission(ParticleSystem system, float val){
		var emission = system.emission;
		emission.rate = new ParticleSystem.MinMaxCurve(val);
		spring.velocity += new Vector3(0f, 1f, 0f) * 6f;
	}

	void Level1(){
		bubles.startSpeed = 20f;
		ChangeEmission (bubles, 20.0f);
		fire.startSize = 2f;
		ChangeEmission (fire, 20.0f);
	}

	void Level2(){
		bubles.startSpeed = 25f;
		ChangeEmission (bubles, 25f);
		fire.startSize = 2.5f;
		ChangeEmission (fire, 30f);

	}
	void Level3(){
		bubles.startSpeed = 30f;
		ChangeEmission (bubles, 30f);
		fire.startSize = 3f;
		ChangeEmission (fire, 40f);
	}
	void Level4(){
		bubles.startSpeed = 40f;
		ChangeEmission (bubles, 40f);
		fire.startSize = 4f;
		ChangeEmission (fire, 50f);

	}
}
