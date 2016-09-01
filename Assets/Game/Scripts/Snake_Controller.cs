using UnityEngine;
using System.Collections;

/**
*	@class Snake_Controller
*	@brief Clase que controla las interacciones o movimientos de la serpiente.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class Snake_Controller : MonoBehaviour {

	/**
	*	@brief Variable que determina si la serpiente está muerta.
	*/
	public bool dead = false;

	/**
	*	@brief Variable que indica si hay un gameObject en el area del collider.
	*/
	public bool inside = false;
	
	/**
	*	@brief Método que se ejecuta una vez por frame, el cual controla las animaciones y la muerte de la serpiente.
	*/
	public void Update () {
		if (inside && !dead) {
			GetComponent<Animator> ().Play("ataque");
		}
		else if (!inside && !dead){
			GetComponent<Animator> ().Play("idle");
		}
		else if (dead) {
			GetComponent<Animator> ().Play("morir");
			Util.GetChildByName(gameObject,"serpienteModel").GetComponent<MeshCollider>().isTrigger = true;
		}


	}

	/**
	*	@brief Metodo ejecutado cuando un collider(trigger) ingresa en el collider(area de ataque) de la serpiente.
	*	
	*	@param Collider hit Collider que ingresa en el area del collider de la serpiente.
	*/
	public void OnTriggerEnter(Collider hit){
		//Si el objeto que entra al trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			inside=true;
		}
	}
	
	/**
	*	@brief Metodo ejecutado cuando un collider(trigger) sale del collider(area de ataque) de la serpiente.
	*	
	*	@param Collider hit Collider que sale del area del collider de la serpiente.
	*/
	public void OnTriggerExit(Collider hit){
		//Si el objeto que sale del trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			inside=false;
		}
	}

	/**
	*	@brief Metodo que aplica el daño hecho a la serpiente.
	*	
	*	@param int damage Daño que le hacen a la serpiente por ataque.
	*/
	public void applyDamage(){
		dead = true;
	}
}
