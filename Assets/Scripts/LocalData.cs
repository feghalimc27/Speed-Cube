using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LocalData : MonoBehaviour {
	public static int highScore = 0;
	public static int coinCount = 0;

	[System.Serializable]
	public struct ScoreData {
		public int highScore;
		public int coinCount;
	}

	void Start() {
		if (SceneManager.GetActiveScene().buildIndex == 0) {
			LoadData();
		}
	}

	public void LoadData() {
		if (File.Exists(Application.persistentDataPath + "/saveData")) {
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream loadFile = File.Open(Application.persistentDataPath + "/saveData", FileMode.Open);
			ScoreData data = (ScoreData)binaryFormatter.Deserialize(loadFile);
			loadFile.Close();

			highScore = data.highScore;
			coinCount = data.coinCount;

			Debug.Log("File Loaded");
		}
		else {
			Debug.Log("File not found");
		}
	}

	public void SaveData() {
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream saveFile = File.Create(Application.persistentDataPath + "/saveData");
		ScoreData data = new ScoreData();

		data.highScore = highScore;
		data.coinCount = coinCount;

		binaryFormatter.Serialize(saveFile, data);
		saveFile.Close();

		Debug.Log("File saved");
	}

}
