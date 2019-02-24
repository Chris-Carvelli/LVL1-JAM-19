using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SingleActivation_ObsBehaviour : MonoBehaviour {
	public float delay = 0;
	public UnityEvent onCollision;
	public UnityEvent onTimerExpires;

	private void OnCollisionEnter2D(Collision2D collision) {
		StartCoroutine(handleCollision());
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		StartCoroutine(handleCollision());
	}

	private IEnumerator handleCollision() {
		onCollision.Invoke();
		yield return new WaitForSeconds(delay);
		onTimerExpires.Invoke();
		Destroy(gameObject);
	}
}
