using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCBlobConroller : MonoBehaviour
{
	public float noiseIntensity = 0.5f;
	private float _noiseSample = 0;
	private float _randomNoiseSeedX;
	private float _randomNoiseSeedY;


	public PlayerController targetPc;
	public int team;
	public int i;

	public float speed = 10;
	public float maxSpeed = 2;

	private Rigidbody2D body;

	private Quaternion startRot;

	//for katamari formation
	public Vector3 glueOffset;

	private bool bouncing = false;
	private float collTime = 0;

	//TMP
	private float radius = 1.2f;
    // Start is called before the first frame update
    void Start()
    {
		body = GetComponent<Rigidbody2D>();

		startRot = transform.rotation;

		_randomNoiseSeedX = Random.value;
		_randomNoiseSeedY = Random.value;
	}

	// Update is called once per frame
	void Update()
    {
		if (bouncing) {
			collTime -= Time.deltaTime;

			if (collTime < 0)
				bouncing = false;
			return;
		}

		if (targetPc == null)
			return;

		switch (targetPc.formation) {
			case Formation.Blob:
				BlobFormation();
				break;
			case Formation.Katamari:
				KatamariFormation();
				break;
		}
		
	}

	private void BlobFormation() {
		Vector3 target = new Vector3(
			getXoffset(),
			getYoffset(),
			0
		);

		Quaternion rot = targetPc.getTargetTransform().localRotation * Quaternion.Euler(0, 0, 90);
		Vector3 dest = (targetPc.getTargetTransform().position + rot * target);

		dest += new Vector3(Mathf.PerlinNoise(_noiseSample, _randomNoiseSeedX) - 0.5f, Mathf.PerlinNoise(_noiseSample, _randomNoiseSeedY) - 0.5f) * noiseIntensity;
		_noiseSample += Time.deltaTime;

		body.velocity = (dest - transform.position) * speed;

		//TMP freeze rotation
		transform.rotation = startRot;
	}

	private void KatamariFormation() {
		Vector3 dest = targetPc.transform.position + glueOffset;

		dest += new Vector3(Mathf.PerlinNoise(_noiseSample, _randomNoiseSeedX) - 0.5f, Mathf.PerlinNoise(_noiseSample, _randomNoiseSeedY) - 0.5f) * noiseIntensity;
		_noiseSample += Time.deltaTime;

		body.velocity = Vector3.ClampMagnitude((dest - transform.position) * speed, maxSpeed);


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

	public void setTint(Color color) {
		foreach (SpriteRenderer sp in GetComponentsInChildren<SpriteRenderer>())
			sp.color = color;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if ((gameObject.layer  == LayerMask.NameToLayer("team_1") &&
			collision.gameObject.layer == LayerMask.NameToLayer("team_2")) ||
			(gameObject.layer == LayerMask.NameToLayer("team_2") &&
			collision.gameObject.layer == LayerMask.NameToLayer("team_1"))) {
			float a = Random.Range(-45, 45);
			a *= Mathf.Deg2Rad;
			Vector2 dir = collision.relativeVelocity + new Vector2(Mathf.Cos(a), Mathf.Sin(a));

			body.AddForce(dir * 10);

			bouncing = true;
			collTime = 0.0f;
		}
	}
}

