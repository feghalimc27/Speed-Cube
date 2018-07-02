using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    private Player player;
    private SpriteRenderer renderer;

	// Use this for initialization
	void Start () {
        player = GetComponent<Player>();
        renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        renderer.flipX = player.GetDirection();
	}
}
