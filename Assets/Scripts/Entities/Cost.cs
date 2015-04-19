using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Cost : MonoBehaviour {
	// Properties
	public int cost = 1;
	
	public GameObject playerObject;
	private ResourcePool _playerResourcePool;

	private Animator _animator;
	public Animator animator {
		get {
			if (!_animator) {
				_animator = gameObject.GetComponent<Animator>();
			}
			
			return _animator;
		}
	}

	// Lifecycle
	void Awake() {
		_playerResourcePool = playerObject.GetComponent<ResourcePool>();
	}

	void Update() {
		animator.SetBool("Affordable", _playerResourcePool.availableResources >= cost);
	}
}
