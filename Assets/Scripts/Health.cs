using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	// Properties
	public int totalHealth = 1;
	public bool dies = false;

	private int _health;
	public int health {
		get {
			return _health;
		}
	}

	// Lifecycle
	void Awake() {
		_health = totalHealth;
	}

	// Mutators
	public void TakeDamage(int damage) {
		Debug.Log ("DAMAGE");

		_health -= damage;
		_health = Mathf.Max(0, _health);

		if (dies) {
			Die();
		}
	}

	public void Die() {
		if (_health == 0) {
			DestroyObject(gameObject);
		}
	}
}
