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

    private MusicManager musicManager;

    private GameStates currentGameState { get; set; } = GameStates.MainMenu;
    public enum GameStates
    {
        MainMenu,
        Lobby,
        Scoreboard,
        Level,
        Credits,
        GameOver,
    }

    public GameStates CurrentGameState
    {
        get { return currentGameState; }
        set
        {
            currentGameState = value;
            Debug.Log("Current Game State: " + currentGameState);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this.gameObject);
        else Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loader = GetComponent<Loader>();
        musicManager = GetComponent<MusicManager>();
    }



    public void StartSingleplayerGame()
    {
        if (PlayerManager.Instance.players.Count >= 1) return;
        PlayerInputManager.instance.JoinPlayer();
        currentGameState = GameStates.Scoreboard;

        loader.LoadScene("ScoreBoard");
        PlayerManager.Instance.SwitchFromUIToGame();
    }

    public void StartMultiplayerGame()
    {

        musicManager.StopMenuMusic();
        loader.LoadScene("ScoreBoard");
        currentGameState = GameStates.Scoreboard;
        PlayerManager.Instance.SwitchFromUIToGame();
    }

    public void GoToLobby()
    {
        loader.LoadScene("LobbyScene");
        currentGameState = GameStates.Lobby;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
