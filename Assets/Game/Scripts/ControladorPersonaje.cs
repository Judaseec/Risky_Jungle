using UnityEngine;
using System.Collections;

public class ControladorPersonaje : MonoBehaviour {

	public float jumpForce = 1500f;

	public bool isGrounded = true;
	public Transform groundChecker;
	private float radiusChecker = 0.03f;
	public LayerMask floorMask;

	private Animator animator;
	public bool run = false;
	public bool runBack = false;
	public float velocity = 1f;
	public bool turned = false;

	public float xCamInit = 0f;
	public float xCharacter =0f;

	void Awake() {
		animator = GetComponent<Animator>();
		Debug.Log (animator);
	}

	// Use this for initialization
	void Start () {

	}

	void FixedUpdate(){

		GetComponent<Rigidbody> ().AddForce (Physics.gravity * GetComponent<Rigidbody> ().mass);

		Collider[] hitColliders = Physics.OverlapSphere (groundChecker.position, radiusChecker, floorMask);
		isGrounded = (hitColliders.Length > 0);
		//animator.SetBool ("isGrounded", isGrounded);

		if(xCamInit <= (xCharacter + 3f) && !runBack){
			GameObject.Find ("Camera").GetComponent<Follow_Character>().enabled = true;
		}

		if (run) {
			GetComponent<Rigidbody>().velocity = new Vector3(velocity, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
		}

		if (runBack) {
			GetComponent<Rigidbody>().velocity = new Vector3(-velocity, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
			GameObject cam = GameObject.Find ("Camera");
			xCamInit = cam.transform.position.x;
			cam.GetComponent<Follow_Character>().enabled = false;
		}
		animator.SetFloat ("velX", GetComponent<Rigidbody>().velocity.x);

		if(isGrounded && !run && !runBack){
			GetComponent<Animator> ().Play("idle");
		}
	}
	
	// Update is called once per frame
	void Update () {
		xCharacter = transform.position.x;

		if (isGrounded && Input.GetKeyDown(KeyCode.UpArrow)) {
			GetComponent<Rigidbody>().AddForce(new Vector2(0, jumpForce));
		}

		if (!isGrounded) {
			GetComponent<Animator> ().Play("jump");
		}

		if (Input.GetKey (KeyCode.RightArrow) && isGrounded) {
			run = true;
			NotificationCenter.DefaultCenter().PostNotification(this, "Character_is_running");
			GetComponent<Animator> ().Play("run");
		} else {
			run = false;
		}

		if (Input.GetKey (KeyCode.LeftArrow) && isGrounded) {
			runBack = true;
			//NotificationCenter.DefaultCenter ().PostNotification (this, "Character_is_running_back");
			GetComponent<Animator> ().Play ("run");
		} else {
			runBack = false;
		}

		if(Input.GetKeyDown (KeyCode.LeftArrow) && !turned){
			turnCharacter();
			turned = true;
		}

		if(Input.GetKeyDown (KeyCode.RightArrow) && turned){
			turnCharacter();
			turned = false;
		}
	}

	void turnCharacter(){
		GetComponent<Transform> ().Rotate(0,180,0,Space.World);
	}
}
