using UnityEngine;
using System.Collections;
using Assets.Scripts.com.ethereal.audio;

/**
*	@class Tiger_Controller
*	@brief Clase que controla las interacciones o movimientos del tigre.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class Tiger_Controller : MonoBehaviour {

	/**
	*	@brief Variable que controla la vida actual del tigre.
	*/
	private float life = 150f;

	/**
	*	@brief Variable que determina si el tigre esta muerto.
	*/
	public bool dead = false;

	/**
	*	@brief Variable que indica si hay un gameObject en el area del collider.
	*/
	public bool inside = false;

	/**
	*	@brief Variable para guardar el efecto ciclico y poder eliminarlo despues.
	*/
	private Object effect;
	
	/**
	*	@brief Método que se ejecuta una vez por frame, el cual controla las animaciones y la muerte del tigre.
	*/
	public void Update () {

		if (life <= 0) {
			dead = true;
		}
		//Debug.Log (GameObject.Find("trigger").GetComponent<Trigger_Controller>().inside);
		if (inside && !dead) {
			GetComponent<Animator> ().Play("atacar");
		}
		else if (!inside && !dead){
			GetComponent<Animator> ().Play("idle");
		}

		if (dead) {
			GetComponent<Animator> ().Play("morir");
			Util.GetChildByName(gameObject,"tigreModel").GetComponent<MeshCollider>().isTrigger = true;
		}
	}
	
	/**
	*	@brief Metodo ejecutado cuando un collider(trigger) ingresa en el collider(area de ataque) del tigre.
	*	
	*	@param Collider hit Collider que ingresa en el area del collider del tigre.
	*/
	public void OnTriggerEnter(Collider hit){
		//Si el objeto que entra al trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			inside=true;
			effect = EthAudio.GetInstance(null).PlayEffectRepeated("Sounds/TigerClip");
		}
	}
	
	/**
	*	@brief Metodo ejecutado cuando un collider(trigger) sale del collider(area de ataque) del tigre.
	*	
	*	@param Collider hit Collider que sale del area del collider del tigre.
	*/
	public void OnTriggerExit(Collider hit){
		//Si el objeto que sale del trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			inside=false;
			EthAudio.GetInstance(null).RemoveEffect(effect);
		}
	}

	/**
	*	@brief Metodo para hacer que el tigre muera.
	*/
	public void applyDamage(){
		dead = true;
	}

	/**
	*	@brief Metodo que aplica el daño hecho al tigre.
	*	
	*	@param int damage Daño que le hacen al tigre por ataque.
	*/
	public void applyDamage(int damage){
		life = life - damage;
	}
}
