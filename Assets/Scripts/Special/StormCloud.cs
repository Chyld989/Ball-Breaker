using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StormCloud : MonoBehaviour {
	[SerializeField] GameObject LightningBoltFlash;
	[SerializeField] GameObject SingleLightningBolt;
	[SerializeField] float CloudMoveSpeed = 0.4f;
	[SerializeField] float XMovementAllowedRightOfStart;
	[SerializeField] float XMovementAllowedLeftOfStart;
	[SerializeField] float BoltMoveSpeed = 0.4f;

	bool Active = false;
	float ActivateTime = -1f;
	float ActivateDelay = 1f;
	float DeactivateTime = -1f;
	float DeactivateAfter = 0.1f;
	int TimesToActivate = 3;
	int TimesActivated = 0;
	bool MovingRight = true;
	float CloudStartingX;
	SpriteRenderer LightningFlashSpriteRenderer;
	SpriteRenderer LightningBoltSpriteRenderer;
	//PolygonCollider2D LightningFlashPolygonCollider2D;
	PolygonCollider2D LightningBoltPolygonCollider2D;
	float LightningBoltStartingY;
	float LightningBoltLaunchX;
	float CurCloudMoveSpeed;
	float CurBoltMoveSpeed;

	// Start is called before the first frame update
	void Start() {
		CurCloudMoveSpeed = CloudMoveSpeed * Time.deltaTime;
		CurBoltMoveSpeed = BoltMoveSpeed * Time.deltaTime;

		LightningFlashSpriteRenderer = LightningBoltFlash.GetComponent<SpriteRenderer>();
		LightningBoltSpriteRenderer = SingleLightningBolt.GetComponent<SpriteRenderer>();
		LightningBoltPolygonCollider2D = SingleLightningBolt.GetComponent<PolygonCollider2D>();

		LightningBoltStartingY = LightningBoltSpriteRenderer.transform.position.y;

		CloudStartingX = transform.position.x;
	}

	private void SetActivateDeactivateTimes() {
		if (ActivateTime == -1f) {
			ActivateTime = Time.time + ActivateDelay;
			DeactivateTime = ActivateTime + DeactivateAfter;
		}
	}

	// Update is called once per frame
	void Update() {
		ActivateHazard();
		MoveHazard();
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		SetActivateDeactivateTimes();
	}

	private void ActivateHazard() {
		if (ActivateTime != -1f && Time.time >= ActivateTime) {
			if (Active == false) {
				Active = true;
				LightningFlashSpriteRenderer.enabled = true;
			}
		}
		if (DeactivateTime != -1f && Time.time >= DeactivateTime) {
			if (Active) {
				Active = false;
				LightningFlashSpriteRenderer.enabled = false;
				TimesActivated++;
				if (TimesActivated < TimesToActivate) {
					ActivateTime = Time.time + ActivateDelay;
					DeactivateTime = ActivateTime + DeactivateAfter;
				} else {
					ActivateTime = -1f;
					DeactivateTime = -1f;
					LightningBoltLaunchX = LightningBoltSpriteRenderer.transform.position.x;
					LightningBoltSpriteRenderer.enabled = true;
					LightningBoltPolygonCollider2D.enabled = true;
				}
			}
		}
	}

	private void MoveHazard() {
		MoveCloud();
		MoveBolt();
	}

	private void MoveCloud() {
		if (MovingRight && transform.position.x > (CloudStartingX + XMovementAllowedRightOfStart)) {
			CurCloudMoveSpeed *= -1;
			MovingRight = false;
		}
		if (MovingRight == false && transform.position.x < (CloudStartingX - XMovementAllowedRightOfStart)) {
			CurCloudMoveSpeed *= -1;
			MovingRight = true;
		}
		transform.position = new Vector3(transform.position.x + CurCloudMoveSpeed, transform.position.y, transform.position.z);
	}

	private void MoveBolt() {
		if (LightningBoltSpriteRenderer.enabled) {
			SingleLightningBolt.transform.position = new Vector3(LightningBoltLaunchX, SingleLightningBolt.transform.position.y - CurBoltMoveSpeed, transform.position.z + 1);
			if (SingleLightningBolt.transform.position.y < -3) {
				LightningBoltSpriteRenderer.enabled = false;
				LightningBoltPolygonCollider2D.enabled = false;
				LightningBoltSpriteRenderer.transform.position = new Vector3(transform.position.x, LightningBoltStartingY);
				TimesActivated = 0;
			}
		}
	}
}
