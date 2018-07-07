using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
	public Scene mainLevel;
	public Text versionText, highScoreText;
	public GameObject optionsPanel, mainPanel;

    private bool settings = false;

	void Start() {
		versionText.text = "SPEED CUBE ALPHA " + Application.version;
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
}
