using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour {
    public static NpcManager instance;

    public List<Npc> npcPrefabs;
    public List<Npc> npcs;
    public int initialNpcCount = 10;
    public Collider2D topBorder, bottomBorder, leftBorder, rightBorder;

    Vector2 levelMinCoord = new Vector2(-10, -10);
    Vector2 levelMaxCoord = new Vector2(10, 10);

    private void Awake() {
        instance = this;
    }

    void Start() {
        spawnNpcsAtRandom(initialNpcCount);
        detectLevelBorders();
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

    void detectLevelBorders() {
        levelMinCoord.x = leftBorder.transform.position.x + leftBorder.bounds.size.x / 2 + 1;
        levelMaxCoord.x = rightBorder.transform.position.x - rightBorder.bounds.size.x / 2 - 1;

        levelMinCoord.y = bottomBorder.transform.position.y + bottomBorder.bounds.size.y / 2 + 1;
        levelMaxCoord.y = topBorder.transform.position.y - topBorder.bounds.size.y / 2 - 1;
    }

    void spawnNpcsAtRandom(int totalNpcs) {
        for (int i = 0; i < totalNpcs; i++) {
            spawnNpc(new Vector3(Random.Range(levelMinCoord.x, levelMaxCoord.x), Random.Range(levelMinCoord.y, levelMaxCoord.y), 0));
        }
    }

    void spawnNpc(Vector3 spawnPoint) {
        if (!isPointWithinBorders(spawnPoint)) {
            return;
        }
        Npc npcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Count)];
        Npc newNpc = Instantiate(npcPrefab, spawnPoint/* - Vector3.forward / 2*/, npcPrefab.transform.rotation);

		//TMP
		newNpc.transform.position = new Vector3(
			newNpc.transform.position.x,
			newNpc.transform.position.y,
			0
		);
        npcs.Add(newNpc);
    }

    public bool isPointWithinBorders(Vector2 point) {
        return point.x >= levelMinCoord.x && point.x <= levelMaxCoord.x && point.y >= levelMinCoord.y && point.y <= levelMaxCoord.y;
    }

	private void OnCollisionEnter2D(Collision2D collision) {
		
	}
}