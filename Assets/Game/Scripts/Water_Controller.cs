using UnityEngine;
using System.Collections;

/**
*	@class Water_Controller
*	@brief Clase que controla las interacciones o movimientos del oso.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class Water_Controller : MonoBehaviour {

	/**
	*	@brief Variable que indica si el personaje esta haciendo contacto con el agua.
	*/
	public bool waterSurfaceTouch = false;

	/**
	*	@brief Método que define si el personaje está en el agua.
	*
	*	@param Collider hit Collider del objeto que permanece en el agua.
	*/
	public void OnTriggerStay (Collider hit)
	{
		if(hit.gameObject.tag == "Principal")
		{
			hit.gameObject.GetComponent<Rigidbody>().useGravity = false;
			//hit.gameObject.GetComponent<Rigidbody>().mass = 0f;
			hit.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(2, GetComponent<Rigidbody>().velocity.y + 1, GetComponent<Rigidbody>().velocity.z);

		}

	}

	/**
	*	@brief Método que le agrega una fuerza a la balsa.
	*/
	public void Update () {
		if (GameObject.Find ("Raft_Trigger") != null) {
			if (GameObject.Find ("Raft_Trigger").GetComponent<Trigger_Controller> ().inside) {
				//GetComponent<Animator> ().Play("atacar");
				Rigidbody raft = GameObject.Find ("Raft").GetComponent<Rigidbody> ();
				raft.velocity = new Vector3 (2, 0, 0);

				//GameObject.Find("indio").GetComponent<ControladorPersonaje>().enabled = false;
			}
		}
	}
}
