using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Damaging : MonoBehaviour {
	// Properties
	private Collider2D _collider;
	public Collider2D collider {
		get {
			if (!_collider) {
				_collider = gameObject.GetComponent<Collider2D>();
			}

			return _collider;
		}
	}

	public int damage = 0;

	// Collisions
	void OnCollisionEnter2D(Collision2D coll) {
		Health health = coll.gameObject.GetComponent<Health>();
		if (health) {
			health.TakeDamage(damage);
		}
	}
}
