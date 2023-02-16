using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Block : MonoBehaviour {
	[SerializeField] AudioClip BreakSound;
	[SerializeField] GameObject BlockDestroyedVfx;
	[SerializeField] Sprite[] HitSprites;

	protected Level Level;
	Ball Ball;
	bool SlowMotion = false;
	float SlowMotionFactor;
	float SlowMotionDistanceTrigger;
	int MaxHits;

	[SerializeField] int TimesHit; // Serialized for debugging purposes only

	private void Start() {
		Level = FindObjectOfType<Level>();
		CountBreakableBlocks();
		Time.timeScale = 1;
		SlowMotionFactor = Level.SlowMotionFactor;
		SlowMotionDistanceTrigger = Level.SlowMotionDistanceTrigger;
		Ball = FindObjectOfType<Ball>();
	}

	private void CountBreakableBlocks() {
		if (tag == "Breakable") {
			Level.CountBlocks();
		}
	}

	private void Update() {
		AdjustBallSpeed();
	}

	protected void AdjustBallSpeed() {
		if (tag == "Breakable") {
			if (OneHitLeft()) {
				// TODO: Something to make sure time doesn't slow down until the ball hits another object (to make sure time doesn't slow immediately after the 2nd hit of the last block on the board)
				if (Vector3.Distance(transform.position, Ball.transform.position) < SlowMotionDistanceTrigger) {
					if (SlowMotion == false) {
						Ball.GetComponent<Rigidbody2D>().velocity = new Vector2(Ball.GetComponent<Rigidbody2D>().velocity.x / SlowMotionFactor, Ball.GetComponent<Rigidbody2D>().velocity.y / SlowMotionFactor);
						SlowMotion = true;
					}
				} else {
					if (SlowMotion) {
						Ball.GetComponent<Rigidbody2D>().velocity = new Vector2(Ball.GetComponent<Rigidbody2D>().velocity.x * SlowMotionFactor, Ball.GetComponent<Rigidbody2D>().velocity.y * SlowMotionFactor);
						SlowMotion = false;
					}
				}
			}
		}
	}

	private bool OneHitLeft() {
		return (Level.BreakingBlocksRemaining == 1 && TimesHit == (MaxHits - 1));
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (tag == "Breakable") {
			HandleHit();
		}
	}

	private void HandleHit() {
		if (Level == null) {
			Level = FindObjectOfType<Level>();
		}
		TimesHit++;
		MaxHits = HitSprites.Length + 1;
		if (TimesHit >= MaxHits) {
			DestroyBlock();
		} else {
			UpdateDamageLevel();
		}
	}

	private void DestroyBlock() {
		DestroyBlockPlayAudio();
		Level.BlockDestroyed();
		FindObjectOfType<GameSession>().AddToScore();
		TriggerDestroyedVfx();
		Destroy(gameObject);
	}

	private void UpdateDamageLevel() {
		var hitSpritesIndex = TimesHit - 1;
		if (HitSprites[hitSpritesIndex] != null) {
			GetComponent<SpriteRenderer>().sprite = HitSprites[hitSpritesIndex];
		} else {
			Debug.LogError($"Block sprite is missing from array (Game object: {gameObject.name}; hitSpritesIndex: {hitSpritesIndex}).");
		}
	}

	protected void DestroyBlockPlayAudio() {
		AudioSource.PlayClipAtPoint(BreakSound, Camera.main.transform.position);
	}

	protected void TriggerDestroyedVfx() {
		GameObject vfx = Instantiate(BlockDestroyedVfx, transform.position, transform.rotation);
		Destroy(vfx, 1f);
	}
}
