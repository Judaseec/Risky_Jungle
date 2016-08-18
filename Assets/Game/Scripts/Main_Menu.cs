using UnityEngine;
using System.Collections;
using Assets.Scripts.com.ethereal.util;

public class Main_Menu : MonoBehaviour {

	private GameObject continuebtn; 
	
	public void NewGame (){
		SceneHandler.LoadScene("Tutorial");
	}

	void Start(){
		continuebtn = Util.GetChildByName (this.gameObject, "Continue");
		string aux = Eth.EncodeBase64 ("asdasdasd");
		Debug.Log (aux);
	}

	void Update(){
		if (Save.savedGame.level == 0 && continuebtn.activeInHierarchy) {
			continuebtn.SetActive (false);
		} else if (Save.savedGame.level > 0 && !continuebtn.activeInHierarchy) {
			continuebtn.SetActive (true);
		}
	}
}
