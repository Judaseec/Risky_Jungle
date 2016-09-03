using UnityEngine;
using System.Collections;

/**
*	@class Advice_Controller
*	@brief Clase que controla cuando mostrar los consejos del tutorial.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class Advice_Controller : MonoBehaviour {

	/**
	* @brief Indica si el consejo actual ya fue mostrado
	*/
	private bool shown = false;

	/**
	* @brief Indica cual consejo debe mostrar el trigger.
	*/
	public int numAdvice;

	/**
	* @brief Representa el panel de consejos.
	*/
	private GameObject panel;

	/**
	* @brief Metodo que inicializa el panel donde se mostrara el consejo.
	*/
	public void Start(){
		panel = GameObject.Find("Advice");
		//panel.SetActive (false);
	}

	/**
	*	@brief Metodo que define las acciones a realizar cuando el personaje entra en zona de un consejo.
	*
	*	@brief Collider hit Collider que entra en el trigger.
	*/
	public void OnTriggerEnter(Collider hit){
		//Si el objeto que entra al trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal" && !shown) {
			GameObject.Find("indio").GetComponent<Character_Controller>().run = false;
			GameObject.Find("indio").GetComponent<Character_Controller>().rightControl = false;
			panel.SetActive (true);
			GameObject.Find("AdviceText").GetComponent<EthText>().textLanguage = "advice"+numAdvice;
			shown = true;
			GameObject.Find("WeaponPanel").SetActive(false);
			GameObject.Find("AttackPanel").SetActive(false);
			GameObject.Find("ControlPanel").SetActive(false);
		}
	}
}
