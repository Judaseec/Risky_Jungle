using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
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
	public bool down = false;
	public bool jump = false;
	
	public float xCamInit = 0f;
	public float xCharacter =0f;

	public int anim;
	public bool spear = false;

	void Awake() {
		animator = GetComponent<Animator>();
		animator.SetInteger ("animation", 0);
		anim = 0;
	}

	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate(){
		//Agregar fuerza de gravedad para que el personaje caiga mas rapido
		GetComponent<Rigidbody> ().AddForce (Physics.gravity * GetComponent<Rigidbody> ().mass);

		//Verificar si el ground checker esta colisionando con el suelo
		Collider[] hitColliders = Physics.OverlapSphere (groundChecker.position, radiusChecker, floorMask);
		isGrounded = (hitColliders.Length > 0);

		if (run) {
			//Modifica la velocidad y el personaje corre
			GetComponent<Rigidbody>().velocity = new Vector3(velocity, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
		}
	}
	
	// Update is called once per frame
	void Update () {
		anim = animator.GetInteger ("animation");

		if (Input.GetKeyDown (KeyCode.DownArrow) && isGrounded) {
			animator.SetInteger("animation", 11);
			down=true;
		}

		if(isGrounded && Input.GetKeyDown (KeyCode.UpArrow) && down){
			animator.SetInteger("animation", 12);
			down=false;
		}

		if(Input.GetKeyDown (KeyCode.Q)){
			if(spear){
				if(!down){
					//ataque lanza
				}else{
					//Advertencia que no puede atacar agachado
				}
			} else{
				if(down){
					animator.SetInteger("animation", 13);
					StartCoroutine(goDown(1));

				}else{
					animator.SetInteger("animation", 5);
					StartCoroutine(stand(1));
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			if(down){
				animator.SetInteger("animation", 15);
				down=false;
				run=true;
			}else if(!down && isGrounded){
				animator.SetInteger("animation", 1);
				run=true;
			}else if(!isGrounded){
				animator.SetInteger("animation", 8);
				jump=false;
				run=true;
			}
		}

		if (Input.GetKeyUp (KeyCode.RightArrow)) {
			run=false;
			animator.SetInteger("animation", 2);
		}

		if (Input.GetKeyDown (KeyCode.UpArrow) && !run && !down){
			GetComponent<Rigidbody> ().AddForce (new Vector2 (0, jumpForce));
			StartCoroutine(waitJump());
		}

		if (Input.GetKeyDown (KeyCode.UpArrow) && run && !down) {
			GetComponent<Rigidbody> ().AddForce (new Vector2 (0, jumpForce));
			StartCoroutine(runJump());
		}

		if (isGrounded && !run && jump) {
			animator.SetInteger("animation", 4);
			jump = false;
		}
	}

	IEnumerator goDown(int time){
		yield return new WaitForSeconds(time);
		animator.SetInteger("animation", 14);
	}

	IEnumerator stand(int time){
		yield return new WaitForSeconds(time);
		animator.SetInteger("animation", 6);
	}

	IEnumerator waitJump(){
		while (isGrounded) {
			yield return null;
		}
		animator.SetInteger("animation", 3);
		jump = true;
	}

	IEnumerator runJump(){
		while (isGrounded) {
			yield return null;
		}
		animator.SetInteger("animation", 7);
		jump = true;
	}
}
