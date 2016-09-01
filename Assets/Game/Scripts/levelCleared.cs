using UnityEngine;
using System.Collections;
using Assets.Scripts.com.ethereal.appsSystem;

public class levelCleared : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider hit){
		//Si el objeto que entra al trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			Save.savedGame.level += 1;
			//Save.SaveGame();
			//timer
			EthAppsSystem.ChangeStateVariable(this,"nivel",(Save.savedGame.level+1)+"");

			Application.LoadLevel (Save.savedGame.level+1);
		}
	}
}
