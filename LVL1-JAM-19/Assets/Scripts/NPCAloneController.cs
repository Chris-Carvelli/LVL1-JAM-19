using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class NPCAloneController : MonoBehaviour
{
	[SerializeField] private UnityEvent OnJoinsBlob;
	[SerializeField] private UnityEvent OnChangesBlob;

	private Collider2D coll;
	private NPCBlobConroller controller;
	// Start is called before the first frame update
	void Start()
    {
		coll = GetComponent<Collider2D>();
		controller = GetComponent<NPCBlobConroller>();

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D collision) {
		if (gameObject.layer == LayerMask.NameToLayer("Npc") &&
			(collision.gameObject.layer == LayerMask.NameToLayer("team_1") || collision.gameObject.layer == LayerMask.NameToLayer("team_2"))) {
			OnJoinsBlob.Invoke();

			int team = collision.gameObject.layer == LayerMask.NameToLayer("team_1") ? 0 : 1;
			gameObject.layer = collision.gameObject.layer;

			controller.enabled = true;

			NPCBlobConroller other = collision.gameObject.GetComponent<NPCBlobConroller>();

			controller.targetPc = other != null ? other.targetPc : collision.gameObject.GetComponent<PlayerController>();

			//FIXME sometimes happens, unclear consequences
			if (controller.targetPc == null)
				return;
			transform.SetParent(controller.targetPc.getTargetTransform());
			controller.i = ++controller.targetPc.childrenCount;

			controller.glueOffset = transform.position - controller.targetPc.transform.position;
			controller.team = team;

			controller.setTint(GameManager.getManager().players[team].teamColor);

			enabled = false;
		}
	}
}
