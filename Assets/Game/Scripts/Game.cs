using UnityEngine;
using System.Collections;

/**
*	@class Game
*	@brief Clase que representa un juego y sus variables a serializar.
*
*	@author Dival Mauricio Hoyos Castro <dmhoyosc@gmail.com>
*	@author Julian David Serna Echeverri<jdsernae@gmail.com>
*/
[System.Serializable]
public class Game {

	/**
	*	@brief Patrón singleton para mantener siempre la misma instancia.
	*/
	public static Game current;

	/**
	*	@brief Nivel actual del jugador.
	*/
	public int level;

	/**
	*	@brief Constructor de la clase que inicia el nivel en 0.
	*/
	public Game(){
		level = 0;
	}
}
