using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour {
	[Range(0.1f, 10)] [SerializeField] float GameSpeed = 1f;
	[SerializeField] int PointsPerBlock = 10;
	[SerializeField] TextMeshProUGUI ScoreText;
	[SerializeField] TextMeshProUGUI LivesText;
	[SerializeField] public int Lives = 3; // Serialized for debugging purposes only
	[SerializeField] int CurrentScore = 0; // Serialized for debugging purposes only

	bool AutoPlayMode = false;

	private void Awake() {
		int gameStatusCount = FindObjectsOfType<GameSession>().Length;

		if (gameStatusCount > 1) {
			gameObject.SetActive(false);
			Destroy(gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
		}
	}

	private void Start() {
		UpdateScore();
		UpdateLives();
	}

	// Update is called once per frame
	void Update() {
		Time.timeScale = GameSpeed;
	}

	public void AddToScore() {
		CurrentScore += PointsPerBlock;
		UpdateScore();
	}

	private void UpdateScore() {
		ScoreText.text = $"Score: {CurrentScore}";
	}

	public void LoseLife() {
		Lives--;
		UpdateLives();
	}

	public void AddLife() {
		Lives++;
		UpdateLives();
	}

	private void UpdateLives() {
		LivesText.text = $"Lives: {Lives}";
	}

	public void ResetGame() {
		Destroy(gameObject);
	}

	public void ChangeAutoPlayMode() {
		AutoPlayMode = !AutoPlayMode;
	}

	public bool IsAutoPlayOn() {
		return AutoPlayMode;
	}
}
