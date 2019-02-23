using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour {
    public List<Npc> npcPrefabs;
    public List<Npc> npcs;
    public int initialNpcCount = 10;
    
    void Start() {
        spawnNpcsAtRandom(initialNpcCount);
    }
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.tag == "Terrain") {
                    spawnNpc(hit.point);
                }
            }
        }
    }

    void spawnNpcsAtRandom(int totalNpcs) {
        Vector2 levelMinCoord = new Vector2(-50, -50);
        Vector2 levelMaxCoord = new Vector2(50, 50);

        for (int i = 0; i < totalNpcs; i++) {
            spawnNpc(new Vector3(Random.Range(levelMinCoord.x, levelMaxCoord.x), 0, Random.Range(levelMinCoord.y, levelMaxCoord.y)));
        }
    }

    void spawnNpc(Vector3 spawnPoint) {
        Npc npcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Count)];
        Npc newNpc = Instantiate(npcPrefab, spawnPoint + Vector3.up, npcPrefab.transform.rotation);
        npcs.Add(newNpc);
    }
}