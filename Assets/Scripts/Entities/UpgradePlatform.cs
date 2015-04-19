using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class UpgradePlatform : MonoBehaviour {
	// Properties
	public GameObject upgradeCanvasPrefab;
	[HideInInspector] public GameObject currentUpgradeCanvasObject;
	
	public GameObject entityForUpgrade; // Upgradeable GameObjects have a child GameObject which is used for the upgrade
	public GameObject defaultEntityUpgradePrefab;

	private Animator _animator;
	public Animator animator {
		get {
			if (!_animator) {
				_animator = gameObject.GetComponent<Animator>();
			}

			return _animator;
		}
	}

	// Mutators
	public void Upgrade(GameObject prefab) {
		GameObject upgradedObject = Instantiate(prefab, entityForUpgrade.transform.position, Quaternion.identity) as GameObject;
		upgradedObject.transform.parent = upgradedObject.transform;
		
		Destroy(entityForUpgrade);
		entityForUpgrade = upgradedObject;
		
		Destroy(currentUpgradeCanvasObject);
	}

	// Responders
	public void OnMouseDown() {
		Debug.Log("click");
		GameObject[] canvases = GameObject.FindGameObjectsWithTag("Canvas");
		foreach (GameObject canvas in canvases) {
			Destroy(canvas);
		}

		if (entityForUpgrade.tag == "Tower") {
			if (currentUpgradeCanvasObject) {
				Destroy(currentUpgradeCanvasObject);
				return;
			}

			Vector2 position = gameObject.transform.position;
			currentUpgradeCanvasObject = Instantiate(upgradeCanvasPrefab, position, Quaternion.identity) as GameObject;
			currentUpgradeCanvasObject.GetComponent<UpgradeHUD>().upgradableObject = gameObject;
		} else {
			animator.SetTrigger("OpenPlatform"); // Default to basic upgrade
		}
	}

	public void OnFinishUpgrade() {
		if (entityForUpgrade.tag == "TowerNone") { // Force basic upgrade
			GameObject upgradedObject = Instantiate(defaultEntityUpgradePrefab, entityForUpgrade.transform.position, Quaternion.identity) as GameObject;
			upgradedObject.transform.parent = upgradedObject.transform;
			
			Destroy(entityForUpgrade);
			entityForUpgrade = upgradedObject;
		}
	}
}