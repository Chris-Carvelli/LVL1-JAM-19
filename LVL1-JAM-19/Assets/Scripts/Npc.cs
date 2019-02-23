using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Npc : MonoBehaviour {
    Rigidbody2D body;
    public float roamRadius = 1;
    public float speed = 1;

    Vector3 targetPosition;
    bool isMovingTowardsTarget = false;
    Vector3 startPosition;
    bool isRoaming = true;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        startPosition = transform.position;
        targetPosition = startPosition;
    }

    void Update() {
        if (isRoaming) {
            if (!isMovingTowardsTarget) {
                targetPosition = getRandomTargetLocation(roamRadius);
                isMovingTowardsTarget = true;
            }
            if (isMovingTowardsTarget){
                moveNpc();
            }
        }
    }


    void moveNpc() {
        if (Vector2.Distance(transform.position, targetPosition) < 0.01f) {
            isMovingTowardsTarget = false;
            return;
        }
        Vector2 directionVector = (targetPosition - transform.position).normalized;
        float xVel = directionVector.x * speed;
        float yVel = directionVector.y * speed;

        body.velocity = new Vector2(xVel, yVel);
    }

    Vector3 getRandomTargetLocation(float radius) {
        Vector3 randomDirection = Random.insideUnitCircle * radius;
        Vector3 destination = randomDirection + startPosition;

        if (NpcManager.instance.isPointWithinBorders(destination)) {
            return destination;
        }

        // Default to start position
        return startPosition;
    }

    /// <summary>
    /// Stops the npc from roaming, makes them stand still
    /// </summary>
    public void stopRoaming() {
        isRoaming = false;
    }
}
