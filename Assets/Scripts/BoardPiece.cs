using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoardPiece : MonoBehaviour {
    public enum PieceType { wall, ground }

    public PieceType type;

	public bool seen = false;

	private void OnBecameVisible() {
		seen = true;
	}

	private void OnBecameInvisible() {
		if (seen) {
			Destroy(gameObject);
		}
	}
}
