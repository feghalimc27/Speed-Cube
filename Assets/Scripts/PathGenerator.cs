using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour {

    public GameObject ground, wall, coin;
	public int maxRenderSize = 150;

    private BoardPiece[] board;
    private Coin[] coins;

	private Coroutine building = null;

	private Vector2 buildPoint;

	private GameObject lastPart = null;

	private int partNumber = 0;

	// Use this for initialization
	void Start () {
		buildPoint = transform.position;
        UpdateBoardContainer();
    }
	
	// Update is called once per frame
	void Update () {
		if (building == null && board.Length < maxRenderSize) {
			building = StartCoroutine("BuildGround");
		}
		else {

		}
        UpdateBoardContainer();
	}

	IEnumerator BuildGround() {
		int length = Random.Range(10, 30);
		int holeChance = 0;
		bool holes = false;
        bool coined = false;
		Color color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));

		if (lastPart) {
			if (lastPart.GetComponent<BoardPiece>().type != BoardPiece.PieceType.ground) {
				lastPart = CreateParentPiece();
				lastPart.GetComponent<BoardPiece>().type = BoardPiece.PieceType.ground;
				lastPart.tag = "Ground";
				lastPart.layer = 8;
			}
		}
		else {
			lastPart = CreateParentPiece();
			lastPart.GetComponent<BoardPiece>().type = BoardPiece.PieceType.ground;
			lastPart.tag = "Ground";
			lastPart.layer = 8;
		}

		partNumber++;

		if (partNumber > 10) {
			holeChance = Random.Range(0, partNumber);

			if (holeChance > 7) {
				holes = true;
			}
		}

		for (int i = 0; i < length; ++i) {
			if (holes) {
				int holeRemoval = Random.Range(4, 9);
				buildPoint.x += holeRemoval;
				holes = false;
			}
			var block = Instantiate(ground);
			block.GetComponent<SpriteRenderer>().color = color;
			block.transform.position = buildPoint;
			block.name = "Block @" + block.transform.position + " " + i;
			block.transform.SetParent(lastPart.transform);

            int coinChance = Random.Range(0, 1000);

            if (coinChance >= 996 && !coined && partNumber > 2) {
                var newCoin = Instantiate(coin);
                newCoin.transform.position = buildPoint + new Vector2(0, 2);
                newCoin.name = "Coin @" + newCoin.transform.position + " " + coins.Length + 1;
                newCoin.transform.SetParent(transform);
                coined = true;
            }

			buildPoint.x++;
			yield return null;
		}

		StopCoroutine(building);
		building = null;
	}

    void UpdateBoardContainer() {
        // Call when adding/removing a piece
        board = GetComponentsInChildren<BoardPiece>();
        coins = GetComponentsInChildren<Coin>();
    }

	GameObject CreateParentPiece() {
		var part = Instantiate(new GameObject());

		part.AddComponent<BoardPiece>();
		part.AddComponent<CompositeCollider2D>();
		part.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		part.transform.position = buildPoint;
		part.transform.parent = transform;
		part.name = "Part " + partNumber;

		return part;
	}
}
