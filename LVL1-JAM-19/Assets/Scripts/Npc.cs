﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Npc : MonoBehaviour {
    Rigidbody2D body;
    public float roamRadius = 1;
    public float speed = 1;

    Vector3 targetPosition;
    Vector3 startPosition;
    bool isRoaming = true;

    float actionTimer = 0;
    float time = 0;

    NpcIdleState currentIdleState = NpcIdleState.None;

    public List<UnityEvent> idleEvents;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        startPosition = transform.position;
        targetPosition = startPosition;
    }

    void Update() {
        if (isRoaming) {
            if (currentIdleState == NpcIdleState.None) {
                selectRandomIdleFunction();
            }
            performIdleFunction();
        }
    }


    void moveNpc() {
        if (Vector2.Distance(transform.position, targetPosition) < 0.01f) {
            currentIdleState = NpcIdleState.None;
            body.velocity = Vector2.zero;
            return;
        }
        Vector2 directionVector = (targetPosition - transform.position).normalized;
        float xVel = directionVector.x * speed;
        float yVel = directionVector.y * speed;

        body.velocity = new Vector2(xVel, yVel);
    }

    void selectRandomIdleFunction() {
        currentIdleState = (NpcIdleState)Random.Range(1, 3);
        if (currentIdleState == NpcIdleState.RandomEvent && (idleEvents == null || idleEvents.Count == 0)) {
            currentIdleState = NpcIdleState.Walking;
        }

        if (currentIdleState == NpcIdleState.Walking) {
            startMovingToRandomPosition();
        } else {
            idleEvents[Random.Range(0, idleEvents.Count)].Invoke();
            actionTimer = Random.Range(1f, 3f);
            time = 0;
        }
    }

    void performIdleFunction() {
        if (currentIdleState == NpcIdleState.Walking) {
            moveNpc();
        } else {
            time += Time.deltaTime;
            if (time > actionTimer) {
                selectRandomIdleFunction();
            }
        }
    }

    void startMovingToRandomPosition() {
        Vector3 randomDirection = Random.insideUnitCircle * roamRadius;
        Vector3 destination = randomDirection + startPosition;

        if (!NpcManager.instance.isPointWithinBorders(destination)) {
            // Default to start position
            destination = startPosition;
        }

        targetPosition = destination;
    }

    /// <summary>
    /// Stops the npc from roaming, makes them stand still
    /// </summary>
    public void stopRoaming() {
        isRoaming = false;
    }
}

enum NpcIdleState {
    None = 0,
    Walking = 1,
    RandomEvent = 2
}
