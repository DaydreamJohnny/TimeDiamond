using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TargetWalker))]
public class Direction : MonoBehaviour {
	// Types
	public enum Value {
		Up = 0,
		Down = 1,
		Left = 2,
		Right = 3
	}

	// Properties
	private Value _direction = Value.Down;
	public Value direction {
		get {
			return _direction;
		}

		set {
			_direction = value;
			animator.SetInteger("Direction", (int)_direction);
		}
	}

	private Animator _animator;
	public Animator animator {
		get {
			if (!_animator) {
				_animator = gameObject.GetComponent<Animator>();
			}
			
			return _animator;
		}
	}

	private TargetWalker _walker;
	public TargetWalker walker {
		get {
			if (!_walker) {
				_walker = gameObject.GetComponent<TargetWalker>();
			}
			
			return _walker;
		}
	}

	private Vector2 lastPosition = Vector2.zero;

	// Lifecycle
	void Awake() {
		direction = _direction;
	}

	void Update() {
		if (walker.m_speed == 0) {
			return;
		}

		Vector2 positionDelta = lastPosition - (Vector2)gameObject.transform.position;
		lastPosition = gameObject.transform.position;

		if (Mathf.Abs(positionDelta.x) > Mathf.Abs(positionDelta.y)) { // Moving Left/Right
			if (positionDelta.x < 0) {
				direction = Value.Right;
			} else {
				direction = Value.Left;
			}
		} else { // Moving Up/Down
			if (positionDelta.y < 0) {
				direction = Value.Up;
			} else {
				direction = Value.Down;
			}
		}
	}
}
