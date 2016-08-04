using UnityEngine;
using System.Collections;

public class Snake_Controller : MonoBehaviour {

	public bool dead = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find("Snake_Trigger").GetComponent<Trigger_Controller>().inside && !dead) {
			GetComponent<Animator> ().Play("ataque");
		}

		if (dead) {
			GetComponent<Animator> ().Play("morir");
		}
	}
}
