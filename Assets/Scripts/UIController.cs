using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public Text scoreText, debugText, versionText;

	public GameObject player;

	void Start() {
		versionText.text = "SPEED CUBE ALPHA " + Application.version;
	}

	void Update () {
		versionText.text = "SPEED CUBE ALPHA " + Application.version;
		scoreText.text = "Score: " + player.GetComponent<Player>().score;
		DebugTextFormatter();
	}

	void DebugTextFormatter() {
		debugText.text = "Max speed: " + player.GetComponent<Player>().attributes.speed +
			"\nCurrent Velcoity: " + player.GetComponent<Player>().GetVelocity() + "\nFPS: " + 1 / Time.deltaTime +
			"\nOn Wall?: " + player.GetComponent<Player>().IsOnWall() + "\nHigh Score: " + LocalData.highScore; 
	}
}
