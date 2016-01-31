using UnityEngine;
using System.Collections;

public class CauldronLevels : MonoBehaviour {

	public ParticleSystem fire;
	public ParticleSystem bubles;


	public void SetLevel(int level){
		switch (level) {
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
			Level4 ();
			break;
			
		}
	}

	void Level1(){
		bubles.startSpeed = 20f;
		bubles.emission = 30f;
		fire.startSize = 2f;
		fire.emission = 20f;
	}

	void Level2(){
		bubles.startSpeed = 25f;
		bubles.emission = 30f;
		fire.startSize = 2.5f;
		fire.emission = 30f;

	}
	void Level3(){
		bubles.startSpeed = 30f;
		bubles.emission = 40f;
		fire.startSize = 3f;
		fire.emission = 40f;
	}
	void Level4(){
		bubles.startSize = 40f;
		bubles.emission = 40f;
		fire.startSize = 4f;
		fire.emission = 50f;

	}
}
