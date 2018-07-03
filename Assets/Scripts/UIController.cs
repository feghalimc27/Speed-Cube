using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public Text scoreText, debugText;

	public GameObject player;

	void Update () {
		scoreText.text = "Score: " + player.GetComponent<Player>().score;
		debugText.text = "SPEED CUBE ALPHA " + Application.version + "\nMax speed: " + player.GetComponent<Player>().attributes.speed +
			"\nCurrent Velcoity: " + player.GetComponent<Player>().GetVelocity(); 
	}
}
