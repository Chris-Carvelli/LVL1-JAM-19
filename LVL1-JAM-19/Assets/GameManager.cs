using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerInfo {
	public PlayerController prefab;
	public Color color;
	public Transform spawn;
}

public class GameManager : MonoBehaviour
{
	public List<PlayerInfo> players;
	
	// Start is called before the first frame update
	void Start()
    {
		int pNum = 1;
		foreach (PlayerInfo player in players) {
			PlayerController pc = Instantiate(player.prefab);
			pc.playerNumber = pNum++;
			pc.GetComponent<SpriteRenderer>().color = player.color;
			pc.transform.position = player.spawn.position;
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
