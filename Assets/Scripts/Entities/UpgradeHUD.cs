using UnityEngine;
using System.Collections;

public class UpgradeHUD : MonoBehaviour {
	// Properties
	[HideInInspector] public GameObject upgradableObject;

	// Responders
	public void OnButtonPress(GameObject upgradePrefab) {
		UpgradePlatform upgradable = upgradableObject.GetComponent<UpgradePlatform>();
		upgradable.Upgrade(upgradePrefab);
	}
}