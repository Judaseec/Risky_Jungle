using UnityEngine;
using System.Collections;
using Assets.Scripts.com.ethereal.util;

public class Character_Controller : MonoBehaviour {

	public float jumpForce = 1000f;

	public bool isGrounded = true;
	public Transform groundChecker;
	private float radiusChecker = 0.03f;
	public LayerMask floorMask;

	private Animator animator;
	public bool run = false;
	public bool runBack = false;
	public float velocity = 10f;
	public bool turned = false;
	public bool down = false;
	public int life = 100;
	private bool atack = false;

	public float xCamInit = 0f;
	public float xCharacter =0f;

	/**
	 * 	@breaf variables para el disparo del dardo
	 */
	public Rigidbody dartPrefab;
	public Transform blowgunEnd;
	
	void Awake() {

	}

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}

	void FixedUpdate(){
		if (life > 0) {
			GetComponent<Rigidbody> ().AddForce (Physics.gravity * GetComponent<Rigidbody> ().mass);

			Collider[] hitColliders = Physics.OverlapSphere (groundChecker.position, radiusChecker, floorMask);
			isGrounded = (hitColliders.Length > 0);

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


			if (isGrounded && !run && !runBack && !down && !atack) {
				animator.Play ("idle");
			}
		} else {
			animator.Play("morir");
		}
	}
	
	// Update is called once per frame
	void Update () {
		xCharacter = transform.position.x;
		if (life > 0) {
			if (isGrounded && Input.GetKeyDown (KeyCode.UpArrow) && !down) {
				GetComponent<Rigidbody> ().AddForce (new Vector2 (0, jumpForce));
			} else if (isGrounded && Input.GetKeyDown (KeyCode.UpArrow) && down) {
				animator.Play ("agachado");
				down = false;
			}

			if (!isGrounded) {
				animator.Play ("jump");
			}

			if (Input.GetKey (KeyCode.RightArrow) && isGrounded) {
				run = true;
				down = false;
				NotificationCenter.DefaultCenter ().PostNotification (this, "Character_is_running");
				animator.Play ("run");
			} else {
				run = false;
			}

			if (Input.GetKey (KeyCode.LeftArrow) && isGrounded) {
				runBack = true;
				down = false;
				//NotificationCenter.DefaultCenter ().PostNotification (this, "Character_is_running_back");
				animator.Play ("run");
			} else {
				runBack = false;
			}

			if (Input.GetKeyDown (KeyCode.DownArrow) && isGrounded) {
				down = true;
				animator.Play ("agachado");
			}

			if (Input.GetKeyDown (KeyCode.LeftArrow) && !turned) {
				turnCharacter ();
				turned = true;
			}

			if (Input.GetKeyDown (KeyCode.RightArrow) && turned) {
				turnCharacter ();
				turned = false;
			}

			if (Input.GetKeyDown (KeyCode.Q)) {
				if (down) {
					animator.Play ("disparo_agachado");
					new EthTimer (1000, stayDown);
				} else if(!atack){
					atack=true;
					animator.Play ("ataque_cerbatana");
					new EthTimer (500, createDart);
					new EthTimer (1000, attack);
				}
			}

			if (Input.GetKeyDown (KeyCode.W) && isGrounded && !run && !runBack && !down && !turned && !atack) {
				atack = true;
				animator.Play ("ataque_lanza");
				new EthTimer (1500, attack);
			}
		}
	}

	void turnCharacter(){
		GetComponent<Transform> ().Rotate(0,180,0,Space.World);
	}

	public void stayDown(object stay){
		animator.Play ("idle");
		down=false;
	}

	public void attack(object obj){
		atack = false;
	}

	public void createDart(object obj){
		//Util.GetChildByName (this.gameObject,"cervatana").GetComponent<Dart_Generator_Controller>().generate();
		Rigidbody dartInstance;
		dartInstance = Instantiate (dartPrefab, new Vector3(blowgunEnd.position.x + 1, blowgunEnd.position.y, blowgunEnd.position.z), blowgunEnd.rotation) as Rigidbody;
		dartInstance.AddForce (blowgunEnd.forward * 400);
	}

	public void setLife(int actual_life){
		life = actual_life;
	}


}
