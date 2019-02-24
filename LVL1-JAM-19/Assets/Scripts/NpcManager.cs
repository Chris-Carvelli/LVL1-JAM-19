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
        detectLevelBorders();

        spawnNpcsAtRandom(initialNpcCount);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && GameManager.getManager().testInputs) {
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
        StartCoroutine(spawnNpcsAtRandomWithWait(totalNpcs));
    }

    IEnumerator spawnNpcsAtRandomWithWait(int totalNpcs) {
        for (int i = 0; i < totalNpcs; i++) {
            Vector2 randomPosition = new Vector2(Random.Range(levelMinCoord.x, levelMaxCoord.x), Random.Range(levelMinCoord.y, levelMaxCoord.y));
            while (positionContainsAPlayer(randomPosition)) {
                randomPosition = new Vector2(Random.Range(levelMinCoord.x, levelMaxCoord.x), Random.Range(levelMinCoord.y, levelMaxCoord.y));
            }
            spawnNpc(randomPosition);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void spawnNpc(Vector3 spawnPoint) {
		Npc npcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Count)];
		spawnNpc(spawnPoint, npcPrefab);
    }

	public void spawnNpc(Vector3 spawnPoint, Npc npcPrefab) {
		if (!isPointWithinBorders(spawnPoint)) {
			return;
		}
		
		Npc newNpc = Instantiate(npcPrefab, spawnPoint, npcPrefab.transform.rotation);
		newNpc.originalPrefab = npcPrefab;

		//TMP
		newNpc.transform.position = new Vector3(
			newNpc.transform.position.x,
			newNpc.transform.position.y,
			0
		);

		npcs.Add(newNpc);
	}

	public void clearAllNpcs() {
        foreach (Npc npc in npcs) {
            Destroy(npc.gameObject);
        }
        npcs.Clear();
    }

    public bool isPointWithinBorders(Vector2 point) {
        return point.x >= levelMinCoord.x && point.x <= levelMaxCoord.x && point.y >= levelMinCoord.y && point.y <= levelMaxCoord.y;
    }

    bool positionContainsAPlayer(Vector2 point) {
        float marginToPlayers = 2;

        foreach (PlayerInfo playerInfo in GameManager.getManager().players) {
            if (point.x >= playerInfo.spawn.position.x - marginToPlayers && point.x <= playerInfo.spawn.position.x + marginToPlayers && point.y >= playerInfo.spawn.position.y - marginToPlayers && point.y <= playerInfo.spawn.position.y + marginToPlayers) {
                return true;
            }
        }
        return false;
    }
}