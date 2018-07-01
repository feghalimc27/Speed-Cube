using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoardPiece : MonoBehaviour {
    public enum PieceType { wall, ground }

    public PieceType type;
}
