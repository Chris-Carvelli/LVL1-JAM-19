using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	[Header("main")]
	[Range(1, 2)]
	public int playerNumber = 1;
	public Transform diretcionTarget;

	[Header("movement")]
	public float speed = 1;
	//TODO if we don't use analog stricks, this will be better than rely on input settings
	//public float accelleration = 1;
	//public AnimationCurve accellerationCurve;

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

	[Header("Debug")]
	public float axis;
	public float vel;
	public float Asin;
	public float velCos;

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

			childrenContainer.transform.rotation = Quaternion.RotateTowards(targetDir, childrenContainer.transform.rotation, 0);
		}
		//float xCoord = transform.position.x + Input.GetAxis(horAxis) * speed;
		//float yCoord = transform.position.y + Input.GetAxis(verAxis) * speed;

		//Vector3 deltaV = new Vector3(
		//	Mathf.Clamp(xCoord, xMargins.x, xMargins.y),
		//	Mathf.Clamp(yCoord, yMargins.x, yMargins.y),
		//	0
		//	);

		//transform.position = deltaV;
	}

	public Transform getTargetTransform() {
		return childrenContainer.transform;
	}
}
