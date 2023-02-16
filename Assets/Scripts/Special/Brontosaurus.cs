using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brontosaurus : Block {
	enum Status {
		Eating = 1,
		Running = 2,
		Waiting = 3,
		Returning = 4
	}

	[SerializeField] float RunAwaySpeed = 1f;
	[SerializeField] float ReturnSpeed = 0.4f;
	[SerializeField] float ScareLengthSeconds = 3f;

	Tree TreeScript = null;
	float StartingX = -999f;
	float YRotation;
	float MovementDirection;
	float CurRunAwaySpeed;
	float CurReturnSpeed;
	Status CurrentStatus = Status.Eating;
	float ReturnTime = -1f;

	// Update is called once per frame
	void Update() {
		AdjustBallSpeed();
		SetGlobalValues();
		SetObjectSpecificValues();
		UpdateMovementPositions();
	}

	private void SetGlobalValues() {
		CurRunAwaySpeed = RunAwaySpeed * Time.deltaTime;
		CurReturnSpeed = ReturnSpeed * Time.deltaTime;
	}

	private void SetObjectSpecificValues() {
		if (StartingX == -999f) {
			StartingX = transform.position.x;
			YRotation = transform.rotation.y;
			if (YRotation == 0) {
				MovementDirection = -1f;
			} else {
				MovementDirection = 1f;
			}
		}
	}

	private void UpdateMovementPositions() {
		switch (CurrentStatus) {
			case Status.Eating:
				// Do nothing, dino is just chillin' while munching on some branches
				tag = "Breakable";
				break;
			case Status.Running:
				if ((MovementDirection == -1f && transform.position.x <= -1.25f) || (MovementDirection == 1f && transform.position.x >= 17f)) {
					CurrentStatus = Status.Waiting;
					if (transform.rotation.y == 0) {
						transform.rotation = new Quaternion(0, 180, 0, 0);
					} else {
						transform.rotation = new Quaternion(0, 0, 0, 0);
					}
					ReturnTime = Time.time + ScareLengthSeconds;
				} else {
					transform.position = new Vector3(transform.position.x + (CurRunAwaySpeed * MovementDirection), transform.position.y, transform.position.z);
				}
				break;
			case Status.Waiting:
				if (ReturnTime != -1f && Time.time >= ReturnTime) {
					CurrentStatus = Status.Returning;
					ReturnTime = -1f;
				}
				break;
			case Status.Returning:
				if ((MovementDirection == -1f && transform.position.x >= StartingX) || (MovementDirection == 1f && transform.position.x <= StartingX)) {
					if (transform.rotation.y == 0) {
						transform.rotation = new Quaternion(0, 180, 0, 0);
					} else {
						transform.rotation = new Quaternion(0, 0, 0, 0);
					}
					CurrentStatus = Status.Eating;
					TreeScript.TreeStopsShaking();
				} else {
					transform.position = new Vector3(transform.position.x + (CurReturnSpeed * MovementDirection * -1), transform.position.y, transform.position.z);
				}
				break;
		}
	}

	public void ScareDinosaur() {
		if (CurrentStatus == Status.Eating) {
			CurrentStatus = Status.Running;
			tag = "Unbreakable";
		}
	}

	public void SetCorrespondingTree(Tree tree) {
		TreeScript = tree;
	}

	private void OnDestroy() {
		TreeScript.DinosaurDestroyed();
	}
}
