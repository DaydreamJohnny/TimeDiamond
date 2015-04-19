using UnityEngine;
using System.Collections;

public class Slower : MonoBehaviour {
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

	public float speedCoefficient = 0.40f;

	// Collisions
	void OnTriggerEnter2D(Collider2D coll) {
		TargetWalker targetWalker = coll.gameObject.GetComponent<TargetWalker>();
		if (targetWalker) {
			targetWalker.td_speedCoef *= speedCoefficient;
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		TargetWalker targetWalker = coll.gameObject.GetComponent<TargetWalker>();
		if (targetWalker) {
			targetWalker.td_speedCoef /= speedCoefficient;
		}
	}
}
