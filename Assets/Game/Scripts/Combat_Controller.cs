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
		Debug.Log (hit);
		if (hit.gameObject.tag == "Principal") {
			hitobj = hit;
			hit.gameObject.GetComponent<Character_Controller>().setLife(0);
			colision=true;
		}
	}

}
