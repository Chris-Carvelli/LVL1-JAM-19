using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Kill_ObsEffect : MonoBehaviour {
	public UnityEvent onCOllision;

	private void OnCollisionEnter2D(Collision2D collision) {
		killController(collision.gameObject.GetComponent<NPCBlobConroller>());
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		killController(collision.gameObject.GetComponent<NPCBlobConroller>());
	}

	private void killController(NPCBlobConroller otherController) {
		if (otherController != null) {
			otherController.kill();
		}
	}
}

// TODO create Stun_ObsEffect that despown and immediately respawn a new one with originalPrefab