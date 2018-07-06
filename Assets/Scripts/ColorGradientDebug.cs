using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGradientDebug : MonoBehaviour {
	public float minimum, maximum;

	private void Start() {
		StartCoroutine("ColorChange");
	}

	IEnumerator ColorChange() {
		while (true) {
			Color color = Color.HSVToRGB(Random.Range(minimum, maximum), 1, 1);

			GetComponent<SpriteRenderer>().color = color;
			yield return new WaitForSeconds(2);
		}
	}
}
