using UnityEngine;
using System.Collections;

public class Upgrader : MonoBehaviour {
	// Properties
	[HideInInspector] public GameObject upgradableObject;

	// Responders
	public void OnButtonPress(GameObject upgradePrefab) {
		Debug.Log("Upgrade");
		upgradableObject.GetComponent<Upgradable>().FinishMouseDown();
	}

	public void OnButtonPress() {
		Debug.Log("Turn off");
		upgradableObject.GetComponent<Upgradable>().FinishMouseDown();
	}
}