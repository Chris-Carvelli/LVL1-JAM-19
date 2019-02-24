using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {
	public static ObstacleManager instance;

	public List<Obstacle> obstaclePrefabs;
	public List<Obstacle> obstacles;
	public int initialObstaclesCount = 10;
	public Collider2D topBorder, bottomBorder, leftBorder, rightBorder;

	Vector2 levelMinCoord = new Vector2(-10, -10);
	Vector2 levelMaxCoord = new Vector2(10, 10);

	private void Awake() {
		instance = this;
	}

	void Start() {
		detectLevelBorders();

		spawnObstaclesAtRandom(initialObstaclesCount);
	}

	void Update() {
		if (Input.GetMouseButtonDown(1) && GameManager.getManager().testInputs) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit)) {
				if (hit.collider.tag == "Terrain") {
					spawnObstacle(hit.point);
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

	void spawnObstaclesAtRandom(int totalObstacles) {
		StartCoroutine(spawnObstaclessAtRandomWithWait(totalObstacles));
	}

	IEnumerator spawnObstaclessAtRandomWithWait(int totalObstacles) {
		for (int i = 0; i < totalObstacles; i++) {
			Vector2 randomPosition = new Vector2(Random.Range(levelMinCoord.x, levelMaxCoord.x), Random.Range(levelMinCoord.y, levelMaxCoord.y));
			while (positionContainsAPlayer(randomPosition)) {
				randomPosition = new Vector2(Random.Range(levelMinCoord.x, levelMaxCoord.x), Random.Range(levelMinCoord.y, levelMaxCoord.y));
			}
			spawnObstacle(randomPosition);
			yield return new WaitForSeconds(0.01f);
		}
	}

	public void spawnObstacle(Vector3 spawnPoint) {
		Obstacle obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
		spawnObstacle(spawnPoint, obstaclePrefab);
	}

	public void spawnObstacle(Vector3 spawnPoint, Obstacle obstaclePrefab) {
		if (!isPointWithinBorders(spawnPoint)) {
			return;
		}

		Obstacle newObstacle = Instantiate(obstaclePrefab, spawnPoint, obstaclePrefab.transform.rotation);
		//newNpc.originalPrefab = npcPrefab;

		//TMP
		newObstacle.transform.position = new Vector3(
			newObstacle.transform.position.x,
			newObstacle.transform.position.y,
			0
		);

		obstacles.Add(newObstacle);
	}

	public void clearAllObstacles() {
		foreach (Obstacle obstacle in obstacles) {
			Destroy(obstacle.gameObject);
		}
		obstacles.Clear();
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