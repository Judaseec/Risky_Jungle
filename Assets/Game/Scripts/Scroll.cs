using UnityEngine;
using System.Collections;


/*
*	@brief Clase encargada de mantener modificando el offset en x de las imagenes de fondo para 
*	el efecto de movimiento cuando el personaje se mueve.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class Scroll : MonoBehaviour {

	/*
	 * @brief Variable de velocidad en la que se desplaza el fondo de la pantalla.
	 */
	public float depth = 0f;

	/*
	 * @brief Rigidbody del personaje para determinar si se esta moviendo.
	 */
	public Rigidbody Character_rigibody;

	/*
	 * @brief Tiempo en el que inicia a moverse.
	 */
	public float start_time = 0f;

	/*
	 * @brief Tiempo en el que termina de moverse.
	 */
	public float end_time = 0f;
	//private float time = 0f;

	/*
	 * @brief Variable que determina si el usuario se esta moviendo.
	 */
	public bool moving = false;

	/*
	 * @brief xoffset de la imagen.
	 */
	public float xoffset = 0f;

	/*
	 * @brief tiempo que guardara el tiempo actual.
	 */
	public float time = 0f;
	//private Renderer renderer = GetComponent<Renderer>();

	// Use this for initialization
	void Start () {
		NotificationCenter.DefaultCenter ().AddObserver (this, "Character_is_running");
		NotificationCenter.DefaultCenter ().AddObserver (this, "Character_is_not_running");
	}

	/*
	 * @brief Metodo que notifica que el personaje esta corriendo.
	 */
	void Character_is_running(){
		moving = true;
		start_time = Time.time;
		//GetComponent<Renderer> ().material.mainTextureOffset.x = xoffset;
	}

	/*
	 * @brief Metodo que notifica que el personaje no esta corriendo.
	 */
	void Character_is_not_running(){
		moving = false;
		end_time = Time.time;
		xoffset = GetComponent<Renderer> ().material.mainTextureOffset.x;
	}
	
	// Update is called once per frame
	void Update () {
		//if (Character_rigibody.velocity.x < 0){
		//	time = Time.time;
		//}
		time = Time.time;
		if (moving) {
			xoffset += depth;
			//GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 ((Time.time - end_time)  * depth, 0);
			GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 (xoffset, 0);
		}
	}
}
