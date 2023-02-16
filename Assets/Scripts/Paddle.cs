using UnityEngine;

public class Paddle : MonoBehaviour {
	[SerializeField] float ScreenWidthInUnits = 16f;
	[SerializeField] float PaddleWidthInUnits = 2f;
	[SerializeField] float StunLength = 2f;

	GameObject Ball;
	bool Stunned = false;
	float StunDeactivateTime = -1f;
	GameSession GameSession;

	private void Start() {
		foreach (var gameObject in FindObjectsOfType<GameObject>()) {
			if (gameObject.name.Contains("Ball")) {
				Ball = gameObject;
			}
		}
		GameSession = FindObjectOfType<GameSession>();
	}

	// Update is called once per frame
	void Update() {
		UnStun();
		ToggleAutoPlayMode();
		if (Stunned == false) {
			if (GameSession.IsAutoPlayOn()) {
				FollowBall();
			} else {
				FollowMouse();
			}
		}
	}

	private void ToggleAutoPlayMode() {
		if (Input.GetKeyDown(KeyCode.A)) {
			GameSession.ChangeAutoPlayMode();
		}
	}

	private void FollowBall() {
		Vector2 paddlePosition = new Vector2(transform.position.x, transform.position.y);
		paddlePosition.x = Mathf.Clamp(Ball.transform.position.x, PaddleWidthInUnits / 2, ScreenWidthInUnits - (PaddleWidthInUnits / 2));
		transform.position = paddlePosition;
	}

	private void FollowMouse() {
		var mouseXPositionInUnits = Input.mousePosition.x / Screen.width * ScreenWidthInUnits;
		Vector2 paddlePosition = new Vector2(transform.position.x, transform.position.y);
		paddlePosition.x = Mathf.Clamp(mouseXPositionInUnits, PaddleWidthInUnits / 2, ScreenWidthInUnits - (PaddleWidthInUnits / 2));
		transform.position = paddlePosition;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Hazard-Stun") {
			Stun();
		}
	}

	public void Stun() {
		Stunned = true;
		StunDeactivateTime = Time.time + StunLength;
	}

	private void UnStun() {
		if (Stunned) {
			if (StunDeactivateTime != -1f && Time.time >= StunDeactivateTime) {
				Stunned = false;
			}
		}
	}
}
