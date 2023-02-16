using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
	[SerializeField] public int BreakingBlocksRemaining; // Serialized for debugging purposes only
	[SerializeField] public float SlowMotionDistanceTrigger = 1.5f;
	[SerializeField] public float SlowMotionFactor = 8f;
	[SerializeField] float ScreenWidthInUnits = 16f;

	SceneLoader SceneLoader;
	public float NextLevelTime = -1f;
	SpriteRenderer MomNotMomSpriteRenderer;
	GameObject Ball;

	// Start is called before the first frame update
	void Start() {
		SceneLoader = GetComponent<SceneLoader>();
	}

	private void Update() {
		if (MomNotMomSpriteRenderer == null) {
			foreach (var curSpriteRenderer in FindObjectsOfType<SpriteRenderer>()) {
				if (curSpriteRenderer.name.Length >= 15) {
					if (curSpriteRenderer.name.Substring(curSpriteRenderer.name.Length - 15, 15) == "Mom_Speech_Text") {
						MomNotMomSpriteRenderer = curSpriteRenderer;
						break;
					}
				}
			}
		}
		if (NextLevelTime != -1f && Time.time >= NextLevelTime) {
			NextLevelTime = -1f;
			MomNotMomSpriteRenderer.enabled = false;
			GameSession gameSession = FindObjectOfType<GameSession>();
			gameSession.AddLife();
			SceneLoader.LoadNextScene();
		}
	}

	public void CountBlocks() {
		BreakingBlocksRemaining++;
	}

	public void BlockDestroyed() {
		BreakingBlocksRemaining--;
		if (BreakingBlocksRemaining <= 0) {
			PositionMomNotMomText();
			NextLevelTime = Time.time + 5f;
		}
	}

	private void PositionMomNotMomText() {
		if (MomNotMomSpriteRenderer != null) {
			foreach (var curGameObject in FindObjectsOfType<GameObject>()) {
				if (curGameObject.name.Contains("Ball")) {
					Ball = curGameObject;
					break;
				}
			}
			var curX = Ball.transform.position.x;
			if (curX < (MomNotMomSpriteRenderer.size.x / 2)) {
				curX = MomNotMomSpriteRenderer.size.x / 2;
			} else if (curX > (ScreenWidthInUnits - (MomNotMomSpriteRenderer.size.x / 2))) {
				curX = ScreenWidthInUnits - (MomNotMomSpriteRenderer.size.x / 2);
			}
			var curY = Ball.transform.position.y;
			if (MomNotMomSpriteRenderer.name == "Mom_Speech_Text") {
				curY = curY + 0.75f;
			} else {
				curY = curY + 0.9f;
			}
			if (curY > ((ScreenWidthInUnits * 0.75f) - (MomNotMomSpriteRenderer.size.y / 2))) {
				curY = (ScreenWidthInUnits * 0.75f) - (MomNotMomSpriteRenderer.size.y / 2);
			}
			MomNotMomSpriteRenderer.transform.position = new Vector3(curX, curY, MomNotMomSpriteRenderer.transform.position.z);
			MomNotMomSpriteRenderer.enabled = true;
		}
	}
}
