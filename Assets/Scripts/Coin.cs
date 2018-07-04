using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    public static int score = 5000;

    public bool seen = false;

    private void OnBecameVisible() {
        seen = true;
    }

    private void OnBecameInvisible() {
        if (seen) {
            Destroy(gameObject);
        }
    }
}
