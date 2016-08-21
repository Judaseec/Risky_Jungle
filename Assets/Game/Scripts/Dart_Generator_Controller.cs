using UnityEngine;
using System.Collections;

public class Dart_Generator_Controller : MonoBehaviour {

	public GameObject dart;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void generate(){
		//GameObject new_dart = Instantiate(dart,GetComponent<Transform>().position, Quaternion.identity) as GameObject;
		//new_dart.GetComponent<Rigidbody>().velocity = new Vector3(3, dart.GetComponent<Rigidbody>().velocity.y, 0);
	}

}
