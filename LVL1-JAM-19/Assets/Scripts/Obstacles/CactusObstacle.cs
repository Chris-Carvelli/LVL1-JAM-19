using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusObstacle : MonoBehaviour
{
	public float bumpIntensity = 100;
	private void OnCollisionEnter2D(Collision2D collision) {
		//float a = Random.Range(-45, 45);
		//a *= Mathf.Deg2Rad;
		//Vector2 dir = collision.relativeVelocity + new Vector2(Mathf.Cos(a), Mathf.Sin(a));

		//collision.rigidbody.AddForce(dir * bumpIntensity);

		//TMP
		NPCBlobConroller otherController = collision.gameObject.GetComponent<NPCBlobConroller>();

		if (otherController != null) {
			otherController.bouncing = true;
			otherController.collTime = float.MaxValue;
			//otherController.collTime = 1;
			otherController.kill();
		}
	}
}
