using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Upgradable : MonoBehaviour {
	// Properties
	public GameObject upgradeCanvasPrefab;
	[HideInInspector] public GameObject currentUpgradeCanvasObject;

	// Responders
	void OnMouseDown() {
		GameObject[] canvases = GameObject.FindGameObjectsWithTag("Canvas");
		foreach (GameObject canvas in canvases) {
			Destroy(canvas);
		}

		if (currentUpgradeCanvasObject) {
			Destroy(currentUpgradeCanvasObject);
			return;
		}

		Vector2 position = gameObject.transform.position;
		currentUpgradeCanvasObject = Instantiate(upgradeCanvasPrefab, position, Quaternion.identity) as GameObject;
		currentUpgradeCanvasObject.GetComponent<Upgrader>().upgradableObject = gameObject;
	}

	public void FinishMouseDown() {
		Destroy(currentUpgradeCanvasObject);
	}
}