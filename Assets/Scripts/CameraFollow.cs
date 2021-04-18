using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public float dampening = 0.15f;
    public float offset = 5;
    public Transform target;

    private Vector3 velocity = Vector3.zero;
	private Vector3 targetPosition;
	private bool wallFollow = false;
	private float speedOffsetChange = 0f;

    void FixedUpdate() {
		VelocityOffset();
		CalculatePosition();

		WallFollow();

        if (target) {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(targetPosition);
            Vector3 delta = targetPosition - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampening);
        }
    }

	void CalculatePosition() {
		if (target.GetComponent<Player>().GetDirection()) {
			targetPosition = target.position + new Vector3(1, 0, 0);
		}
		else {
			targetPosition = target.position + new Vector3(offset + speedOffsetChange, 0, 0);
		}
	}

	public void CalculateSpeedOffset() {
		float maxSpeed = target.GetComponent<Player>().attributes.speed;
		offset =  4 + (maxSpeed / 20);
	}

	void VelocityOffset() {
		speedOffsetChange = target.GetComponent<Player>().GetVelocity().x / ((offset - 4) * 20);
	}

	void WallFollow() {
		if (target.GetComponent<Player>().IsOnWall() && !wallFollow) {
			wallFollow = true;
		}
		if (wallFollow && target.GetComponent<Player>().IsGrounded()) {
			wallFollow = false;
		}

		if (wallFollow) {
			targetPosition = targetPosition + new Vector3(0, 1, 0);
		}
	}
}
