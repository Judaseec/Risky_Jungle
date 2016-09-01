using UnityEngine;
using System.Collections;

/**
*	@class Dart_Generator_Controller
*	@brief Clase que genera los dardos que dispara el indio.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class Dart_Generator_Controller : MonoBehaviour {

	/**
	*	@brief GameObject que es disparado desde la cervatana e impactaria en un enemigo.
	*/
	public GameObject dart;

	/**
	*	@brief Metodo que genera un nuevo dardo y le aplica el movimiento.
	*/
	public void generate(){
		//GameObject new_dart = Instantiate(dart,GetComponent<Transform>().position, Quaternion.identity) as GameObject;
		//new_dart.GetComponent<Rigidbody>().velocity = new Vector3(3, dart.GetComponent<Rigidbody>().velocity.y, 0);
	}

}
