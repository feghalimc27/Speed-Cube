using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {

    public static bool paused = false;

    public GameObject pausePanel, debugPanel, gamePanel, gameOverPanel;
    public Text zoomText;
	public static float zoomLevel = 6;

    public static bool debug = false;

    void OnEnable() {
        Time.timeScale = 1f;
    }

	private void Start() {
		debugPanel.SetActive(debug);
		FindObjectOfType<Camera>().orthographicSize = zoomLevel;
		OnZoomChange();
	}

	// Update is called once per frame
	void Update () {
        OnEscapeKeyPress();
	}

    public void OnZoomChange() {
		zoomLevel = FindObjectOfType<Camera>().orthographicSize;
		zoomText.text = "ZOOM LEVEL: " + zoomLevel;
    }

    void OnEscapeKeyPress() {
        if (Input.GetButtonDown("Cancel")) {
            if (paused) {
                UnPause();
            }
            else {
                Pause();
            }
        }
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

	public void QuitToMenu() {
		SceneManager.LoadScene(0);
	}

	public void GameOver() {
		gamePanel.SetActive(false);
		gameOverPanel.SetActive(true);

		Text[] gameOverObjects = gameOverPanel.GetComponentsInChildren<Text>();

		foreach (var text in gameOverObjects) {
			if (text.text.Contains("SCORE: ")) {
				text.text = "SCORE: " + FindObjectOfType<Player>().score;
			}
		}
	}

    public void ToggleDebug() {
        if (debug) {
            debug = false;
            debugPanel.SetActive(false);
        }
        else {
            debug = true;
            debugPanel.SetActive(true);
        }
    }

    public void Pause() {
        pausePanel.SetActive(true);
        gamePanel.SetActive(false);
        Time.timeScale = 0f;
        paused = true;
    }

    public void UnPause() {
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        Time.timeScale = 1f;
        paused = false;
    }
}
