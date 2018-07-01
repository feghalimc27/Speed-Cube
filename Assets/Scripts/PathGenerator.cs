using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour {

    public GameObject ground, wall, coin;

    private BoardPiece[] board;
    private Coin[] coins;

	// Use this for initialization
	void Start () {
        UpdateBoardContainer();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateBoardContainer();
	}

    void UpdateBoardContainer() {
        // Call when adding/removing a piece
        board = GetComponentsInChildren<BoardPiece>();
        coins = GetComponentsInChildren<Coin>();
    }
}
