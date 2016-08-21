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
			//hit.gameObject.GetComponent<Character_Controller>().setLife(0);
			colision=true;
		}

		if (hit.gameObject.tag == "Bear") {
			hit.gameObject.GetComponent<Bear_Controller>().applyDamage(50);
			colision=true;
		}

		if (hit.gameObject.tag == "Dart") {
			if(this.gameObject.transform.parent.gameObject.tag == "Bear"){
				this.gameObject.transform.parent.gameObject.GetComponent<Bear_Controller>().applyDamage(50);
				colision=true;
			}
			if(this.gameObject.transform.parent.gameObject.tag == "Snake"){
				this.gameObject.transform.parent.gameObject.GetComponent<Snake_Controller>().applyDamage();
				colision=true;
			}
			if(this.gameObject.transform.parent.gameObject.tag == "Crocodile"){
				this.gameObject.transform.parent.gameObject.GetComponent<Crocodile_Controller>().applyDamage();
				colision=true;
			}
			if(this.gameObject.transform.parent.gameObject.tag == "Tiger"){
				this.gameObject.transform.parent.gameObject.GetComponent<Tiger_Controller>().applyDamage(50);
				colision=true;
			}
		}


	}

}
