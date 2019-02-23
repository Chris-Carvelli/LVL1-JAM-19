using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerInfo {
	public PlayerController prefab;
	public Color playerColor;
	public Color teamColor;
	public Color teamColorDark	;
	public Transform spawn;
}

public class GameManager : MonoBehaviour
{
	private static GameManager _instance = null;
	public List<PlayerInfo> players;
	
	public static GameManager getManager() {
		return _instance;
	}

	private void Awake() {
		if (_instance == null)
			_instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
		int pNum = 1;
		foreach (PlayerInfo player in players) {
			PlayerController pc = Instantiate(player.prefab);
			pc.name += $"_{pNum}";
			pc.gameObject.layer = LayerMask.NameToLayer($"team_{pNum}");
			pc.playerNumber = pNum++;
            pc.setTint(player.playerColor);
			pc.transform.position = player.spawn.position;
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
