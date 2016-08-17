﻿using UnityEngine;
using System.Collections;

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
		else if (dead) {
			GetComponent<Animator> ().Play("muerte");
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
}
