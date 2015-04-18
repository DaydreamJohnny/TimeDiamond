using UnityEngine;
using System.Collections;

public class Upgradable : MonoBehaviour {
	// Properties
	public GameObject upgradeCanvasObject;

	// Responders
	void OnMouseDown() {
		Instantiate(upgradeCanvasObject, gameObject.transform.localPosition, Quaternion.identity);
	}
}
