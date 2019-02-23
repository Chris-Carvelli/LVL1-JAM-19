using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class NPCAloneController : MonoBehaviour
{
	[SerializeField] private UnityEvent OnJoinsBlob;
	[SerializeField] private UnityEvent OnChangesBlob;

	private BoxCollider2D coll;
	private NPCBlobConroller controller;
	// Start is called before the first frame update
	void Start()
    {
		coll = GetComponent<BoxCollider2D>();
		controller = GetComponent<NPCBlobConroller>();

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision) {
		if ((collision.gameObject.layer == LayerMask.NameToLayer("team_1") ||
			collision.gameObject.layer == LayerMask.NameToLayer("team_2")) &&
			gameObject.layer != collision.gameObject.layer) {
			OnJoinsBlob.Invoke();

			int team = collision.gameObject.layer == LayerMask.NameToLayer("team_1") ? 0 : 1;
			gameObject.layer = collision.gameObject.layer;

			controller.enabled = true;
			if (controller.targetPc != null) {
				OnChangesBlob.Invoke();
				controller.targetPc.childrenCount--;
			}

			NPCBlobConroller other = collision.gameObject.GetComponent<NPCBlobConroller>();

			controller.targetPc = other != null ? other.targetPc : collision.GetComponent<PlayerController>();
			transform.SetParent(controller.targetPc.getTargetTransform());
			controller.i = ++controller.targetPc.childrenCount;

			controller.setTint(GameManager.getManager().players[team].teamColor);

			enabled = false;
		}
	}
}
