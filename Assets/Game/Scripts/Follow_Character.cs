using UnityEngine;
using System.Collections;

public class Follow_Character : MonoBehaviour {

	public Transform character;
	public float separate = 3f;
	public bool back = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (character.position.x + separate, transform.position.y, transform.position.z);
	}
}
