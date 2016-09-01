using UnityEngine;
using System.Collections;
using Assets.Scripts.com.ethereal.util;
using Assets.Scripts.com.ethereal.appsSystem;

public class Character_Controller : MonoBehaviour {

	public float jumpForce = 500f;

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
	private bool attack = false;

	public float xCamInit = 0f;
	public float xCharacter =0f;
	private bool isSpear = true;
	private bool isPaused = false;
	private GameObject pausePanel;
	private GameObject controlPanel;
	private GameObject attackPanel;
	private GameObject gameOverPanel;

	//Controles
	private bool rightControl;
	private bool leftControl;
	private bool attackControl;
	private bool jumpControl;
	private bool downControl;

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
		pausePanel = GameObject.Find("PauseMenuPanel");
		attackPanel = GameObject.Find("AttackPanel");
		controlPanel = GameObject.Find("ControlPanel");
		gameOverPanel = GameObject.Find("GameOverPanel");
		pausePanel.SetActive (false);
		gameOverPanel.SetActive (false);
#if UNITY_STANDALONE || UNITY_WEBPLAYER
		attackPanel.SetActive(false);
		controlPanel.SetActive(false);
#endif
	}

	void FixedUpdate(){
		//Validaciones para cambiar el estado de las variables de los controles
		//Validaciones para windows

#if UNITY_STANDALONE || UNITY_WEBPLAYER
		//salto
		if(Input.GetKeyDown (KeyCode.UpArrow)){
			jumpControl = true;
		} else{
			jumpControl = false;
		}

		//correr a la derecha
		if(Input.GetKey (KeyCode.RightArrow)){
			rightControl = true;
		}else{
			rightControl = false;
			NotificationCenter.DefaultCenter ().PostNotification (this, "Character_is_not_running");
		}

		//Correr a la izquierda
		if(Input.GetKey (KeyCode.LeftArrow)){
			leftControl = true;
		}else{
			leftControl = false;
		}

		//Agacharse
		if(Input.GetKey (KeyCode.DownArrow)){
			downControl = true;
		}else{
			downControl = false;
		}

		//Atacar
		if(Input.GetKey (KeyCode.Q)){
			attackControl = true;
		}else{
			attackControl = false;
		}
#elif UNITY_ANDROID

#endif
		if (life > 0) {
			GetComponent<Rigidbody> ().AddForce (Physics.gravity * GetComponent<Rigidbody> ().mass);

			Collider[] hitColliders = Physics.OverlapSphere (groundChecker.position, radiusChecker, floorMask);
			isGrounded = (hitColliders.Length > 0);

			if(xCamInit <= (xCharacter + 3f) && !runBack){
				GameObject.Find ("Camera").GetComponent<Follow_Character>().enabled = true;
			}

			if (run) {
				GetComponent<Rigidbody>().velocity = new Vector3(velocity, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
				//NotificationCenter.DefaultCenter ().PostNotification (this, "Character_is_running");
			}

			if (runBack) {
				GetComponent<Rigidbody>().velocity = new Vector3(-velocity, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
				GameObject cam = GameObject.Find ("Camera");
				xCamInit = cam.transform.position.x;
				cam.GetComponent<Follow_Character>().enabled = false;
			}


			if (isGrounded && !run && !runBack && !down && !attack) {
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
			if (isGrounded && jumpControl && !down && !isPaused) {
				GetComponent<Rigidbody> ().AddForce (new Vector2 (0, jumpForce));
			} else if (isGrounded && jumpControl && down && !isPaused) {
				animator.Play ("agachado");
				down = false;
			}

			if (!isGrounded) {
				animator.Play ("jump");
			}

			if (rightControl && isGrounded && !isPaused) {
				run = true;
				down = false;
				NotificationCenter.DefaultCenter ().PostNotification (this, "Character_is_running");
				animator.Play ("run");
			} else {
				run = false;
			}

			if (leftControl && isGrounded && !isPaused) {
				runBack = true;
				down = false;
				//NotificationCenter.DefaultCenter ().PostNotification (this, "Character_is_running_back");
				animator.Play ("run");
			} else {
				runBack = false;
			}

			if (downControl && isGrounded && !isPaused) {
				down = true;
				animator.Play ("agachado");
			}

			if (leftControl && !turned && !isPaused) {
				turnCharacter ();
				turned = true;
			}

			if (rightControl && turned && !isPaused) {
				turnCharacter ();
				turned = false;
			}

			if (attackControl && !isPaused) {
				if(!isSpear){
					if (down && !attack) {
						attack=true;
						animator.Play ("disparo_agachado");
						new EthTimer (500, createDartDown);
						new EthTimer (1000, attackAction);
						new EthTimer (1000, stayDown);
					} else if(!attack){
						attack=true;
						animator.Play ("ataque_cerbatana");
						new EthTimer (500, createDart);
						new EthTimer (1000, attackAction);
					}
				}else{
					if (isGrounded && !run && !runBack && !down && !turned && !attack && !isPaused) {
						attack = true;
						animator.Play ("ataque_lanza");
						new EthTimer (1500, attackAction);
					}
				}
			}
		}
	}

	void OnCollisionEnter(Collision hit) {

		if(hit.gameObject.tag == "Fall"){
			new EthTimer(1500, showGameOver);
		}
	}

	void turnCharacter(){
		GetComponent<Transform> ().Rotate(0,180,0,Space.World);
	}

	public void stayDown(object stay){
		animator.Play ("idle");
		down=false;
	}

	public void attackAction(object obj){
		attack = false;
	}

	public void createDart(object obj){
		//Util.GetChildByName (this.gameObject,"cervatana").GetComponent<Dart_Generator_Controller>().generate();
		Rigidbody dartInstance;
		dartInstance = Instantiate (dartPrefab, new Vector3(blowgunEnd.position.x + 1, blowgunEnd.position.y, blowgunEnd.position.z), blowgunEnd.rotation) as Rigidbody;
		dartInstance.AddForce (blowgunEnd.forward * 400);
	}

	public void createDartDown(object obj){
		//Util.GetChildByName (this.gameObject,"cervatana").GetComponent<Dart_Generator_Controller>().generate();
		Rigidbody dartInstance;
		dartInstance = Instantiate (dartPrefab, new Vector3(blowgunEnd.position.x + 1, blowgunEnd.position.y - 1, blowgunEnd.position.z), blowgunEnd.rotation) as Rigidbody;
		dartInstance.AddForce (blowgunEnd.forward * 400);
	}

	//Funciones para el HUD
	public void setLife(int actual_life){
		life = actual_life;

		if(life == 0){
			new EthTimer(1500, showGameOver);
		}
	}

	public void setSpear(){
		isSpear = true;
	}

	public void setBlowgun(){
		isSpear = false;
	}

	public void showGameOver(object obj){
		gameOverPanel.SetActive (true);
	}

	//Funciones para el menu de pausa
	public void pause(){
		isPaused = true;
		pausePanel.SetActive (true);
	}

	public void resume(){
		pausePanel.SetActive (false);
		isPaused = false;
	}

	public void restart(){
		Application.LoadLevel (Save.savedGame.level+1);
	}

	public void menu(){
		SceneHandler.LoadScene ("Main_Menu");
	}

	public void upBtnDown(){
		jumpControl = true;
	}

	public void upBtnUp(){
		jumpControl = false;
	}

	public void downBtnDown(){
		downControl = true;
	}
	
	public void downBtnUp(){
		downControl = false;
	}

	public void rigthBtnDown(){
		rightControl = true;
	}
	
	public void rightBtnUp(){
		rightControl = false;
	}

	public void leftBtnDown(){
		leftControl = true;
	}
	
	public void leftBtnUp(){
		leftControl = false;
	}

	public void attackBtnDown(){
		attackControl = true;
	}
	
	public void atackBtnUp(){
		attackControl = false;
	}
}
