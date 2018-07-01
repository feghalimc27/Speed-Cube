using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public PlayerAttributes attributes;

	[HideInInspector]
	public int score = 0;

    private bool grounded = false, onWall = false, direction = false;

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
				float transferMagnitude = velocityList[1].magnitude;

				rb.velocity = new Vector2(0, transferMagnitude * attributes.friction);
			}
		}
	}

	void OnCollisionStay2D(Collision2D col) {
        // Environmental Collision
        if (col.gameObject.layer == 8) {
            Vector2 relativePosition = col.transform.position - transform.position;

			// If below, ground, return grounded
			if (col.gameObject.tag == "Ground") {
				if (Mathf.Sign(relativePosition.y) == -1) {
					grounded = true;
				}
				else if (Mathf.Sign(relativePosition.y) != -1) {
					grounded = false;
				}
			}

			// If on the side and not on the ground, on wall; allow walljump
			if (col.gameObject.tag == "Wall") {
				if (!grounded && Mathf.Sign(relativePosition.x) == 1) {
					onWall = true;
					direction = true;
				}
				else if (!grounded && Mathf.Sign(relativePosition.x) == -1) {
					onWall = true;
					direction = false;
				}
				else {
					onWall = false;
				}
			}
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.layer == 8) {
            Vector2 relativePosition = col.transform.position - transform.position;

			if (col.gameObject.tag == "Ground") {
				if (Mathf.Sign(relativePosition.y) == -1) {
					grounded = false;
				}
			}

			if (col.gameObject.tag == "Wall") {
				if (!grounded && Mathf.Sign(relativePosition.x) == 1) {
					onWall = false;
				}
				else if (!grounded && Mathf.Sign(relativePosition.x) == -1) {
					onWall = false;
				}
			}
        }
    }

    void DetectCollision() {

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
		if (grounded && Input.GetButtonDown("Jump")) {
			rb.velocity = new Vector2(rb.velocity.x, attributes.jumpStrength);
		}

		if (onWall && Input.GetButtonDown("Jump")) {
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
		attributes.speed = speedMultiplier.IncreaseSpeed(attributes);
		ScaleAdjustment();
	}

	void IncrementScore() {
		score += 10;
	}

	void ScaleAdjustment() {
		float scaleFactor = 2.0f - speedMultiplier.speedMultipliter;

		Vector2 spriteScale = transform.localScale;

		transform.localScale = spriteScale * scaleFactor;
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

	public Vector2 GetVelocity() {
		return rb.velocity;
	}
}
