using UnityEngine;
using System.Collections;

public class Crocodile_Controller : MonoBehaviour {

	public bool sleep = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find("Crocodile_Trigger").GetComponent<Trigger_Controller>().inside && !sleep) {
			GetComponent<Animator> ().Play("ataque");
		}

		if (sleep) {
			GetComponent<Animator> ().Play("muerte");
		}
	}
}
