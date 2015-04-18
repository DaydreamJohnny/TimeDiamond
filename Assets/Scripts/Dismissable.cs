using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Dismissable : MonoBehaviour {
	// Responders
	void OnMouseDown() {
		GameObject[] canvases = GameObject.FindGameObjectsWithTag("Canvas");
		foreach (GameObject canvas in canvases) {
			Destroy(canvas);
		}
	}
}
