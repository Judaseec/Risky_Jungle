using UnityEngine;
using System.Collections;

/**
*	@class slide_Controller
*	@brief Clase encargada de administrar cuando el personaje resbala.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class slide_Controller : MonoBehaviour {

	/**
	 * @brief Variable de prueba.
	 */
	public bool debug = false;

	/**
	 * @brief Poición del personaje en el eje Y.
	 */
	public float hito = 0f;

	/**
	 * @brief Posición inicial de la cámara en el eje Y.
	 */
	public float yCamInit = 0f;

	/**
	 * @brief Método para detectar cuando el personaje entra en zona resbaladiza.
	 */
	public void OnTriggerEnter(Collider hit){
		//Si el objeto que entra al trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {

			GameObject cam = GameObject.Find ("Camera");
			yCamInit = cam.transform.position.y;
			//hito = hit.gameObject.transform.position.y;
			//cam.transform.position = new Vector3 (cam.transform.position.x, hit.gameObject.transform.position.y, cam.transform.position.z);
			//cam.transform.position.y = hit.gameObject.transform.position.y;
			//cam.GetComponent<Follow_Character>().enabled = false;

		}
	}

	/**
	 * @brief Método para detectar cuando el personaje sale de la zona resbaladiza.
	 */
	public void OnTriggerExit(Collider hit){
		//Si el objeto que sale del trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {

			GameObject cam = GameObject.Find ("Camera");
			cam.transform.position = new Vector3 (cam.transform.position.x, yCamInit, cam.transform.position.z);
			//var yCamInit = cam.transform.position.y;

		}
	}

	/**
	 * @brief Método para detectar si el personaje permanece en la zona resbaladiza.
	 */
	public void OnTriggerStay(Collider hit) {
		if (hit.gameObject.tag == "Principal") {

			GameObject cam = GameObject.Find ("Camera");
			//yCamInit = cam.transform.position.y;
			hito = hit.gameObject.transform.position.y;
			cam.transform.position = new Vector3 (cam.transform.position.x, hit.gameObject.transform.position.y, cam.transform.position.z);       
		}
	}
}
