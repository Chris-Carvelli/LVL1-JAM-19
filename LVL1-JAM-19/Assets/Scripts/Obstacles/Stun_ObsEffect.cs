using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun_ObsEffect : MonoBehaviour {
	private void OnCollisionEnter2D(Collision2D collision) {
		stunController(collision.gameObject.GetComponent<NPCBlobConroller>());
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		stunController(collision.gameObject.GetComponent<NPCBlobConroller>());
	}

	private void stunController(NPCBlobConroller otherController) {
		if (otherController != null) {
			otherController.stun();
		}
	}
}

// TODO create Stun_ObsEffect that despown and immediately respawn a new one with originalPrefab