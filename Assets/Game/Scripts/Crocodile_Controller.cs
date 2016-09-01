using UnityEngine;
using System.Collections;
using Assets.Scripts.com.ethereal.util;

/*
*	@brief Clase que controla las interacciones o movimientos del cocodrilo.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class Crocodile_Controller : MonoBehaviour {

	/*
	*	@brief Variable que muestra si el cocodrilo esta dormido.
	*/
	public bool sleep = false;

	/*
	*	@brief Variable que muestra la vida del cocodrilo.
	*/
	private float life = 150f;

	/*
	*	@brief Variable que muestra si el cocodrilo esta muerto (dormido).
	*/
	public bool dead = false;

	/*
	*	@brief Variable que muestra si el cocodrilo esta dormido.
	*/
	public bool inside = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (inside && !dead) {
			GetComponent<Animator> ().Play("ataque");
		}
		else if (!inside && !dead){
			GetComponent<Animator> ().Play("idle");
		}

		if (dead) {
			GetComponent<Animator> ().Play("muerte");

		}
		
		
	}
	
	/*
	*	@brief Metodo ejecutado cuando un collider(trigger) ingresa en el collider(area de ataque) del cocodrilo.
	*	
	*	@param Collider hit Collider que ingresa en el area del collider del cocodrilo.
	*/
	void OnTriggerEnter(Collider hit){
		//Si el objeto que entra al trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			inside=true;
		}
	}
	
	/*
	*	@brief Metodo ejecutado cuando un collider(trigger) sale del collider(area de ataque) del cocodrilo.
	*	
	*	@param Collider hit Collider que sale del area del collider del cocodrilo.
	*/
	void OnTriggerExit(Collider hit){
		//Si el objeto que sale del trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			inside=false;
		}
	}

	/*
	*	@brief Metodo ejecutado 5 segundos despues de que se determina que el cocodrilo ha muerto.
	*	
	*	@param Collider hit Collider que sale del area del collider del cocodrilo.
	*
	*	Una vez se dispara al cocodrilo, el personaje tiene 5 segundos para saltar encima y pasar el obstaculo antes
	*	de que reviva y lo pueda matar.
	*/
	public void reviveAction(object stay){
		GetComponent<Animator> ().Play ("idle");
		dead=false;
		Util.GetChildByName(gameObject,"cocodriloModel").GetComponent<BoxCollider>().isTrigger = true;
	}

	/*
	*	@brief Metodo que aplica el daño hecho al cocodrilo.
	*	
	*	@param int damage Daño que le hacen al cocodrilo por ataque.
	*/
	public void applyDamage(){
		dead = true;
		Util.GetChildByName(gameObject,"cocodriloModel").GetComponent<BoxCollider>().isTrigger = false;
		new EthTimer (5000, reviveAction);
	}

	/*
	*	@brief Metodo para obtener si el cocodrilo esta muerto.
	*/
	public bool getDead(){
		return dead;
	}
}
