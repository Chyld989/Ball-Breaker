using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour {
	GameSession GameSession;
	Ball Ball;

	private void Start() {
		GameSession = FindObjectOfType<GameSession>();
		Ball = FindObjectOfType<Ball>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.name == "Pixelated Egg Ball") {
			GameSession.LoseLife();
			if (GameSession.Lives <= 0) {
				LoadGameOverScene();
			} else {
				Ball.ResetBallPosition();
			}
		}
	}

	private void LoadGameOverScene() {
		SceneManager.LoadScene("Game Over");
	}
}
