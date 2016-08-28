using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour {

	/*
	 * @brief Variable de velocidad en la que se desplaza el fondo_3 de la pantalla
	 */
	public float depth = 0f;
	public Rigidbody Character_rigibody;
	public float start_time = 0f;
	public float end_time = 0f;
	//private float time = 0f;
	public bool moving = false;
	public float xoffset = 0f;

	public float time = 0f;
	//private Renderer renderer = GetComponent<Renderer>();

	// Use this for initialization
	void Start () {
		NotificationCenter.DefaultCenter ().AddObserver (this, "Character_is_running");
		NotificationCenter.DefaultCenter ().AddObserver (this, "Character_is_not_running");
	}

	void Character_is_running(){
		moving = true;
		start_time = Time.time;
		//GetComponent<Renderer> ().material.mainTextureOffset.x = xoffset;
	}

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
