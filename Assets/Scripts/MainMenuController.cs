using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
	public Scene mainLevel;
	public Text versionText, highScoreText, gravityToggleText;
	public GameObject optionsPanel, mainPanel, debugButton;

	private bool settings = false, showGrav = false;

	void Start() {
		versionText.text = "SPEED CUBE ALPHA " + Application.version;
		if (Player.gravityMultiplier) {
			gravityToggleText.text = "GRAV CHANGE: ON";
		}
		else {
			gravityToggleText.text = "GRAV CHANGE: OFF";
		}
	}

	void Update() {
		DebugToggle();
	}

	public void StartGame() {
		SceneManager.LoadScene(1);
	}

	public void SettingsMenu() {
		if (!settings) {
            mainPanel.SetActive(false);
            optionsPanel.SetActive(true);
            highScoreText.text = "HIGH SCORE: " + LocalData.highScore;
            settings = true;
        }
        else {
            mainPanel.SetActive(true);
            optionsPanel.SetActive(false);
            settings = false;
        }
	}

	public void DebugGravityToggle() {
		Player.gravityMultiplier = !Player.gravityMultiplier;
		if (Player.gravityMultiplier) {
			gravityToggleText.text = "GRAV CHANGE: ON";
		}
		else {
			gravityToggleText.text = "GRAV CHANGE: OFF";
		}
	}

	private void DebugToggle() {
		Touch[] touches = Input.touches;

		if (touches.Length == 2 && settings && !showGrav) {
			debugButton.SetActive(!debugButton.activeSelf);
			showGrav = true;
		}
		else if (touches.Length != 2) {
			showGrav = false;
		}
	}
}
