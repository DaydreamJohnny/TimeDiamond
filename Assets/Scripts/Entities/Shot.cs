using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Shot : MonoBehaviour {
	// Properties
	public int speed = 1;
	public int damage = 1;

	[HideInInspector] public GameObject targetObject;

	// Lifecycle
	void Update() {
		gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, targetObject.transform.position, speed * Time.deltaTime);
	}

	// Collisions
	void OnTriggerEnter2D(Collider2D coll) {
		Health health = coll.gameObject.GetComponent<Health>();
		Debug.Log (coll.gameObject);
		if (health) {
			Debug.Log("tryin");
			health.TakeDamage(damage);
		}

		Destroy(gameObject, 0.25f);
	}
}
