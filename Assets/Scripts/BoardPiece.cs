using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoardPiece : MonoBehaviour {
    public enum PieceType { wall, ground }

    public PieceType type;

	public bool seen = false;

    public static float killTime = 2f;

	private void OnBecameVisible() {
		seen = true;
		CancelInvoke();
	}

	private void OnBecameInvisible() {
		if (seen) {
			Invoke("Destroy", killTime);
		}
	}

	void Destroy() {
		Destroy(gameObject);
	}
}
