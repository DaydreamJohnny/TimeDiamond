using UnityEngine;
using System.Collections;

public class Upgrader : MonoBehaviour {
	// Properties
	[HideInInspector] public GameObject upgradableObject;

	// Responders
	public void OnButtonPress(GameObject upgradePrefab) {
		Upgradable upgradable = upgradableObject.GetComponent<Upgradable>();

		GameObject upgradedObject = Instantiate(upgradePrefab, upgradable.entityForUpgrade.transform.position, Quaternion.identity) as GameObject;
		upgradedObject.transform.parent = upgradedObject.transform;

		Destroy(upgradable.entityForUpgrade);
		upgradable.entityForUpgrade = upgradedObject;

		upgradable.FinishMouseDown();
	}
}