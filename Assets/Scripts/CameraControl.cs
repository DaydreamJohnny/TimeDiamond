using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	// Properties
	public float speed = 0.3f;

	// Lifecycle
	void Update() {
		Pan();
	}

	// Control
	void Pan() {
		Vector2 panVector = Vector2.zero;
			
		if (Input.GetKey(KeyCode.W)) {
			panVector += Vector2.up;
		}
		
		if (Input.GetKey(KeyCode.A)) {
			panVector -= Vector2.right;
		}
		
		if (Input.GetKey(KeyCode.S)) {
			panVector -= Vector2.up;
		}
		
		if (Input.GetKey(KeyCode.D)) {
			panVector += Vector2.right;
		}

		gameObject.transform.Translate(panVector.x * speed, panVector.y * speed, 0);
	}
}
