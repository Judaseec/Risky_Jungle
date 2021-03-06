﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.com.ethereal.appsSystem;

/**
*	@class levelCleared
*	@brief Clase encargada de detectar si el personaje ha pasado el nivel actual y esta listo
*	para el siguiente.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public class levelCleared : MonoBehaviour {
	/**
	*	@brief Metodo ejecutado cuando el collider del personaje ingresa en el area de designada
	*	para determinar que el usuario a completado el nivel actual y podria pasar al siguiente.
	*	
	*	@param Collider hit Collider que ingresa en el area del collider del cocodrilo.
	*/
	public void OnTriggerEnter(Collider hit){
		//Si el objeto que entra al trigger tiene el tag principal...
		if (hit.gameObject.tag == "Principal") {
			if(Save.savedGame.level < 4){
				Save.savedGame.level++;
			} else {
				Save.savedGame.level = 0;
			}
				//Save.SaveGame();
				//timer
			Debug.Log(Save.savedGame.level);
			EthAppsSystem.ChangeStateVariable(this,"nivel",Save.savedGame.level+"");
			EthAppsSystem.Log (this,"Nivel completado", "nuevo nivel", "nuevo", Save.savedGame.level+"");
			Application.LoadLevel (Save.savedGame.level);
		}
	}
}
