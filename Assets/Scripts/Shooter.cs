using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Shooter : MonoBehaviour {
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

	private GameObject targetObject;

	public int fireRate = 1;
	private float nextFireTime = 0;

	public GameObject shotPrefab;

	// Lifecycle
	void Update() {
		if (targetObject && Time.time > nextFireTime) {
			Shoot(targetObject);
		}
	}

	// Collisions
	void OnTriggerEnter2D(Collider2D coll) {
		if (!targetObject) {
			if (coll.gameObject.tag == "Enemy") { // Only fire upon enemies
				targetObject = coll.gameObject;
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (coll.gameObject == targetObject) {
			targetObject = null;
		}
	}

	// Shooters
	void Shoot(GameObject targetObject) {
		GameObject shotObject = Instantiate(shotPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
		shotObject.GetComponent<Shot>().targetObject = targetObject;

		nextFireTime = Time.time + fireRate;
	}
}
