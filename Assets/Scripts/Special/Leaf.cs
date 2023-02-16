using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour {
	enum FloatDirection {
		Left = 1,
		Right = 2
	}

	[SerializeField] float RotateSpeed = 0.2f;
	[SerializeField] float FallSpeed = 0.6f;

	float LowRotationRange;
	float HighRotationRange;
	float CurrentRotation;
	float CurFallSpeed;
	FloatDirection CurrentDirection;

	// Start is called before the first frame update
	void Start() {
		HighRotationRange = UnityEngine.Random.Range(20f, 50f);
		LowRotationRange = HighRotationRange * -1;
		CurrentRotation = UnityEngine.Random.Range(LowRotationRange, HighRotationRange);
		if (UnityEngine.Random.Range(0, 2) == 1) {
			CurrentDirection = FloatDirection.Left;
		} else {
			CurrentDirection = FloatDirection.Right;
		}
		transform.rotation = new Quaternion(0, 0, CurrentRotation, 0);
	}

	// Update is called once per frame
	void Update() {
		CheckIfDirectionNeedsToChange();
		AdjustRotation();
		ApplyRotation();
		DropLeaf();
	}

	private void DropLeaf() {
		CurFallSpeed = FallSpeed * Time.deltaTime;
		transform.position = new Vector3(transform.position.x, transform.position.y - CurFallSpeed, transform.position.z);
	}

	public void SetColor(int r, int g, int b) {
		GetComponent<SpriteRenderer>().color = new Color(r/255f, g/255f, b/255f);
	}

	private void CheckIfDirectionNeedsToChange() {
		if (CurrentDirection == FloatDirection.Left && CurrentRotation < LowRotationRange) {
			CurrentDirection = FloatDirection.Right;
		} else if (CurrentDirection == FloatDirection.Right && CurrentRotation > HighRotationRange) {
			CurrentDirection = FloatDirection.Left;
		}
	}

	private void AdjustRotation() {
		if (CurrentDirection == FloatDirection.Left) {
			CurrentRotation -= RotateSpeed;
		} else {
			CurrentRotation += RotateSpeed;
		}
	}

	private void ApplyRotation() {
		transform.rotation = Quaternion.Euler(0, 0, CurrentRotation);
	}
}
