using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public PlayerAttributes attributes;

    private bool grounded = false, onWall = false, direction = false;

    private Rigidbody2D rb;
    private Collider2D collisionBox;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        collisionBox = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        DebugInfo();
	}

    void FixedUpdate() {
        DetectCollision();

        ApplyMotion();
        ApplyGravity();
    }

    void OnCollisionStay2D(Collision2D col) {
        // Environmental Collision
        if (col.gameObject.layer == 8) {
            Vector2 relativePosition = col.transform.position - transform.position;

            // If below, ground, return grounded
            if (Mathf.Sign(relativePosition.y) == -1) {
                grounded = true;
            }
            else if (Mathf.Sign(relativePosition.y) != -1) {
                grounded = false;
            }

            // If on the side and not on the ground, on wall; allow walljump
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

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.layer == 8) {
            Vector2 relativePosition = col.transform.position - transform.position;

            if (Mathf.Sign(relativePosition.y) == -1) {
                grounded = false;
            }


            if (!grounded && Mathf.Sign(relativePosition.x) == 1) {
                onWall = false;
            }
            else if (!grounded && Mathf.Sign(relativePosition.x) == -1) {
                onWall = false;
            }
        }
    }

    void DetectCollision() {

    }

    void ApplyMotion() {
        if (!direction) {
            if (rb.velocity.x <= attributes.speed) {
                rb.velocity = new Vector2(rb.velocity.x + attributes.acceleration * Time.deltaTime, rb.velocity.y);
            }
            else {
                rb.velocity = new Vector2(attributes.speed, rb.velocity.y);
            }
        }
        else {
            if (rb.velocity.x >= -attributes.speed) {
                rb.velocity = new Vector2(rb.velocity.x - attributes.acceleration * Time.deltaTime, rb.velocity.y);
            }
            else {
                rb.velocity = new Vector2(-attributes.speed, rb.velocity.y);
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

    void DebugInfo() {
        Debug.Log("Velocity Vector: " + rb.velocity + " Grounded?: " + grounded);
    }

    public bool IsGrounded() {
        return grounded;
    }

    public bool IsOnWall() {
        return onWall;
    }
}
