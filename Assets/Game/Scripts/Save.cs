using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/**
*	@class Save
*	@brief Clase que representa un juego y sus variables a serializar.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
public static class Save {

	/**
	*	@brief Instancia del uego guardado.
	*/
	public static Game savedGame = new Game ();

	/**
	*	@brief Método para guardar en el dispositivo los datos del juego.
	*/
	public static void SaveGame(){
		savedGame = Game.current;
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGame.eth");
		bf.Serialize (file, Save.savedGame);
		file.Close();
	}

	/**
	*	@brief Método para cargar del dispositivo los datos d ejuego.
	*/
	public static void LoadGame(){
		if(File.Exists(Application.persistentDataPath + "/savedGame.eth")){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGame.eth", FileMode.Open);
			Save.savedGame = (Game)bf.Deserialize(file);
			file.Close();
		}
	}
}
