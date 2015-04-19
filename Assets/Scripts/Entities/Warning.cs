using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Warning : MonoBehaviour {
	// Properties
	private Collider2D _collider;
	new public Collider2D collider {
		get {
			if (!_collider) {
				_collider = gameObject.GetComponent<Collider2D>();
			}
			
			return _collider;
		}
	}

	private int numberOfCollisions = 0;

	// Collisions
	void OnTriggerEnter2D(Collider2D coll) {
		if (numberOfCollisions > 0) { // Only trigger warning for first entry.
			return;
		}

		TargetWalker targetWalker = coll.gameObject.GetComponent<TargetWalker>();
		if (targetWalker) {
			int arrivalTime = (int)((collider.bounds.extents.x / 2.0) / targetWalker.m_speed);
			TriggerWarning(arrivalTime);

			numberOfCollisions++;
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		numberOfCollisions--;
	}

	// Warners
	void TriggerWarning(int arrivalTime) {
		Debug.Log(arrivalTime);
	}
}
