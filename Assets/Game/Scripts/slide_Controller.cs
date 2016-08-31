using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

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
			var yCamInit = cam.transform.position.y;
			//cam.transform.position.y = hit.gameObject.transform.position.y;
			//cam.GetComponent<Follow_Character>().enabled = false;

		}
	}

	void OnTriggerExit(Collider hit){
		//Si el objeto que sale del trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			GameObject cam = GameObject.Find ("Camera");
			//var yCamInit = cam.transform.position.y;
			//cam.transform.position.y = hit.gameObject.transform.position.y;
		}
	}
}
