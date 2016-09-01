using UnityEngine;
using System.Collections;

/**
*	@class dartDestroy
*	@brief Clase que destruye todo dardo generado despues de 2 segundos.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class dartDestroy : MonoBehaviour {

	/**
	*	@brief Método que hace que despues de dos segundos de generado el dardo este se destruye.
	*/
	public void Start () {
		Destroy (gameObject, 2f);
	}
}
