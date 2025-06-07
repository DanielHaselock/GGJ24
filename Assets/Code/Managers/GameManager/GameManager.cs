using System;
using System.Collections.Generic;
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

    private List<string> Levels = new List<string>
    {
        "BaseScene",
        "SampleScene",
        "SampleScene2",
        "CoinScene"
    };
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
        PlayerInputManager.instance.JoinPlayer();
        currentGameState = GameStates.Scoreboard;
        GoToRoundsSelect();
        //loader.LoadScene("ScoreBoard");
        //PlayerManager.Instance.SwitchActionMaps();
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

        if (this.maxRounds < 1)
            this.maxRounds = 99;

        if (this.maxRounds >= 100)
            this.maxRounds = 1;

        FindFirstObjectByType<Rounds>().updateUI(this.maxRounds);
    }

    public void EndRoundCheck()
    {
        roundsPlayed++;

        if (roundsPlayed >= maxRounds)
        {
            roundsPlayed = 0;
            GoToEndScene();
        }
        else
            loader.LoadScene("ScoreBoard");
    }

    public void GoToEndScene()
    {
        loader.LoadScene("EndScene");
        currentGameState = GameStates.GameOver;
    }

    public void LoadLevelScene() //Debug only to test the maxRounds
    {
        /*string sceneToLoad = Levels[UnityEngine.Random.Range(0, Levels.Count)];
        Debug.Log("Loading scene: " + sceneToLoad);
        loader.LoadScene(sceneToLoad);
        currentGameState = GameStates.Level;*/
        loader.LoadScene("CoinScene");
        currentGameState = GameStates.Level;
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
