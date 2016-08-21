using UnityEngine;
using System.Collections;

public class Bear_Controller : MonoBehaviour {

	public float life = 150f;
	public bool dead = false;
	public bool inside = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (GameObject.Find("trigger").GetComponent<Trigger_Controller>().inside);
		if (life <= 0) {
			dead = true;
		}

		if (inside && !dead) {
			GetComponent<Animator> ().Play("atacar");
		}
		else if (!inside && !dead){
			GetComponent<Animator> ().Play("idle");
		}

		if (dead) {
			GetComponent<Animator> ().Play("morir");
			//Util.GetChildByName(gameObject,"oso").GetComponent<Combat_Controller>().enabled = false;
			Util.GetChildByName(gameObject,"osoModel").GetComponent<MeshCollider>().isTrigger = true;
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

	public void applyDamage(int damage){
		life = life - damage;
	}
}
