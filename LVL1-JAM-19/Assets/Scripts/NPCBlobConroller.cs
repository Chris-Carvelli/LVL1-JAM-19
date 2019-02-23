using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCBlobConroller : MonoBehaviour
{
	public PlayerController targetPc;
	public int i;

	public float speed = 10;

	private Rigidbody2D body;

	private Quaternion startRot;

	//TMP
	private float radius = 1.2f;
    // Start is called before the first frame update
    void Start()
    {
		body = GetComponent<Rigidbody2D>();

		startRot = transform.rotation;
    }

	// Update is called once per frame
	void Update()
    {
		if (targetPc == null)
			return;


		Vector3 target = new Vector3(
			getXoffset(),
			getYoffset(),
			0
		);

		Quaternion rot = targetPc.getTargetTransform().localRotation * Quaternion.Euler(0, 0, 90);
		Vector3 dest = (targetPc.getTargetTransform().position + rot * target);
		body.velocity = (dest - transform.position) * speed;

		//TMP freeze rotation
		transform.rotation = startRot;
	}

	private float getXoffset() {
		float ret = 0;

		switch(i % 3) {
			case 0:
				ret = radius;
				break;
			case 1:
				ret = 0;
				break;
			case 2:
				ret = -radius;
				break;
		}

		return ret;
	}

	private float getYoffset() {
		return (((i - 1) / 3) + 1) * radius;
	}
}
