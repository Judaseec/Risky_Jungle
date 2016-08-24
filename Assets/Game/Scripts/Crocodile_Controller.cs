using UnityEngine;
using System.Collections;
using Assets.Scripts.com.ethereal.util;

public class Crocodile_Controller : MonoBehaviour {

	public bool sleep = false;
	private float life = 150f;
	public bool dead = false;
	public bool inside = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (inside && !dead) {
			GetComponent<Animator> ().Play("ataque");
		}
		else if (!inside && !dead){
			GetComponent<Animator> ().Play("idle");
		}

		if (dead) {
			GetComponent<Animator> ().Play("muerte");
			Util.GetChildByName(gameObject,"cocodriloModel").GetComponent<BoxCollider>().isTrigger = false;

			new EthTimer (5000, reviveAction);
		}
		
		
	}
	
	void OnTriggerEnter(Collider hit){
		//Si el objeto que entra al trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			inside=true;
		}
	}
	
	void OnTriggerExit(Collider hit){
		//Si el objeto que sale del trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			inside=false;
		}
	}

	public void reviveAction(object stay){
		GetComponent<Animator> ().Play ("idle");
		dead=false;
	}

	public void applyDamage(){
		dead = true;
	}

	public bool getDead(){
		return dead;
	}
}
