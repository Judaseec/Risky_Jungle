using UnityEngine;
using System.Collections;

public class Bear_Controller : MonoBehaviour {



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (GameObject.Find("trigger").GetComponent<Trigger_Controller>().inside);
		if (GameObject.Find("trigger").GetComponent<Trigger_Controller>().inside) {
			GetComponent<Animator> ().Play("atacar");
		}
	}
}
