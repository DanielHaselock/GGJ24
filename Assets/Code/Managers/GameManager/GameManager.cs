using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
/// <summary>
/// GameManagerRemake is a singleton MonoBehaviour that manages game state transitions.
/// - Ensures only one instance exists (singleton pattern).
/// - Will check the settings of the game and load the appropriate scene based on game settings.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Loader loader;

    private MusicManager musicManager;

    //Add rounds here
    private int maxRounds = 1;
    private int roundsPlayed = 0;

    private GameStates currentGameState { get; set; } = GameStates.MainMenu;
    public enum GameStates
    {
        MainMenu,
        Lobby,
        RoundSelect,
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
        maxRounds = 1;
    }

    public void StartSingleplayerGame()
    {
        if (PlayerManager.Instance.players.Count >= 1) return;
        PlayerInputManager.instance.JoinPlayer();
        currentGameState = GameStates.Scoreboard;
        loader.LoadScene("ScoreBoard");
        PlayerManager.Instance.SwitchActionMaps();
        maxRounds = 99;
    }

    public void StartMultiplayerGame()
    {
        musicManager.StopMenuMusic();
        loader.LoadScene("ScoreBoard");
        currentGameState = GameStates.Scoreboard;
        PlayerManager.Instance.SwitchActionMaps();
    }

    public void GoToLobby()
    {
        loader.LoadScene("LobbyScene");
        currentGameState = GameStates.Lobby;
    }

    public void GoToMainMenu()
    {
        loader.LoadScene("NewMainMenu");
        currentGameState = GameStates.MainMenu;
        PlayerManager.Instance.SwitchActionMaps();
        PlayerManager.Instance.ClearPlayers();
    }

    public void GoToRoundsSelect()
    {
        loader.LoadScene("RoundScene");
        currentGameState = GameStates.RoundSelect;
    }

    public void addMaxRounds(int value)
    {
        this.maxRounds += value;

        if(this.maxRounds < 1)
            this.maxRounds = 1;

        FindFirstObjectByType<Rounds>().updateUI(this.maxRounds);
    }

    public void endRoundCheck()
    {
        roundsPlayed++;

        if (roundsPlayed >= maxRounds)
        {
            roundsPlayed = 0;
            GoToMainMenu();
        }
        else
            DEBUG_reloadSampleScene();
    }

    private void DEBUG_reloadSampleScene() //Debug only to test the maxRounds
    {
        if(String.Equals(SceneManager.GetActiveScene().name, "SampleScene")) //Avoid loading the same scene as it bugs the curtains
            loader.LoadScene("SampleScene2");
        else
            loader.LoadScene("SampleScene");

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
