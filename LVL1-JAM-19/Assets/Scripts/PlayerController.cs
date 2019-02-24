using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Formation {
	Blob,
	Katamari
}

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	[Header("main")]
	[Range(1, 2)]
	public int playerNumber = 1;
	public Formation formation = Formation.Blob;

	public List<SpriteRenderer> tintedRendereres;

	[Header("movement")]
	public float speed = 1;
	//TODO if we don't use analog stricks, this will be better than rely on input settings
	//public float accelleration = 1;
	//public AnimationCurve accellerationCurve;

	public int childrenCount = 0;

    [SerializeField] private UnityEvent _OnMoveTrigger;
    [SerializeField] private UnityEvent _OnStopTrigger;

    public UnityEvent winEvent, loseEvent, drawEvent;

    private string horAxis;
	private string verAxis;

	private Vector3 direction;

	private Rigidbody2D body;
	private ChildrenComponent childrenContainer;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
		childrenContainer = GetComponentInChildren<ChildrenComponent>();

		horAxis = $"Horizontal_{playerNumber}";
		verAxis = $"Vertical_{playerNumber}";
	}

	public Vector3 vel;
	// Update is called once per frame
	void Update()
    {
        if (GameManager.getManager().getGameState() != GameState.Playing) {
            return;
        }
		float xVel = Input.GetAxis(horAxis) * speed;
		float yVel = Input.GetAxis(verAxis) * speed;
		vel = new Vector2(xVel, yVel).normalized;

		body.velocity = vel * speed;

		
		if (vel.magnitude > 0) {
			if (formation == Formation.Blob) {
				float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
				Quaternion targetDir = Quaternion.AngleAxis(angle, Vector3.forward);
				childrenContainer.transform.rotation = Quaternion.RotateTowards(targetDir, childrenContainer.transform.rotation, 0);
			}
			else
				childrenContainer.transform.rotation = Quaternion.identity;

			_OnMoveTrigger.Invoke();
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

    public void playerIsWinner() {
        body.velocity = Vector2.zero;
        if(winEvent != null) {
            winEvent.Invoke();
        }
    }

    public void playerIsLoser() {
        body.velocity = Vector2.zero;
        if (loseEvent != null) {
            loseEvent.Invoke();
        }
    }

    public void playerDrawn() {
        body.velocity = Vector2.zero;
        if (drawEvent != null) {
            drawEvent.Invoke();
        }
    }
}
