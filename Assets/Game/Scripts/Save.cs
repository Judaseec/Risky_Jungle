using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class Save {

	public static Game savedGame = new Game ();

	public static void SaveGame(){
		savedGame = Game.current;
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGame.eth");
		bf.Serialize (file, Save.savedGame);
		file.Close();
	}

	public static void LoadGame(){
		if(File.Exists(Application.persistentDataPath + "/savedGame.eth")){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGame.eth", FileMode.Open);
			Save.savedGame = (Game)bf.Deserialize(file);
			file.Close();
		}
	}
}
