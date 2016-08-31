using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.com.ethereal.util;
using Assets.Scripts.com.ethereal.audio;

public class Main_Menu : MonoBehaviour {

	private GameObject continuebtn;
	private GameObject configPanel;
	private Scrollbar effects;
	private Scrollbar music;
	
	private bool isConfEnable;
	
	void Start(){
		//Save.LoadGame ();
		continuebtn = Util.GetChildByName (this.gameObject, "Continue");
		configPanel = Util.GetChildByName (this.gameObject, "ConfigPanel");
		effects = Util.GetChildByName (Util.GetChildByName(this.gameObject, "ConfigPanel"), "EffectsScroll").GetComponent<Scrollbar>();
		music = Util.GetChildByName (Util.GetChildByName(this.gameObject, "ConfigPanel"), "MusicScroll").GetComponent<Scrollbar>();
		isConfEnable = false;
		configPanel.SetActive (isConfEnable);
		if (EthLang.LangAct == null || EthLang.LangAct.Equals ("")) {
			EthLang.LangAct = Util.GetSystemLanguage ();
		}
		EthLang.ActiveLangs("config/dict",EthLang.LangAct);

		EthAudio.GetInstance (this).PlayMusic ("Sounds/MenuClip");

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
		SceneHandler.LoadScene("Tutorial");
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

	public void EffectsVolume(){
		EthAudio.GetInstance (this).SetEffectsVolume (effects.value * 100);
	}

	public void MusicVolume(){
		EthAudio.GetInstance (this).SetMusicVolume (music.value * 100);
	}

	public void clickLang(bool val){
		if (val) {
			EthLang.LangAct = "es";
		} else {
			EthLang.LangAct = "en";
		}
	}
}
