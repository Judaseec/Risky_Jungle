using UnityEngine;
using System.Collections;

public class ControladorPersonaje : MonoBehaviour {

	public float jumpForce = 1500f;

	private bool isGrounded = true;
	public Transform groundChecker;
	private float radiusChecker = 0.03f;
	public LayerMask floorMask;

	private Animator animator;
	public bool run = false;
	public float velocity = 1f;

	void Awake() {
		animator = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {

	}

	void FixedUpdate(){

		GetComponent<Rigidbody> ().AddForce (Physics.gravity * GetComponent<Rigidbody> ().mass);

		Collider[] hitColliders = Physics.OverlapSphere (groundChecker.position, radiusChecker, floorMask);
		isGrounded = (hitColliders.Length > 0);
		//animator.SetBool ("isGrounded", isGrounded);

		if (run) {
			GetComponent<Rigidbody>().velocity = new Vector3(velocity, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);

		}
		animator.SetFloat ("velX", GetComponent<Rigidbody>().velocity.x);
	}
	
	// Update is called once per frame
	void Update () {
		if (isGrounded && Input.GetKeyDown(KeyCode.UpArrow)) {
			GetComponent<Rigidbody>().AddForce(new Vector2(0, jumpForce));
			GetComponent<Animator> ().Play("jump");
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			run = true;
		} else {
			run = false;
		}
	}
}
