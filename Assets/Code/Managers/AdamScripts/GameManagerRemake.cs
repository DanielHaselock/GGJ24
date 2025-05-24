using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// GameManagerRemake is a singleton MonoBehaviour that manages game state transitions.
/// - Ensures only one instance exists (singleton pattern).
/// - Will check the settings of the game and load the appropriate scene based on game settings.
/// </summary>
public class GameManagerRemake : MonoBehaviour
{
    public static GameManagerRemake Instance;

    private Loader loader;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this.gameObject);
        else Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loader = GetComponent<Loader>();
    }
        


    public void StartSingleplayerGame()
    {
        if (PlayerManager.Instance.players.Count >= 1) return;
        PlayerInputManager.instance.JoinPlayer();

        loader.LoadScene("ScoreBoard");
    }

    public void StartMultiplayerGame()
    {

        loader.LoadScene("ScoreBoard");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
