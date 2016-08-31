using UnityEngine;
using System.Collections;

public class slide_Controller : MonoBehaviour {

	public bool debug = false;
	public float hito = 0f;
	public float yCamInit = 0f;

	public 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider hit){
		//Si el objeto que entra al trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {

			GameObject cam = GameObject.Find ("Camera");
			yCamInit = cam.transform.position.y;
			//hito = hit.gameObject.transform.position.y;
			//cam.transform.position = new Vector3 (cam.transform.position.x, hit.gameObject.transform.position.y, cam.transform.position.z);
			//cam.transform.position.y = hit.gameObject.transform.position.y;
			//cam.GetComponent<Follow_Character>().enabled = false;

		}
	}

	void OnTriggerExit(Collider hit){
		//Si el objeto que sale del trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {

			GameObject cam = GameObject.Find ("Camera");
			cam.transform.position = new Vector3 (cam.transform.position.x, yCamInit, cam.transform.position.z);
			//var yCamInit = cam.transform.position.y;

		}
	}

	void OnTriggerStay(Collider hit) {
		if (hit.gameObject.tag == "Principal") {

			GameObject cam = GameObject.Find ("Camera");
			//yCamInit = cam.transform.position.y;
			hito = hit.gameObject.transform.position.y;
			cam.transform.position = new Vector3 (cam.transform.position.x, hit.gameObject.transform.position.y, cam.transform.position.z);       
		}
	}
}
