using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {
	[SerializeField] Paddle Paddle1;
	[SerializeField] float VelocityX = 2f;
	[SerializeField] float VelocityY = 12f;
	[SerializeField] AudioClip[] BallSounds;
	[SerializeField] Sprite ClosedEgg;
	[SerializeField] float ScreenWidthInUnits = 16f;
	[SerializeField] float RandomFactor = 0.2f;

	bool AutoLaunchBall = false;
	Vector2 PaddleToBallVector;
	public bool IsLaunched = false;
	float Speed = 0f;
	AudioSource AudioSource;
	float ResetAtTime = -1f;
	Level Level;
	Sprite OpenEgg;
	SpriteRenderer SpriteRenderer;
	Rigidbody2D Rigidbody2D;
	SpriteRenderer EggSpeechText;

	// Start is called before the first frame update
	void Start() {
		Level = FindObjectOfType<Level>();
		PaddleToBallVector = transform.position - Paddle1.transform.position;
		AudioSource = GetComponent<AudioSource>();
		SpriteRenderer = GetComponent<SpriteRenderer>();
		Rigidbody2D = GetComponent<Rigidbody2D>();
		OpenEgg = SpriteRenderer.sprite;
	}

	// Update is called once per frame
	void Update() {
		SetEggSpeechText();
		if (IsLaunched == false) {
			LockToPaddle();
			LockTextToEgg();
			LaunchOnMouseClick();
		} else {
			if (Level.NextLevelTime != -1f) {
				StopBall();
			} else {
				Speed = Rigidbody2D.velocity.magnitude;
				StartResetBallCountdown();
				if (ResetAtTime != -1f && Time.time >= ResetAtTime) {
					ResetBallPosition();
					ResetAtTime = -1f;
				}
			}
		}
	}

	private void StopBall() {
		Rigidbody2D.velocity = new Vector2(0, 0);
		Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
		SpriteRenderer.sprite = OpenEgg;
	}

	private void SetEggSpeechText() {
		if (EggSpeechText == null) {
			foreach (var curSpriteRenderer in FindObjectsOfType<SpriteRenderer>()) {
				if (curSpriteRenderer.name.Length > 12) {
					if (curSpriteRenderer.name.Substring(curSpriteRenderer.name.Length - 12, 12) == "_Speech_Text" && curSpriteRenderer.name.Contains("Mom") == false) {
						EggSpeechText = curSpriteRenderer;
						break;
					}
				}
			}
		}
	}

	private void LockTextToEgg() {
		if (EggSpeechText != null) {
			//EggSpeechText.enabled = true;
			var curX = transform.position.x;
			if (curX < (EggSpeechText.size.x / 2)) {
				curX = (EggSpeechText.size.x / 2);
			} else if (curX > (ScreenWidthInUnits - (EggSpeechText.size.x / 2))) {
				curX = (ScreenWidthInUnits - (EggSpeechText.size.x / 2));
			}
			var curY = EggSpeechText.transform.position.y;
			var curZ = EggSpeechText.transform.position.z;
			EggSpeechText.transform.position = new Vector3(curX, curY, curZ);
		}
	}

	private void DisappearEggSpeechText() {
		if (EggSpeechText != null) {
			EggSpeechText.enabled = false;
		}
	}

	private void OnGUI() {
		//GUI.Box(new Rect(10, 10, 100, 60), "Ball Velocity");
		//GUI.Label(new Rect(20, 40, 80, 20), Speed + "m/s");
	}

	private void LaunchOnMouseClick() {
		if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space) || AutoLaunchBall) {
			IsLaunched = true;
			SpriteRenderer.sprite = ClosedEgg;
			Rigidbody2D.velocity = new Vector2(VelocityX, VelocityY);
			DisappearEggSpeechText();
		}
	}

	private void LockToPaddle() {
		Vector2 paddlePosition = new Vector2(Paddle1.transform.position.x, Paddle1.transform.position.y);
		transform.position = paddlePosition + PaddleToBallVector;
		transform.rotation = new Quaternion(0, 0, 0, 0);
	}

	private void StartResetBallCountdown() {
		if (Input.GetKeyDown(KeyCode.R)) {
			ResetAtTime = Time.time + 5f;
		}
	}

	public void ResetBallPosition() {
		LockToPaddle();
		SpriteRenderer.sprite = OpenEgg;
		IsLaunched = false;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		var velocityTweak = new Vector2(UnityEngine.Random.Range(0f, RandomFactor), UnityEngine.Random.Range(0f, RandomFactor));

		if (IsLaunched) {
			AudioClip audioClip = BallSounds[UnityEngine.Random.Range(0, BallSounds.Length)];
			AudioSource.PlayOneShot(audioClip);
			Rigidbody2D.velocity += velocityTweak;
		}
	}
}
