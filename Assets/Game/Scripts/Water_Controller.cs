using UnityEngine;
using System.Collections;

public class Water_Controller : MonoBehaviour {

	public bool waterSurfaceTouch = false;

	void OnTriggerStay (Collider hit)
	{
		if(hit.gameObject.tag == "Principal")
		{
			hit.gameObject.GetComponent<Rigidbody>().useGravity = false;
			//hit.gameObject.GetComponent<Rigidbody>().mass = 0f;
			hit.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(2, GetComponent<Rigidbody>().velocity.y + 1, GetComponent<Rigidbody>().velocity.z);

		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find("Raft_Trigger").GetComponent<Trigger_Controller>().inside) {
			//GetComponent<Animator> ().Play("atacar");
			Rigidbody raft = GameObject.Find("Raft").GetComponent<Rigidbody>();
			raft.velocity = new Vector3(2, 0, 0);

			//GameObject.Find("indio").GetComponent<ControladorPersonaje>().enabled = false;
		}
	}
}
