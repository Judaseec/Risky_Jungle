using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour {

	/*
	 * @brief Variable de velocidad en la que se desplaza el fondo_3 de la pantalla
	 */
	public float velocity = 0f;
	//private Renderer renderer = GetComponent<Renderer>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2 (Time.time * velocity, 0);
	}
}
