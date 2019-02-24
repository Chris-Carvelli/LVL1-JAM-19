using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	public bool testInputs = false;
	private static GameManager _instance = null;
	public List<PlayerInfo> players;

    public static GameManager getManager() {
        return _instance;
    }

    GameState _gameState = GameState.StartCountDown;
    public GameState getGameState() {
        return _gameState;
    }

    PlayerController player1, player2;
    public int player1Score = 0;
    public int player2Score = 0;

    public List<UnityEvent> countDownEvents;
    public UnityEvent gameOverEvent;
    public Text player1ScoreText, player2ScoreText;

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
            if (pNum == 1) {
                player1 = pc;
            } else {
                player2 = pc;
            }
            pc.name += $"_{pNum}";
			pc.gameObject.layer = LayerMask.NameToLayer($"team_{pNum}");
			pc.playerNumber = pNum++;
            pc.setTint(player.playerColor);
			pc.transform.position = player.spawn.position;
		}
    }

    private void Update() {
        if (_gameState == GameState.StartCountDown) {
            startCountDown();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            exitToMenu();
        }
    }

    void startCountDown() {
        _gameState = GameState.IsCountingDown;
        StartCoroutine(countDown());
    }

    IEnumerator countDown() {
        for(int i = 0; i < countDownEvents.Count; i++) {
            if(i == 0) {
                // "Ready" takes longer to say in japanese
                yield return new WaitForSeconds(1.5f);
            } else {
                yield return new WaitForSeconds(1);
            }
            countDownEvents[i].Invoke();
        }
        countDownEnded();
    }

    void countDownEnded() {
        _gameState = GameState.Playing;
    }

    public void addScore(int player) {
        if (player == 1) {
            player1Score++;
        } else if (player == 2) {
            player2Score++;
        }
    }

    public void subtractScore(int player) {
        if (player == 1) {
            player1Score--;
        } else if (player == 2) {
            player2Score--;
        }
    }

    public void checkGameState() {
        if (player1Score + player2Score >= NpcManager.instance.npcs.Count) {
            // All npcs have been collected
            gameOver();
            player1ScoreText.text = player1Score.ToString();
            player2ScoreText.text = player2Score.ToString();
        }
    }

    void gameOver() {
        _gameState = GameState.GameOver;

        if (player1Score > player2Score) {
            print("Player 1 wins");
            player1.playerIsWinner();
            player2.playerIsLoser();

        } else if (player2Score > player1Score) {
            print("Player 2 wins");
            player1.playerIsLoser();
            player2.playerIsWinner();

        } else {
            print("Draw");
            player1.playerDrawn();
            player2.playerDrawn();
        }

        print("GAME OVER");
        if (gameOverEvent != null) {
            gameOverEvent.Invoke();
        }
    }

    public void exitToMenu() {
        if (SceneManager.sceneCountInBuildSettings == 0) {
            return;
        }
        SceneManager.LoadScene(0);
    }

    public void startGame() {
        if (SceneManager.sceneCountInBuildSettings <= 1) {
            return;
        }
        SceneManager.LoadScene(1);
    }

    public void restartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

public enum GameState {
    StartCountDown = 0,
    IsCountingDown = 1,
    Playing = 2,
    GameOver = 3
}