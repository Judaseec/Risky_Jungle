using UnityEngine;
using System.Collections;

/*
*	@class Combat_Controller
*	@brief Clase que controla las acciones del combate entre personajes.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class Combat_Controller : MonoBehaviour {

	/**
	*	@brief Variable que muestra si el personaje entra en el area de ataque de algun animal.
	*/
	public bool enter = false;

	/**
	*	@brief Variable que muestra si existe una collision entre personaje y animales.
	*/
	public bool colision = false;

	/**
	*	@brief Collider guardado del personaje que choca con el collider del atacante.
	*/
	private Collision hitobj;
	
	/**
	* 	@breaf Método que se ejecuta una vez por frame.
	*/
	public void Update () {
		if (colision) {
			//hitobj.gameObject.GetComponent<Animator>().Stop();
		}
	}

	/**
	*	@brief Metodo ejecutado cuando un collider choca con el collider que posee este script.
	*	
	*	@param Collider hit Collider del enemigo que choca con el del objeto actual.
	*/
	public void OnCollisionEnter(Collision hit) {
		Debug.Log (hit);
		if (hit.gameObject.tag == "Principal") {
			hitobj = hit;
			if(this.gameObject.transform.parent.gameObject.tag == "Crocodile"){
				if(!this.gameObject.transform.parent.gameObject.GetComponent<Crocodile_Controller>().getDead()){
					hit.gameObject.GetComponent<Character_Controller>().setLife(0);
				}

			}else{
				hit.gameObject.GetComponent<Character_Controller>().setLife(0);
			}
			colision=true;
		}

		if (hit.gameObject.tag == "Bear") {
			hit.gameObject.GetComponent<Bear_Controller>().applyDamage(50);
			colision=true;
		}

		if (hit.gameObject.tag == "Dart") {
			if(this.gameObject.transform.parent.gameObject.tag == "Bear"){
				this.gameObject.transform.parent.gameObject.GetComponent<Bear_Controller>().applyDamage(50);
				colision=true;
			}
			if(this.gameObject.transform.parent.gameObject.tag == "Snake"){
				this.gameObject.transform.parent.gameObject.GetComponent<Snake_Controller>().applyDamage();
				colision=true;
			}
			if(this.gameObject.transform.parent.gameObject.tag == "Crocodile"){
				this.gameObject.transform.parent.gameObject.GetComponent<Crocodile_Controller>().applyDamage();
				colision=true;
			}
			if(this.gameObject.transform.parent.gameObject.tag == "Tiger"){
				this.gameObject.transform.parent.gameObject.GetComponent<Tiger_Controller>().applyDamage(50);
				colision=true;
			}
		}


	}

}
