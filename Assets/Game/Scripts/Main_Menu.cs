using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.com.ethereal.util;
using Assets.Scripts.com.ethereal.audio;
using Assets.Scripts.com.ethereal.appsSystem;

/**
*	@class Main_Menu
*	@brief Clase que controla las interacciones y validaciones del menú principal.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class Main_Menu : MonoBehaviour {

	/**
	*	@brief Game object que representa el botón continuar.
	*/
	private GameObject continuebtn;

	/**
	*	@brief Game object que representa el panel de configuraciones.
	*/
	private GameObject configPanel;

	/**
	*	@brief Barra de progreso para configurar los efectos.
	*/
	private Scrollbar effects;

	/**
	*	@brief Barra de progreso para configurar la música.
	*/
	private Scrollbar music;

	/**
	*	@brief Game object que representa el panel de carga.
	*/
	private GameObject loaderPanel;

	/**
	*	@brief Define si el menú de configuración está activo.
	*/
	private bool isConfEnable;

	/**
	*	@brief Define si el sistema de analiticas fue cargado.
	*/
	public bool isSystemReady = false;

	/**
	*	@brief Método que se ejecuta una vez la clase es llamada e inicializa el sistema de analiticas.
	*/
	public void Awake(){
			EthAppsSystem.Init (this, setSystemReady);
	}

	/**
	*	@brief Método que iniciliza los componentes del menú y configura el idioma actual.
	*/
	public void Start(){
		//Save.LoadGame ();
		continuebtn = Util.GetChildByName (this.gameObject, "Continue");
		configPanel = Util.GetChildByName (this.gameObject, "ConfigPanel");
		loaderPanel = Util.GetChildByName (this.gameObject, "LoaderPanel");
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

	/**
	*	@brief Método que se ejecuta una vez por frame y define si se muestra el cargador y el botón continuar.
	*/
	public void Update(){
		if (isSystemReady || EthAppsSystem.isready) {
			loaderPanel.SetActive(false);
		}
		if (Save.savedGame.level == 0 && continuebtn.activeInHierarchy) {
			continuebtn.SetActive (false);
		} else if (Save.savedGame.level > 0 && !continuebtn.activeInHierarchy) {
			Eth.SetVisibleGameObject(continuebtn, false);
		}
	}

	/**
	*	@brief Método para iniciar un nuevo juego.
	*/
	public void NewGame (){

		EthAppsSystem.Log (this,"Nuevo juego", "Boton nuevo juego", "nuevo", "nuevo");
		Debug.Log ("newGame");
		//tiempo de espera
		SceneHandler.LoadScene("Tutorial");
		Save.savedGame.level++;
	}

	/**
	*	@brief Método para abrir el menú de configuraciones.
	*/
	public void Conf (){
		EthAppsSystem.LogScreen (this, "Menu configuracion");
		Debug.Log ("config");
		isConfEnable = !isConfEnable;
		configPanel.SetActive (isConfEnable);
	}

	/**
	*	@brief Método para cerrar el menu de configuracion.
	*/
	public void closeConf(){
		isConfEnable = false;
		configPanel.SetActive (isConfEnable);
	}

	/**
	*	@brief Método para configurar el lenguaje a español.
	*/
	public void Esp (){
		EthLang.LangAct = "es";
	}

	/**
	*	@brief Método para configurar el idioma a inglés.
	*/
	public void Eng (){
		EthLang.LangAct = "en";
	}

	/**
	*	@brief Método para cambiar el volumen de los efectos.
	*/
	public void EffectsVolume(){
		EthAudio.GetInstance (this).SetEffectsVolume (effects.value * 100);
	}

	/**
	*	@brief Método para configurar el volumen de la música.
	*/
	public void MusicVolume(){
		EthAudio.GetInstance (this).SetMusicVolume (music.value * 100);
	}

	/**
	*	@brief Método para definir el idioma en los botones toogle.
	*/
	public void clickLang(bool val){
		if (val) {
			EthLang.LangAct = "es";
		} else {
			EthLang.LangAct = "en";
		}
	}

	/**
	*	@brief Método para definir si el sistema de analiticas fue configurado.
	*/
	public void setSystemReady(bool obj){
		isSystemReady = true;
	}
}
