using UnityEngine;
using System.Collections;

/**
*	@class Trigger_Controller
*	@brief Clase que controla cuando el personaje principal entra a un trigger.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class Trigger_Controller : MonoBehaviour {

	/**
	*	@brief Variable que define si el personaje está dentro del trigger.
	*/
	public bool inside = false;

	/**
	*	@brief Método que define cuando el personaje entra al trigger.
	*
	*	@param Collider hit Collider que entra al trigger.
	*/
	public void OnTriggerEnter(Collider hit){
		//Si el objeto que entra al trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			inside=true;
		}
	}

	/**
	*	@brief Método que define cuando el personaje sale al trigger.
	*
	*	@param Collider hit Collider que sale del trigger.
	*/
	public void OnTriggerExit(Collider hit){
		//Si el objeto que sale del trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			inside=false;
		}
	}
}
