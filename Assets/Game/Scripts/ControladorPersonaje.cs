﻿using UnityEngine;
using System.Collections;

public class ControladorPersonaje : MonoBehaviour {

	public float jumpForce = 300f;

	public bool isGrounded = true;
	public Transform groundChecker;
	private float radiusChecker = 0.07f;
	public LayerMask floorMask;

	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate(){
		Collider[] hitColliders = Physics.OverlapSphere (groundChecker.position, radiusChecker, floorMask);
		isGrounded = (hitColliders.Length > 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("up")) {
			GetComponent<Rigidbody>().AddForce(new Vector2(0, jumpForce));
		}
	}
}
