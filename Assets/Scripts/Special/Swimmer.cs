using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swimmer : Block {
	[SerializeField] float MoveSpeed = 0.4f;
	[SerializeField] float XMovementAllowedRightOfStart;
	[SerializeField] float XMovementAllowedLeftOfStart;

	bool MovingRight = true;
	float YMovement;
	float StartingX = -999f;
	float StartingY;
	float SineMultiplier = 2.5f;
	float YRotation;
	float CurMoveSpeed;

	// Update is called once per frame
	void Update() {
		AdjustBallSpeed();
		SetGlobalValues();
		SetObjectSpecificValues();
		UpdateMovementPositions();
	}

	private void SetGlobalValues() {
		CurMoveSpeed = MoveSpeed * Time.deltaTime;
	}

	private void SetObjectSpecificValues() {
		if (StartingX == -999f) {
			StartingX = transform.position.x;
			StartingY = transform.position.y;
		}
	}

	private void UpdateMovementPositions() {
		if (MovingRight == false && transform.position.x < (StartingX - XMovementAllowedLeftOfStart)) {
			if (name.Contains("Fish")) {
				YRotation = 180;
			} else {
				YRotation = 0;
			}
			transform.rotation = new Quaternion(0, YRotation, 0, 0);
			MoveSpeed *= -1;
			MovingRight = true;
		} else if (MovingRight && transform.position.x > (StartingX + XMovementAllowedRightOfStart)) {
			if (name.Contains("Fish")) {
				YRotation = 0;
			} else {
				YRotation = 180;
			}
			transform.rotation = new Quaternion(0, YRotation, 0, 0);
			MoveSpeed *= -1;
			MovingRight = false;
		}

		if (name.Contains("Fish")) {
			YMovement = transform.position.y;
		} else {
			YMovement = Mathf.Sin(Time.time * SineMultiplier) + StartingY - 1;
		}
		transform.position = new Vector3(transform.position.x + CurMoveSpeed, YMovement);
	}
}
