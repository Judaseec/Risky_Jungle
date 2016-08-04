using UnityEngine;
using System.Collections;

public class Bear_Controller : MonoBehaviour {

	private float life = 150f;
	public bool dead = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (GameObject.Find("trigger").GetComponent<Trigger_Controller>().inside);
		if (GameObject.Find("Bear_Trigger").GetComponent<Trigger_Controller>().inside && !dead) {
			GetComponent<Animator> ().Play("atacar");
		}

		if (dead) {
			GetComponent<Animator> ().Play("morir");
		}
	}
}
