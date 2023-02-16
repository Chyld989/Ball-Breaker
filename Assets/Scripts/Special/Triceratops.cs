using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Triceratops : Block {
	enum MovingDirection {
		Forward = 1,
		Backward = 2
	}

	[SerializeField] float MoveSpeedForward = 0.4f;
	[SerializeField] float MoveSpeedBackward = 0.2f;
	[SerializeField] GameObject[] BladesOfGrass;

	MovingDirection CurrentDirection = MovingDirection.Backward;
	float StartingX = -999f;
	float MiddleX;
	float YRotation;
	float CurMoveSpeedForward;
	float CurMoveSpeedBackward;

	// Update is called once per frame
	void Update() {
		AdjustBallSpeed();
		SetGlobalValues();
		SetObjectSpecificValues();
		UpdateMovementPositions();
	}

	private void SetGlobalValues() {
		CurMoveSpeedForward = MoveSpeedForward * Time.deltaTime;
		CurMoveSpeedBackward = MoveSpeedBackward * Time.deltaTime;
	}

	private void SetObjectSpecificValues() {
		if (StartingX == -999f) {
			StartingX = transform.position.x;
			if (name.Contains("(Large) Left")) {
				MiddleX = 8.3f;
			} else if (name.Contains("(Large) Right")) {
				MiddleX = 10.92f;
			} else if (name.Contains("(Medium) Left")) {
				MiddleX = 8.64f;
			} else if (name.Contains("(Medium) Right")) {
				MiddleX = 10.58f;
			} else if (name.Contains("(Small) Left")) {
				MiddleX = 8.96f;
			} else if (name.Contains("(Small) Right")) {
				MiddleX = 10.26f;
			} else {
				throw new Exception($"Unknown Triceratops detected! {name}");
			}
			YRotation = transform.rotation.y;
			if (YRotation != 0) {
				MoveSpeedForward *= -1;
				MoveSpeedBackward *= -1;
			}
		}
	}

	private void UpdateMovementPositions() {
		switch (CurrentDirection) {
			case MovingDirection.Forward:
				if ((name.Contains("Left") && transform.position.x >= MiddleX) || (name.Contains("Right") && transform.position.x <= MiddleX)) {
					// Need to change directions
					CurrentDirection = MovingDirection.Backward;
				} else {
					transform.position = new Vector3(transform.position.x + CurMoveSpeedForward, transform.position.y);
				}
				break;
			case MovingDirection.Backward:
				if ((name.Contains("Left") && transform.position.x <= StartingX) || (name.Contains("Right") && transform.position.x >= StartingX)) {
					CurrentDirection = MovingDirection.Forward;
					for (int i = 0; i < UnityEngine.Random.Range(3, 7); i++) {
						SpawnGrass();
					}
				} else {
					transform.position = new Vector3(transform.position.x + -CurMoveSpeedBackward, transform.position.y);
				}
				break;
		}
	}

	private void SpawnGrass() {
		var grassX = transform.position.x;
		var grassY = transform.position.y;
		grassX = UpdateGrassLocationX(grassX);
		grassY = UpdateGrassLocationY(grassY);
		GameObject bladeOfGrass = Instantiate(BladesOfGrass[UnityEngine.Random.Range(0, 6)], new Vector3(grassX, grassY, 0.5f), new Quaternion(0, 0, 0, 0));
		Destroy(bladeOfGrass, 5f);
	}

	private float UpdateGrassLocationY(float grassY) {
		grassY = grassY - ((transform.GetComponent<SpriteRenderer>().size.y / 13) * transform.localScale.y);
		return grassY;
	}

	private float UpdateGrassLocationX(float grassX) {
		if (transform.rotation.y == 0) {
			grassX = grassX - ((transform.GetComponent<SpriteRenderer>().size.x / 13) * transform.localScale.x);
		} else {
			grassX = grassX + ((transform.GetComponent<SpriteRenderer>().size.x / 13) * transform.localScale.x);
		}
		grassX += (UnityEngine.Random.Range(-0.25f, 0.25f) * transform.localScale.x);
		return grassX;
	}
}
