using UnityEngine;
using System.Collections;

/*
*	@brief Clase que controla las interacciones o movimientos del oso.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class Bear_Controller : MonoBehaviour {

	/*
	*	@brief Variable que controla la vida actual del oso.
	*/
	public float life = 150f;

	/*
	*	@brief Variable que determina si el oso esta muerto.
	*/
	public bool dead = false;

	/*
	*	@brief Variable que muestra que hay un gameObject en el area del collider.
	*/
	public bool inside = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (GameObject.Find("trigger").GetComponent<Trigger_Controller>().inside);
		if (life <= 0) {
			dead = true;
		}

		if (inside && !dead) {
			GetComponent<Animator> ().Play("atacar");
		}
		else if (!inside && !dead){
			GetComponent<Animator> ().Play("idle");
		}

		if (dead) {
			GetComponent<Animator> ().Play("morir");
			//Util.GetChildByName(gameObject,"oso").GetComponent<Combat_Controller>().enabled = false;
			Util.GetChildByName(gameObject,"osoModel").GetComponent<MeshCollider>().isTrigger = true;
		}
	}

	/*
	*	@brief Metodo ejecutado cuando un collider(trigger) ingresa en el collider(area de ataque) del oso.
	*	
	*	@param Collider hit Collider que ingresa en el area del collider del oso.
	*/
	void OnTriggerEnter(Collider hit){
		//Si el objeto que entra al trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			inside=true;
		}
	}
	
	/*
	*	@brief Metodo ejecutado cuando un collider(trigger) sale del collider(area de ataque) del oso.
	*	
	*	@param Collider hit Collider que sale del area del collider del oso.
	*/
	void OnTriggerExit(Collider hit){
		//Si el objeto que sale del trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			inside=false;
		}
	}

	/*
	*	@brief Metodo que aplica el daño hecho al oso.
	*	
	*	@param int damage Daño que le hacen al oso por ataque.
	*/
	public void applyDamage(int damage){
		life = life - damage;
	}
}
