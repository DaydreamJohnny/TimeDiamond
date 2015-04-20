using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
public class AreaOfEffect : MonoBehaviour {
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

	private List<GameObject> _targets = new List<GameObject>();
	private List<GameObject> targets {
		get {
			_targets = _targets.Where(target => target != null).ToList();
			return _targets; 
		}

		set {
			_targets = value;
		}
	}

	public float fireRate = 1;
	private float nextFireTime = 0;
	
	public int damage = 1;
	
	// Lifecycle
	void Update() {
		if (Time.time > nextFireTime) {
			Shoot(targets);
		}
	}
	
	// Collisions
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Enemy") { // Only fire upon enemies
			targets.Add(coll.gameObject);
		}
	}
	
	void OnTriggerExit2D(Collider2D coll) {
		targets.Remove(coll.gameObject);
	}
	
	// Shooters
	void Shoot(List<GameObject> targetObjects) {
		foreach (GameObject target in targets) {
			Health health = target.GetComponent<Health>();
			if (health) {
				health.TakeDamage(damage);
			}
		}
		
		nextFireTime = Time.time + fireRate;
	}
}