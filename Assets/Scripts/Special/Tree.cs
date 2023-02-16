using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class Tree : Block {
	[SerializeField] GameObject Leaf;

	Brontosaurus DinosaurEatingScript;
	bool DinosaurScriptSet = false;
	bool DinosaurIsDestroyed = false;
	bool TreeIsShaking = false;

	private void Update() {
		if (DinosaurScriptSet == false) {
			var correspondingDinosaurName = name.Replace("EdibleTreeSmall", "Brontosaurus");
			correspondingDinosaurName = correspondingDinosaurName.Replace("EdibleTreeLarge", "Brontosaurus");
			foreach (var gameObject in FindObjectsOfType<GameObject>()) {
				if (gameObject.name == correspondingDinosaurName) {
					DinosaurEatingScript = gameObject.GetComponent<Brontosaurus>();
					DinosaurEatingScript.SetCorrespondingTree(this);
					DinosaurScriptSet = true;
					break;
				}
			}
		}
		AdjustBallSpeed();
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.name == "Pixelated Egg Ball") {
			ShakeTree();
		}
	}

	private void ShakeTree() {
		if (TreeIsShaking == false) {
			TreeIsShaking = true;
			SpawnLeaves();
			ScareDinosaur();
		}
	}

	private void SpawnLeaves() {
		int leafCount = UnityEngine.Random.Range(3, 6);
		for (int i = 0; i < leafCount; i++) {
			float leafX = UnityEngine.Random.Range(0, 1);
			int leafY = i + 3;
			var leafSpawnLocationY = transform.position.y + (GetComponent<SpriteRenderer>().size.y / leafY);
			GameObject leaf = Instantiate(Leaf, new Vector3(transform.position.x + (i * leafX), leafSpawnLocationY), new Quaternion(0, 0, 0, 0));
			var leafScript = leaf.GetComponent<Leaf>();
			switch (leafY) {
				case 3:
					leafScript.SetColor(107, 162, 74);
					break;
				case 4:
					leafScript.SetColor(78, 125, 82);
					break;
				case 5:
					leafScript.SetColor(74, 117, 79);
					break;
				case 6:
					leafScript.SetColor(62, 97, 41);
					break;
				default:
					leafScript.SetColor(49, 78, 77);
					break;
			}
			Destroy(leaf, 20f);
		}
	}

	public void TreeStopsShaking() {
		TreeIsShaking = false;
	}

	private void ScareDinosaur() {
		if (DinosaurIsDestroyed == false) {
			DinosaurEatingScript.ScareDinosaur();
		}
	}

	public void DinosaurDestroyed() {
		DinosaurIsDestroyed = true;
		TreeIsShaking = true;
	}
}
