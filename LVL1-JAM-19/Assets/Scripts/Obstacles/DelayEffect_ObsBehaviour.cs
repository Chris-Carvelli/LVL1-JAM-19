using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum DebugStateTimer {
	Idle,
	Pre,
	Post
}

public class DelayEffect_ObsBehaviour : MonoBehaviour
{
	public MonoBehaviour target;
	public DebugStateTimer state;

	public float idleTimer= 3;
	public float preTimer = 1;
	public float postTimer = 1.5f;

	public UnityEvent idleExpires;
	public UnityEvent preExpires;
	public UnityEvent postExpires;
	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(cycle());
    }

	private IEnumerator cycle() {
		while (true) {
			state = DebugStateTimer.Idle;
			yield return new WaitForSeconds(idleTimer);
			idleExpires.Invoke();

			state = DebugStateTimer.Pre;
			yield return new WaitForSeconds(preTimer);
			preExpires.Invoke();

			state = DebugStateTimer.Post;
			yield return new WaitForSeconds(postTimer);
			postExpires.Invoke();
		}
	}
}
