using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoardPiece : MonoBehaviour {
    public enum PieceType { wall, ground }

    public PieceType type;

	public bool seen = false;

    public static float killTime = 2f;

	// Terminate blocks when they are not visible for a certain kill time
	private void OnBecameVisible() {
		seen = true;
		StopCoroutine("Destroy");
	}

	private void OnBecameInvisible() {
		if (seen) {
			StartCoroutine("Destroy");
		}
	}

	private void OnDestroy() {
		GetComponentInParent<BoardPiece>().seen = true;
		GetComponentInParent<PathGenerator>().UpdateBoardContainer();
	}

	IEnumerator Destroy() {
		yield return new WaitForSeconds(killTime);
		Destroy(gameObject);
	}
}
