using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour {
    bool grounded;

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Ground") {
            grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Ground") {
            grounded = false;
        }
    }

    void Update() {
        Debug.Log(grounded);
    }

    public bool IsGrounded() {
        return grounded;
    }
}
