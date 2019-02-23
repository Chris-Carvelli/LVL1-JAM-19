using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Npc : MonoBehaviour {
    NavMeshAgent agent;
    Vector3 startPosition;
    public float roamRadius = 4;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
    }
    
    void Update() {
        if (!agent.hasPath && !agent.pathPending) {
            agent.destination = RandomNavmeshLocation(roamRadius);
        }
    }

    public Vector3 RandomNavmeshLocation(float radius) {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += startPosition;
        
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
            finalPosition = hit.position;

        } else if (NavMesh.SamplePosition(startPosition, out hit, radius, 1)) {
            finalPosition = hit.position;

        } else {
            finalPosition = startPosition;
        }
        return finalPosition;
    }
}
