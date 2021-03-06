using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour {

    public GameObject ground, wall, coin;
	public PhysicsMaterial2D groundMaterial;
	public int maxRenderSize = 150;
    public int coinAdditive = 4;
	[Range(1, 10), Tooltip("Lower is more frequent, likelyhood out of 10")]
	public int wallFrequency = 8;

	public int minGroundLength = 20;
	public int maxGroundLength = 40;
	public int minWallLength = 40;
	public int maxWallLength = 90;
	public int minHoleSize = 4;
	public int maxHoleSize = 7;
    public float hueIncrementGap = 0.01f;

    private BoardPiece[] board;
    private Coin[] coins;

	private Color color;
	private float h = 2, s = 1, v = 1;

	private Coroutine building = null;

	private Vector2 buildPoint;
	[SerializeField]
	private Vector2 wallOffset = new Vector2(-5, 5);

	private GameObject lastPart = null;

	private int partNumber = 0;

	private bool lastCoined = false;

	// Use this for initialization
	void Start () {
		buildPoint = transform.position;
        UpdateBoardContainer();
		color = CreateColor();
		BoardPiece.killTime = 2.0f;

		//somehow makes framerate worse?
		// Application.targetFrameRate = 60;
    }
	
	// Update is called once per frame
	void Update () {
		if (building == null && board.Length < maxRenderSize) {
			if (lastPart) {
				if (partNumber < 3 || lastPart.GetComponent<BoardPiece>().type == BoardPiece.PieceType.wall) {
					building = StartCoroutine("BuildGround");
				}
				else {
					int choose = Random.Range(0, 10);

					if (choose >= wallFrequency) {
						building = StartCoroutine("BuildWall");
						UpdateBoardContainer();
					}
					else {
						building = StartCoroutine("BuildGround");
						UpdateBoardContainer();
					}
				}
			}
			else {
				building = StartCoroutine("BuildGround");
				UpdateBoardContainer();
			}
		}
		else {

		}
		Debug.Log(board.Length);
	}

	IEnumerator BuildGround() {
		int length = Random.Range(minGroundLength, maxGroundLength);
		int holeChance = 0;
		bool holes = false;
        bool coined = false;
		bool lastWall = false;

		if (lastPart) {
			lastWall = lastPart.tag == "Wall";

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

		if (partNumber > 10 && !lastWall && !lastCoined) {
			holeChance = Random.Range(0, partNumber);

			if (holeChance > 7) {
				holes = true;
			}
		}

		if (lastWall) {
			length = 30;
		}

		for (int i = 0; i < length; ++i) {
			if (holes) {
				int holeRemoval = Random.Range(minHoleSize, maxHoleSize);
				buildPoint.x += holeRemoval;
				holes = false;
				if (length < 30) {
					length = 30;
				}
			}

			color = CreateColor();
			var block = Instantiate(ground);
			block.GetComponent<SpriteRenderer>().color = color;
			block.transform.position = buildPoint;
			block.name = "Block @" + block.transform.position + " " + i;
			block.transform.SetParent(lastPart.transform);

            int coinChance = Random.Range(0, 1000);

            if (coinChance >= 990 && !coined && partNumber > 2) {
                var newCoin = Instantiate(coin);
                newCoin.transform.position = buildPoint + new Vector2(0, 2);
                newCoin.transform.localScale *= 0.5f;
                newCoin.name = "Coin @" + newCoin.transform.position + " " + coins.Length + 1;
                newCoin.transform.SetParent(transform);
                coined = true;
                length += coinAdditive;
            }

			buildPoint.x++;
			yield return null;
		}

		lastCoined = coined;
		StopCoroutine(building);
		building = null;
	}

	IEnumerator BuildWall() {
		int length = Random.Range(minWallLength, maxWallLength);
		bool coined = false;

		Vector2 wallPoint = buildPoint + wallOffset;


		GameObject newPart = CreateParentPiece();
		newPart.GetComponent<BoardPiece>().type = BoardPiece.PieceType.wall;
		newPart.transform.position = wallPoint;
		newPart.tag = "Wall";
		newPart.layer = 8;
		newPart.name = "Wall " + (partNumber + 1);

		if (lastPart) {
			if (lastPart.GetComponent<BoardPiece>().type != BoardPiece.PieceType.wall) {
				lastPart = CreateParentPiece();
				lastPart.GetComponent<BoardPiece>().type = BoardPiece.PieceType.wall;
				lastPart.tag = "Wall";
				lastPart.layer = 8;
				lastPart.name = "Wall " + (partNumber);
			}
		}
		else {
			lastPart = CreateParentPiece();
			lastPart.GetComponent<BoardPiece>().type = BoardPiece.PieceType.wall;
			lastPart.tag = "Wall";
			lastPart.layer = 8;
			lastPart.name = "Wall " + (partNumber);
		}

		partNumber++;

		for (int i = 0; i < length; ++i) {
			color = CreateColor();
			var block = Instantiate(wall);
			block.GetComponent<SpriteRenderer>().color = color;
			block.transform.position = buildPoint;
			block.name = "Block @" + block.transform.position + " " + i;
			block.transform.SetParent(lastPart.transform);
			var oppositeBlock = Instantiate(wall);
			oppositeBlock.GetComponent<SpriteRenderer>().color = color;
			oppositeBlock.transform.position = wallPoint;
			oppositeBlock.name = "Block @" + oppositeBlock.transform.position + " " + i;
			oppositeBlock.transform.SetParent(newPart.transform);

			int coinChance = Random.Range(0, 1000);

			if (coinChance >= 990 && !coined && partNumber > 2 && i < length - 5) {
				var newCoin = Instantiate(coin);
				newCoin.transform.position = wallPoint + new Vector2(2.5f, 0);
				newCoin.transform.localScale *= 0.5f;
				newCoin.name = "Coin @" + newCoin.transform.position + " " + coins.Length + 1;
				newCoin.transform.SetParent(transform);
				coined = true;
				length += coinAdditive;
			}

			buildPoint.y++;
			wallPoint.y++;
			yield return null;
		}

		GameObject roof = CreateParentPiece();

        roof.layer = 8;

		for (int i = 0; i < 10; ++i) {
			var block = Instantiate(ground);
			block.GetComponent<SpriteRenderer>().color = color;
			block.transform.position = wallPoint;
			block.name = "Block @" + block.transform.position + " " + i;
			block.transform.SetParent(roof.transform);

			wallPoint.x++;
			yield return null;
		}

		StopCoroutine(building);
		building = null;
	}

    public void UpdateBoardContainer() {
        // Call when adding/removing a piece
        board = GetComponentsInChildren<BoardPiece>();
        coins = GetComponentsInChildren<Coin>();
    }

	float GetRandomHue() {
		return Random.Range(0.000f, 1.000f);
	}

	void IncrementHue() {
		h += hueIncrementGap;

		if (h >= 1) {
			h = 0;
		}
	}

	Color CreateColor() {
		if (h == 2) {
			h = GetRandomHue();
		}

		IncrementHue();

		return Color.HSVToRGB(h, s, v);
	}

	GameObject CreateParentPiece() {
		var part = new GameObject();

		part.AddComponent<BoardPiece>();
		part.AddComponent<CompositeCollider2D>();
		part.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		part.GetComponent<Rigidbody2D>().sharedMaterial = groundMaterial;
		part.transform.position = buildPoint;
		part.transform.parent = transform;
		part.name = "Part " + partNumber;

		return part;
	}
}
