using UnityEngine;
using System.Collections;

/**
*	@class Follow_Character
*	@brief Clase que permite a la camara mantener la posicion en x del personaje.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class Follow_Character : MonoBehaviour {

	/**
	*	@brief Transform del personaje con el cual se obtiene su ubicacion en el entorno.
	*/
	public Transform character;

	/**
	*	@brief Constante de separacion para mantener al personaje a cierta distancia del 
	*	borde horizontal y no en el centro de la pantalla.
	*/
	public float separate = 3f;
	public bool back = false;

	
	/**
	*	@brief Método que se ejecuta una vez por frame y mantiene actualizada la posición de la cámara.
	*/
	public void Update () {
		transform.position = new Vector3 (character.position.x + separate, transform.position.y, transform.position.z);
	}
}
