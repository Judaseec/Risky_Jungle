using UnityEngine;
using System.Collections;
using Assets.Scripts.com.ethereal.util;

public class Main_Menu : MonoBehaviour {

	private GameObject continuebtn;
	private GameObject configPanel;

	private bool isConfEnable;

	void Start(){
		continuebtn = Util.GetChildByName (this.gameObject, "Continue");
		configPanel = Util.GetChildByName (this.gameObject, "ConfigPanel");
		isConfEnable = false;
		configPanel.SetActive (isConfEnable);

		//Activar los idiomas (EthLang.ActiveLangs)

		string aux = Eth.EncodeBase64 ("asdasdasd");
		Debug.Log (aux);
	}

	void Update(){

		if (Save.savedGame.level == 0 && continuebtn.activeInHierarchy) {
			continuebtn.SetActive (false);
		} else if (Save.savedGame.level > 0 && !continuebtn.activeInHierarchy) {
			Eth.SetVisibleGameObject(continuebtn, false);
		}
	}

	public void NewGame (){
		SceneHandler.LoadScene("Tutorial", this.gameObject);
	}

	public void Conf (){
		isConfEnable = !isConfEnable;
		configPanel.SetActive (isConfEnable);
	}

	public void Esp (){
		EthLang.LangAct = "es";
	}

	public void Eng (){
		EthLang.LangAct = "en";
	}
}
