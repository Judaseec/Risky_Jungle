using UnityEngine;
using System.Collections;

public class Combat_Controller : MonoBehaviour {

	public bool enter = false;
	public bool colision = false;
	private Collision hitobj;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (colision) {
			//hitobj.gameObject.GetComponent<Animator>().Stop();
		}
	}

	void OnCollisionEnter(Collision hit) {

		if (hit.gameObject.tag == "Principal") {
			hitobj = hit;
				colision=true;
		}
	}

	void OnTriggerEnter(Collider hit){
		//Si el objeto que entra al trigger tiene el tag principal...
		if (hit.tag == "Principal") {
			hit.gameObject.GetComponent<Animator>().Play("morir");
			enter = true;
		}
	}
	
	void OnTriggerExit(Collider hit){
		//Si el objeto que sale del trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			//inside=false;
		}
		enter = false;
	}
}
