using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class NPCAloneController : MonoBehaviour
{
	private BoxCollider2D coll;
    // Start is called before the first frame update
    void Start()
    {
		coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.layer == LayerMask.NameToLayer("team_1") ||
			collision.gameObject.layer == LayerMask.NameToLayer("team_2")) {
			int team = collision.gameObject.layer == LayerMask.NameToLayer("team_1") ? 0 : 1;

			NPCBlobConroller controller = GetComponent<NPCBlobConroller>();
			controller.enabled = true;

			NPCBlobConroller other = collision.gameObject.GetComponent<NPCBlobConroller>();

			controller.targetPc = other != null ? other.targetPc : collision.GetComponent<PlayerController>();
			transform.SetParent(controller.targetPc.getTargetTransform());

			controller.GetComponent<SpriteRenderer>().color = GameManager.getManager().players[team].teamColor;

			enabled = false;
		}
	}
}
