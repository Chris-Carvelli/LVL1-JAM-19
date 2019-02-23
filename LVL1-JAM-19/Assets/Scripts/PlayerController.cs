using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	[Header("main")]
	[Range(1, 2)]
	public int playerNumber = 1;

	[Header("movement")]
	public float speed = 1;
	//TODO if we don't use analog stricks, this will be better than rely on input settings
	//public float accelleration = 1;
	//public AnimationCurve accellerationCurve;

	private string horAxis;
	private string verAxis;

	//public Vector2 xMargins = new Vector2(-14, 14);
	//public Vector2 yMargins = new Vector2(-3, 32);

	private Rigidbody2D body;

	// Start is called before the first frame update
	void Start()
    {
		body = GetComponent<Rigidbody2D>();

		horAxis = $"Horizontal_{playerNumber}";
		verAxis = $"Vertical_{playerNumber}";
	}

	// Update is called once per frame
	void Update()
    {
		float xVel = Input.GetAxis(horAxis) * speed;
		float yVel = Input.GetAxis(verAxis) * speed;

		body.velocity = new Vector2(xVel, yVel);

		//float xCoord = transform.position.x + Input.GetAxis(horAxis) * speed;
		//float yCoord = transform.position.y + Input.GetAxis(verAxis) * speed;

		//Vector3 deltaV = new Vector3(
		//	Mathf.Clamp(xCoord, xMargins.x, xMargins.y),
		//	Mathf.Clamp(yCoord, yMargins.x, yMargins.y),
		//	0
		//	);

		//transform.position = deltaV;
	}
}
