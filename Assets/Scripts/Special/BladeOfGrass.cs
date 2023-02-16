using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeOfGrass : MonoBehaviour {
	[SerializeField] float MoveSpeed;
	[SerializeField] float RotateSpeed;
	[SerializeField] float FadeAfter;
	[SerializeField] float FadeSpeed;

	float CurrentRotation;
	float MaxY;
	float StartingY;
	float PositionY;
	float PositionX;
	float SpawnTime;
	SpriteRenderer SpriteRenderer;
	float CurrentAlpha;
	Color[] greens = { new Color(165 / 255f, 207 / 255f, 136 / 255f, 255 / 255f), new Color(135 / 255f, 189 / 255f, 112 / 255f, 255 / 255f), new Color(52 / 255f, 96 / 255f, 63 / 255f, 255 / 255f) };
	int color;

	// Start is called before the first frame update
	void Start() {
		CurrentRotation = transform.rotation.z;
		StartingY = transform.position.y + 0.75f;
		RotateSpeed = Random.Range(250f, 315f);
		MoveSpeed = 0.5f;
		if (transform.position.x > 8) {
			RotateSpeed *= -1;
		} else {
			MoveSpeed *= -1;
		}
		MaxY = transform.position.y + 1f;
		SpawnTime = Time.time;
		SpriteRenderer = GetComponent<SpriteRenderer>();
		CurrentAlpha = 1f;
		color = Random.Range(0, 3);
		SpriteRenderer.color = new Color(greens[color].r, greens[color].g, greens[color].b);
	}

	// Update is called once per frame
	void Update() {
		var curMoveSpeed = MoveSpeed * Time.deltaTime;
		PositionX = transform.position.x + curMoveSpeed;
		PositionY = Mathf.Sin(Time.time - SpawnTime) + StartingY - 1;
		transform.position = new Vector3(PositionX, PositionY, transform.position.z);

		var curRotateSpeed = RotateSpeed * Time.deltaTime;
		CurrentRotation += curRotateSpeed;
		transform.rotation = Quaternion.Euler(0, 0, CurrentRotation);
		if (Time.time > (SpawnTime + 2f)) {
			CurrentAlpha -= 0.5f * Time.deltaTime;
		}
		SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, CurrentAlpha);
	}
}
