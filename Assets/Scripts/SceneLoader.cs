﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
	public void LoadStartScene() {
		// Reset game state
		GameSession gameStatus = FindObjectOfType<GameSession>();
		gameStatus.ResetGame();
		SceneManager.LoadScene(0);
	}

	public void LoadNextScene() {
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		int nextSceneIndex = currentSceneIndex + 1;

		SceneManager.LoadScene(nextSceneIndex);
	}

	public void Quit() {
		Application.Quit();
	}
}
