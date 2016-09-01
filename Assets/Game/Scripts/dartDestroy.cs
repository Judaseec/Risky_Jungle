using UnityEngine;
using System.Collections;

/*
*	@brief Clase que destruye todo dardo generado despues de 2 segundos.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class dartDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 2f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
