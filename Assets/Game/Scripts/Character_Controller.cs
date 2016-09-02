using UnityEngine;
using System.Collections;
using Assets.Scripts.com.ethereal.util;
using Assets.Scripts.com.ethereal.appsSystem;

/**
*	@class Character_Controller
*	@brief Clase que controla las interacciones y movimientos del personaje principal.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class Character_Controller : MonoBehaviour {

	/**
	*	@brief Variable que controla la fuerza del salto del personaje.
	*/
	public float jumpForce = 500f;

	/**
	*	@brief Variable que define si el personaje se encuentra sobre un terreno.
	*/
	public bool isGrounded = true;

	/**
	*	@brief Componente transform que s eutiliza para verificar si el personaje está sobre un terreno.
	*/
	public Transform groundChecker;

	/**
	*	@brief Radio de detección del ground checker.
	*/
	private float radiusChecker = 0.03f;

	/**
	*	@brief Layer para definir los game objects de tipo terreno.
	*/
	public LayerMask floorMask;

	/**
	*	@brief Variable que representa el componente animator del personaje principal.
	*/
	private Animator animator;

	/**
	*	@brief Variable que define si el personaje está corriendo.
	*/
	public bool run = false;

	/**
	*	@brief Variable que define si el personaje está corriendo hacia atras.
	*/
	public bool runBack = false;

	/**
	*	@brief Velocidad de movimiento del personaje principal.
	*/
	public float velocity = 10f;

	/**
	*	@brief Variable que indica si el personaje esta mirando hacia atrás.
	*/
	public bool turned = false;

	/**
	*	@brief Variable que define si el personaje se encuentra agachado.
	*/
	public bool down = false;

	/**
	*	@brief Puntos de vida del personaje.
	*/
	public int life = 100;

	/**
	*	@brief Variable que define si el personaje esta realizando un ataque.
	*/
	private bool attack = false;

	public float xCamInit = 0f;

	/**
	*	@brief Posición inicial de la cámara.
	*/
	public float xCharacter =0f;

	/**
	*	@brief Variable que define si la lanza esta seleccionada como arma.
	*/
	private bool isSpear = true;

	/**
	*	@brief Variable que define si el juego está pausado.
	*/
	private bool isPaused = false;

	/**
	*	@brief Game object que representa la ventana de pausa.
	*/
	private GameObject pausePanel;

	/**
	*	@brief Game object que representa el panel donde se encuentran los controles de movimiento.
	*/
	private GameObject controlPanel;

	/**
	*	@brief Game object que representa el panel donde se encuentran los controles de ataque.
	*/
	private GameObject attackPanel;

	/**
	*	@brief Game object que representa la ventana de game over.
	*/
	private GameObject gameOverPanel;

	//Controles
	/**
	*	@brief Variable que define si el botón de desplazamiento hacia la derecha está presionado.
	*/
	private bool rightControl;

	/**
	*	@brief Variable que define si el botón de desplazamiento hacia la izquierda está presionado.
	*/
	private bool leftControl;

	/**
	*	@brief Variable que define si el botón de ataque está presionado.
	*/
	private bool attackControl;

	/**
	*	@brief Variable que define si el botón de salto está presionado.
	*/
	private bool jumpControl;

	/**
	*	@brief Variable que define si el botón de agacharse está presionado.
	*/
	private bool downControl;

	/**
	 * 	@breaf variable para el disparo del dardo
	 */
	public Rigidbody dartPrefab;

	/**
	 * 	@breaf variable para el disparo del dardo
	 */
	public Transform blowgunEnd;
	
	/**
	* 	@breaf Método que se ejecuta una vez se ejecuta la escena.
	*
	*	Inicializa el componente animator, oculta las pantallas de game over y pausa para permitirle al usuario jugar.
	*	Si la plataforma es móvil muestra los controles de desplazamiento.
	*/
	public void Start () {
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

	/**
	* 	@breaf Método que configura la interacción de los controles en caso de que la plataforma no sea móvil.
	*
	*	Ademas valida que la cámara no siga al personaje si este se devuelve.
	*/
	public void FixedUpdate(){
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
	
	/**
	* 	@breaf Este método se ejecuta una vez por frame y en él se hacen las validaciones de las animaciones del personaje principal.
	*/
	public void Update () {
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

	/**
	* 	@breaf Método para detectar cuando el personaje cae a un precipicio o agua.
	*
	*	@param Collision hit Objeto con el que se genera la colisión.
	*/
	public void OnCollisionEnter(Collision hit) {

		if(hit.gameObject.tag == "Fall"){
			new EthTimer(1500, showGameOver);
		}
	}

	/**
	* 	@breaf Método para girar el personaje 180 grados en el eje Y.
	*/
	public void turnCharacter(){
		GetComponent<Transform> ().Rotate(0,180,0,Space.World);
	}

	/**
	* 	@breaf Método para reproducri la animación de idle despues de estar agachado.
	*
	*	@param object obk Objeto que genera la acción.
	*/
	public void stayDown(object stay){
		animator.Play ("idle");
		down=false;
	}

	/**
	* 	@breaf Método para definir el inicio de un ataque.
	*
	*	@param object obk Objeto que genera la acción.
	*/
	public void attackAction(object obj){
		attack = false;
	}

	/**
	* 	@breaf Método para generar un dardo desde la cerbatana.
	*
	*	@param object obk Objeto que genera la acción.
	*/
	public void createDart(object obj){
		//Util.GetChildByName (this.gameObject,"cervatana").GetComponent<Dart_Generator_Controller>().generate();
		Rigidbody dartInstance;
		dartInstance = Instantiate (dartPrefab, new Vector3(blowgunEnd.position.x + 1, blowgunEnd.position.y, blowgunEnd.position.z), blowgunEnd.rotation) as Rigidbody;
		dartInstance.AddForce (blowgunEnd.forward * 400);
	}

	/**
	* 	@breaf Método para generar un dardo cuando el personaje está agachado.
	*
	*	@param object obk Objeto que genera la acción.
	*/
	public void createDartDown(object obj){
		//Util.GetChildByName (this.gameObject,"cervatana").GetComponent<Dart_Generator_Controller>().generate();
		Rigidbody dartInstance;
		dartInstance = Instantiate (dartPrefab, new Vector3(blowgunEnd.position.x + 1, blowgunEnd.position.y - 1, blowgunEnd.position.z), blowgunEnd.rotation) as Rigidbody;
		dartInstance.AddForce (blowgunEnd.forward * 400);
	}

	/**
	* 	@breaf Método para modificar la vida del personaje.
	*
	*	@param int actual_life Nuevo valor de la vida del personaje.
	*/
	public void setLife(int actual_life){
		life = actual_life;

		if(life == 0){
			new EthTimer(1500, showGameOver);
		}
	}

	/**
	* 	@breaf Método para modificar el arma equipada
	* 
	* 	@param bool var True para equipar la lanza o false para equipar la cerbatana.
	*/
	public void setGun(bool var){
		if (var) {
			isSpear = true;
		} else {
			isSpear = false;
		}
	}

	/**
	* 	@breaf Método para mostrar la pantalla de game over.
	*
	*	@param object obk Objeto que genera la acción.
	*/
	public void showGameOver(object obj){
		gameOverPanel.SetActive (true);
	}

	/**
	* 	@breaf Método para mostrar el menú de pausa.
	*/
	public void pause(){
		isPaused = true;
		pausePanel.SetActive (true);
	}

	/**
	* 	@breaf Método para reanudar la partida.
	*/
	public void resume(){
		pausePanel.SetActive (false);
		isPaused = false;
	}

	/**
	* 	@breaf Método para reiniciar el nivel.
	*/
	public void restart(){
		Application.LoadLevel (Save.savedGame.level+1);
	}

	/**
	* 	@breaf Método para regresar al menú principal.
	*/
	public void menu(){
		SceneHandler.LoadScene ("Main_Menu");
	}

	/**
	* 	@breaf Indica que se presionó la tecla de salto.
	*/
	public void upBtnDown(){
		jumpControl = true;
	}

	/**
	* 	@breaf Indica que se solto el botón de salto.
	*/
	public void upBtnUp(){
		jumpControl = false;
	}

	/**
	* 	@breaf Indica que se presionó el botón de agachado.
	*/
	public void downBtnDown(){
		downControl = true;
	}
	
	/**
	* 	@breaf Indica que se soltó el botón de agachado.
	*/
	public void downBtnUp(){
		downControl = false;
	}

	/**
	* 	@breaf Indica que se presionó el botón de desplazamiento a la derecha.
	*/
	public void rigthBtnDown(){
		rightControl = true;
	}
	
	/**
	* 	@breaf Indica que se soltó el botón de desplazamiento a la derecha.
	*/
	public void rightBtnUp(){
		rightControl = false;
	}

	/**
	* 	@breaf Indica que se presionó el botón de desplazamiento a la izquierda.
	*/
	public void leftBtnDown(){
		leftControl = true;
	}
	
	/**
	* 	@breaf Indica que se soltó el botón de desplazamiento a la izquierda.
	*/
	public void leftBtnUp(){
		leftControl = false;
	}

	/**
	* 	@breaf Indica que se presionó el botón de ataque.
	*/
	public void attackBtnDown(){
		attackControl = true;
	}
	
	/**
	* 	@breaf Indica que se soltó el botón de ataque.
	*/
	public void atackBtnUp(){
		attackControl = false;
	}
}
