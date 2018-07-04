using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
	public Scene mainLevel;
	public Text versionText;
	public GameObject optionsPanel, mainPanel;

	void Start() {
		versionText.text = "SPEED CUBE ALPHA " + Application.version;
	}

	public void StartGame() {
		SceneManager.LoadScene(1);
	}

	public void SettingsMenu() {
		
	}
}
