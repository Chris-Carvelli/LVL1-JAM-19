﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	[Header("main")]
	[Range(1, 2)]
	public int playerNumber = 1;

	public List<SpriteRenderer> tintedRendereres;

	[Header("movement")]
	public float speed = 1;
	//TODO if we don't use analog stricks, this will be better than rely on input settings
	//public float accelleration = 1;
	//public AnimationCurve accellerationCurve;

	public int childrenCount = 0;

    [SerializeField] private UnityEvent _OnMoveTrigger;
    [SerializeField] private UnityEvent _OnStopTrigger;

    private string horAxis;
	private string verAxis;

	private Vector3 direction;

	private Rigidbody2D body;
	private ChildrenComponent childrenContainer;

	// Start is called before the first frame update
	void Start()
    {
		body = GetComponent<Rigidbody2D>();
		childrenContainer = GetComponentInChildren<ChildrenComponent>();

		horAxis = $"Horizontal_{playerNumber}";
		verAxis = $"Vertical_{playerNumber}";
	}

	// Update is called once per frame
	void Update()
    {
		float xVel = Input.GetAxis(horAxis) * speed;
		float yVel = Input.GetAxis(verAxis) * speed;
		Vector3 vel = new Vector2(xVel, yVel);

		body.velocity = vel;

		if (vel.magnitude > 0) {
			float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
			Quaternion targetDir = Quaternion.AngleAxis(angle, Vector3.forward);
            _OnMoveTrigger.Invoke();
			childrenContainer.transform.rotation = Quaternion.RotateTowards(targetDir, childrenContainer.transform.rotation, 0);
		}

        if (vel.magnitude <= 0)
        {
            _OnStopTrigger.Invoke();
        }
	}

	public Transform getTargetTransform() {
		return childrenContainer.transform;
	}

	public void setTint(Color color) {
		foreach (SpriteRenderer sp in tintedRendereres)
			sp.color = color;
	}
}
