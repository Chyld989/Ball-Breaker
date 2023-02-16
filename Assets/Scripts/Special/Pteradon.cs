using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pteradon : Block {
	enum FlyingDirection {
		DownRight = 1,
		UpRight = 2,
		DownLeft = 3,
		UpLeft = 4
	}

	[SerializeField] float MoveSpeed = 0.4f;
	[SerializeField] FlyingDirection CurrentDirection = FlyingDirection.UpLeft;
	[SerializeField] float MaxHeight = 10.35f;
	[SerializeField] float MinHeight = 6.15f;
	[SerializeField] float MaxLeft = 1.75f;
	[SerializeField] float MaxRight = 14.25f;

	[SerializeField] float MoveHeightAllowed;
	[SerializeField] float MoveWidthAllowed;
	[SerializeField] float MoveSpeedX;
	[SerializeField] float MoveSpeedY;
	[SerializeField] float MovementRatioXToY;

	float StartingX = -999f;
	float StartingY;
	float YRotation;
	float CurMoveSpeedX;
	float CurMoveSpeedY;

	// Update is called once per frame
	void Update() {
		AdjustBallSpeed();
		SetGlobalValues();
		SetObjectSpecificValues();
		UpdateMovementPositions();
	}

	private void SetGlobalValues() {
		if (StartingX == -999f) {
			MoveHeightAllowed = MaxHeight - MinHeight;
			MoveWidthAllowed = MaxRight - MaxLeft;
			MovementRatioXToY = MoveWidthAllowed / MoveHeightAllowed;
			MoveSpeedX = MoveSpeed * MovementRatioXToY;
			MoveSpeedY = MoveSpeed;
		}
		CurMoveSpeedX = MoveSpeedX * Time.deltaTime;
		CurMoveSpeedY = MoveSpeedY * Time.deltaTime;
	}

	private void SetObjectSpecificValues() {
		if (StartingX == -999f) {
			StartingX = transform.position.x;
			StartingY = transform.position.y;
			YRotation = transform.rotation.y;
		}
	}

	private void UpdateMovementPositions() {
		switch (CurrentDirection) {
			// X movement = 13.5
			// Y movement = 4.2
			case FlyingDirection.UpLeft:
				// Moving straight up on the left side of the screen
				MoveStraightUp();
				break;
			case FlyingDirection.DownRight:
				// Moving diagonal from top left to bottom right
				if (transform.position.x > MaxRight && transform.position.y < MinHeight) {
					CurrentDirection = FlyingDirection.UpRight;
				} else {
					transform.position = new Vector3(transform.position.x + CurMoveSpeedX, transform.position.y - CurMoveSpeedY);
				}
				break;
			case FlyingDirection.UpRight:
				// Moving straight up on the right side of the screen
				MoveStraightUp();
				break;
			case FlyingDirection.DownLeft:
				// Moving diagonal from top right to bottom left
				if (transform.position.x < MaxLeft && transform.position.y < MinHeight) {
					CurrentDirection = FlyingDirection.UpLeft;
				} else {
					transform.position = new Vector3(transform.position.x - CurMoveSpeedX, transform.position.y - CurMoveSpeedY);
				}
				break;
		}
	}

	private void MoveStraightUp() {
		if (transform.position.y > MaxHeight) {
			// Turn around
			TurnAround();
			if (CurrentDirection == FlyingDirection.UpLeft) {
				CurrentDirection = FlyingDirection.DownRight;
			} else {
				CurrentDirection = FlyingDirection.DownLeft;
			}
		} else {
			transform.position = new Vector3(transform.position.x, transform.position.y + CurMoveSpeedY);
		}
	}

	private void TurnAround() {
		YRotation += 1;
		if (YRotation >= 2) {
			YRotation -= 2;
		}
		transform.rotation = new Quaternion(0, YRotation, 0, 0);
	}
}
