using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public float dampening = 0.15f;
    public float offset = 5;
    public Transform target;

    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update() {
        Vector3 targetPosition;
        if (target.GetComponent<Player>().GetDirection()) {
            targetPosition = target.position + new Vector3(1, 0, 0);
        }
        else {
            targetPosition = target.position + new Vector3(offset, 0, 0);
        }

        if (target) {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(targetPosition);
            Vector3 delta = targetPosition - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampening);
        }
    }
}
