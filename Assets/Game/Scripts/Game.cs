using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game {

	public static Game current;
	public int level;

	public Game(){
		level = 0;
	}
}
