using UnityEngine;
using System.Collections;

public class ResourcePool : MonoBehaviour {
	// Properties
	public int initialResources = 1;
	public int totalResources {get; private set;}
	public int availableResources {get; private set;}

	// Lifecycle
	void Awake() {
		totalResources = initialResources;
		availableResources = totalResources;
	}

	// Mutators
	public void AddResources(int newResources) {
		totalResources += newResources;
		availableResources += newResources;
	}

	public void UseResources(int usedResources) {
		availableResources -= usedResources;
	}

	public void FreeResources(int freedResources) {
		availableResources += freedResources;
	}
}
