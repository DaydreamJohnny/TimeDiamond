using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Damaging : MonoBehaviour {
	// Properties
	public int damage = 0;

	// Collisions
	void OnCollisionEnter2D(Collision2D coll) {
		Health health = coll.gameObject.GetComponent<Health>();
		if (health) {
			health.TakeDamage(damage);
		}
	}
}
