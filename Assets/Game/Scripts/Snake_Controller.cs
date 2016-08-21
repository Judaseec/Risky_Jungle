using UnityEngine;
using System.Collections;

public class Snake_Controller : MonoBehaviour {

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
		else if (dead) {
			GetComponent<Animator> ().Play("morir");
			Util.GetChildByName(gameObject,"serpienteModel").GetComponent<MeshCollider>().isTrigger = true;
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

	public void applyDamage(){
		dead = true;
	}
}
