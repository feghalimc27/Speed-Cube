using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMultiplier : MonoBehaviour {
	public float speedMultipliter = 1.07f;

	public float IncreaseSpeed(PlayerAttributes attributes) {
		Debug.Log("Increased Speed");

		return attributes.speed * speedMultipliter;
	}
}