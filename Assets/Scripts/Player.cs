﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public PlayerAttributes attributes;

	[HideInInspector]
	public int score = 0;

	[SerializeField]
	private int scoreIncrement = 10;

	public float terminalVelocity = 80;

	private bool grounded = false, onWall = false, direction = false, scoring = true;

    private Rigidbody2D rb;
    private Collider2D collisionBox;
	private SpeedMultiplier speedMultiplier;

	[SerializeField]
	private List<Vector2> velocityList = new List<Vector2>();

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        collisionBox = GetComponent<Collider2D>();
		speedMultiplier = GetComponent<SpeedMultiplier>();
		attributes = Instantiate<PlayerAttributes>(attributes);
	}
	
	// Update is called once per frame
	void Update () {
		KillOnFreeFall();
	}

	void KillOnFreeFall() {
		if (rb.velocity.y < -terminalVelocity) {
			scoring = false;
			FindObjectOfType<PauseMenuController>().GameOver();
			if (score > LocalData.highScore) {
				LocalData.highScore = score;
				FindObjectOfType<LocalData>().SaveData();
			}
		}
	}

    void FixedUpdate() {
		LogVelocity();
        DetectCollision();

        ApplyMotion();
        ApplyGravity();
		IncrementScore();
    }

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Objective") {
			CoinCollision();

			Destroy(col.gameObject);
		}
	}

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.layer == 8) {
            if (col.gameObject.tag == "Wall") {
                Vector2 relativePosition = col.transform.position - transform.position;
                float transferMagnitude = velocityList[1].magnitude;

                rb.velocity = new Vector2(0, transferMagnitude * attributes.friction);
                // If collision is on the side on wall, stop moving in x
                if (Mathf.Abs(Mathf.Sign(relativePosition.x)) == 1) {
                    onWall = true;
                }
            }
            if (col.gameObject.tag == "Ground") {
                Vector2 relativePosition = col.transform.position - transform.position;
                if (Mathf.Sign(relativePosition.y) == -1) {
                    grounded = true;
                }
            }
        }
    }

    //void OnCollisionStay2D(Collision2D col) {
    //       // Environmental Collision
    //       if (col.gameObject.layer == 8) {
    //           Vector2 relativePosition = col.transform.position - transform.position;

    //		// If below, ground, return grounded
    //		if (col.gameObject.tag == "Ground") {
    //			if (Mathf.Sign(relativePosition.y) == -1) {
    //				grounded = true;
    //			}
    //			else if (Mathf.Sign(relativePosition.y) != -1) {
    //				grounded = false;
    //			}
    //		}

    //		// If on the side and not on the ground, on wall; allow walljump
    //		if (col.gameObject.tag == "Wall") {
    //			if (!grounded && Mathf.Sign(relativePosition.x) == 1) {
    //				onWall = true;
    //				direction = true;
    //			}
    //			else if (!grounded && Mathf.Sign(relativePosition.x) == -1) {
    //				onWall = true;
    //				direction = false;
    //			}
    //			else {
    //				onWall = false;
    //			}
    //		}
    //       }
    //   }

    //   void OnCollisionExit2D(Collision2D col) {
    //       if (col.gameObject.layer == 8) {
    //           Vector2 relativePosition = col.transform.position - transform.position;

    //		if (col.gameObject.tag == "Ground") {
    //			if (Mathf.Sign(relativePosition.y) == -1) {
    //				grounded = false;
    //			}
    //		}

    //		if (col.gameObject.tag == "Wall") {
    //			if (!grounded && Mathf.Sign(relativePosition.x) == 1) {
    //				onWall = false;
    //			}
    //			else if (!grounded && Mathf.Sign(relativePosition.x) == -1) {
    //				onWall = false;
    //			}
    //		}
    //       }
    //   }

    void DetectCollision() {
        float edgeLength = collisionBox.bounds.size.x / 2 + 0.3f;

		Vector2 nextPosition = transform.position + new Vector3(rb.velocity.x * Time.deltaTime, rb.velocity.y * Time.deltaTime, 0);
		Vector2 boxCastBounds = collisionBox.bounds.size;
		Vector2 sideBoxBounds = new Vector2(edgeLength, edgeLength);

        //RaycastHit2D groundCast = Physics2D.Raycast(transform.position, Vector2.down, edgeLength);
		RaycastHit2D groundCast = Physics2D.BoxCast(nextPosition, boxCastBounds, 0, Vector2.down, edgeLength);
		RaycastHit2D wallCastRight = Physics2D.Raycast(nextPosition, Vector2.right, edgeLength);
		RaycastHit2D wallCastLeft = Physics2D.Raycast(nextPosition, Vector2.left, edgeLength);

        if (groundCast) {
            grounded = groundCast.transform.gameObject.tag == "Ground";
        }
        else {
            grounded = false;
        }
        if (wallCastRight) {
			if (!onWall && wallCastRight.transform.gameObject.tag == "Wall" && !grounded) {
				float transferMagnitude = velocityList[1].magnitude;

				rb.velocity = new Vector2(rb.velocity.x, transferMagnitude * attributes.friction);
			}
            onWall = wallCastRight.transform.gameObject.tag == "Wall";
            if (onWall) {
                direction = true;
            }
        }
        else if (wallCastLeft) {
			if (!onWall && wallCastLeft.transform.gameObject.tag == "Wall" && !grounded) {
				float transferMagnitude = velocityList[1].magnitude;

				rb.velocity = new Vector2(rb.velocity.x, transferMagnitude * attributes.friction);
			}
            onWall = wallCastLeft.transform.gameObject.tag == "Wall";
            if (onWall) {
                direction = false;
            }
        }
        else {
            onWall = false;
        }

		Debug.DrawRay(nextPosition, Vector2.left * edgeLength, Color.red);
		Debug.DrawRay(nextPosition, Vector2.right * edgeLength, Color.red);
		Debug.DrawRay(nextPosition, Vector2.down * edgeLength, Color.red);
    }

    void DebugSpeedIncrease() {
		if (Input.GetButtonDown("Fire1")) {
			attributes.speed = speedMultiplier.IncreaseSpeed(attributes);
			ScaleAdjustment();
		}

	}

	void LogVelocity() {
		if (velocityList.Count < 2) {
			velocityList.Add(rb.velocity);
		}
		else {
			velocityList.RemoveAt(0);
			velocityList.Add(rb.velocity);
		}
	}

    void ApplyMotion() {
		if (!direction && !onWall) {
            if (rb.velocity.x <= attributes.speed) {
                rb.velocity = new Vector2(rb.velocity.x + attributes.acceleration * Time.deltaTime, rb.velocity.y);
            }
            else {
                rb.velocity = new Vector2(attributes.speed, rb.velocity.y);
            }
        }
		else if (direction && !onWall) {
            if (rb.velocity.x >= -attributes.speed) {
                rb.velocity = new Vector2(rb.velocity.x - attributes.acceleration * Time.deltaTime, rb.velocity.y);
            }
            else {
                rb.velocity = new Vector2(-attributes.speed, rb.velocity.y);
            }
        } 

		Jump();
    }

	void Jump() {
		if (Input.GetButtonDown("Jump") && grounded) {
			rb.velocity = new Vector2(rb.velocity.x, attributes.jumpStrength);
		}

		if (Input.GetButtonDown("Jump") && onWall) {
            if (rb.velocity.y < 0) {
                rb.velocity = new Vector2(0, 0);
            }

			switch (direction) {
				case true:
					rb.velocity = new Vector2(Mathf.Sin(60 * Mathf.Deg2Rad) * -attributes.jumpStrength, rb.velocity.y + attributes.jumpStrength);
					break;
				case false:
					rb.velocity = new Vector2(Mathf.Sin(60 * Mathf.Deg2Rad) * attributes.jumpStrength, rb.velocity.y + attributes.jumpStrength);
					break;
			}
		}
	}

    void FlipSpeed() {
        rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
    }

    void ApplyGravity() {
        if (!grounded) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - attributes.gravity * Time.deltaTime);
        }
    }

    void ApplyFriction() {

    }

	void CoinCollision() {
		score += Coin.score;
		scoreIncrement += 1;
		attributes.speed = speedMultiplier.IncreaseSpeed(attributes);
		if (BoardPiece.killTime > 1.2f) {
			BoardPiece.killTime -= 0.1f;
		}
		ScaleAdjustment();
	}

	void IncrementScore() {
		if (scoring) {
			score += scoreIncrement;
		}
	}

	void ScaleAdjustment() {
		//float scaleFactor = 2.0f - speedMultiplier.speedMultipliter;

		//Vector2 spriteScale = transform.localScale;

		//transform.localScale = spriteScale * scaleFactor;

		StartCoroutine("SmoothScale");
	}

	IEnumerator SmoothScale() {
		float scaleFactor = 2.0f - speedMultiplier.speedMultipliter;

		GetComponent<SpriteRenderer>().size *= scaleFactor;

		Vector2 finalSize = GetComponent<SpriteRenderer>().size;

		while (transform.localScale.x - Vector2.Lerp(transform.localScale, GetComponent<SpriteRenderer>().size, scaleFactor * 2 * Time.deltaTime).x > 0.0001f) {
			transform.localScale = Vector2.Lerp(transform.localScale, GetComponent<SpriteRenderer>().size, scaleFactor * 2 * Time.deltaTime);
			yield return null;
		}

		StopCoroutine("SmoothScale");
	}

    void DebugInfo() {
        Debug.Log("Velocity Vector: " + rb.velocity + " Grounded?: " + grounded);
    }

    public bool IsGrounded() {
        return grounded;
    }

    public bool IsOnWall() {
        return onWall;
    }

    public bool GetDirection() {
        return direction;
    }

	public Vector2 GetVelocity() {
		return rb.velocity;
	}
}
