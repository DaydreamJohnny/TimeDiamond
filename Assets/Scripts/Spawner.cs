using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	// Properties
	public int rate = 5;
	public GameObject entityPrefab;
	public FollowPath followPath;

	private float nextSpawnTime = 0;

	// Lifecycle
	void Update() {
		if (Time.time > nextSpawnTime) {
			Spawn();
		}
	}

	// Spawners
	void Spawn() {
		Vector2 position = gameObject.transform.position;
		GameObject spawnedObject = Instantiate(entityPrefab, position, Quaternion.identity) as GameObject;
		spawnedObject.GetComponent<TargetWalker>().m_pathController = followPath;

		nextSpawnTime = Time.time + rate;
	}
}
