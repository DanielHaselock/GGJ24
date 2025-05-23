using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManagerRemake : MonoBehaviour
{
    public static GameManagerRemake Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this.gameObject);
        else Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void StartSingleplayerGame()
    {
        if (PlayerManager.Instance.players.Count >= 1) return;
        PlayerInputManager.instance.JoinPlayer();

        SceneManager.LoadScene("ScoreBoard");

    }

    public void StartMultiplayerGame()
    {

        SceneManager.LoadScene("ScoreBoard");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
