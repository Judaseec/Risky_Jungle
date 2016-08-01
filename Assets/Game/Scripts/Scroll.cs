using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour {

	/*
	 * @brief Variable de velocidad en la que se desplaza el fondo_3 de la pantalla
	 */
	public float depth = 0f;
	public Rigidbody Character_rigibody;
	private float time = 0;
	private bool moving = false;
	//private Renderer renderer = GetComponent<Renderer>();

	// Use this for initialization
	void Start () {
		NotificationCenter.DefaultCenter ().AddObserver (this, "Character_is_running");
	}

	void Character_is_running(){
		moving = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Character_rigibody.velocity.x < 0){
			time = Time.time;
		}
		if (moving) {
			GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 ((Time.time) * Character_rigibody.velocity.x * depth, 0);
		}
	}
}
