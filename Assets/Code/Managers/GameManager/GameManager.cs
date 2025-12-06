using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Loader), typeof(MusicManager))]
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
    public int maxRounds { get; private set; } = 5;
    private int roundsPlayed = 0;
    private bool roundStopped = false;

    private GameStates currentGameState { get; set; } = GameStates.MainMenu;
    private GameStates nextGameState { get; set; } = GameStates.MainMenu;

    [SerializeField] private List<string> Levels = new List<string>{};
    private List<string> currentLevels;

    private bool hasFoundNextLevel = false;

    private string nextSceneToLoad = "";
    
    public enum GameStates
    {
        MainMenu,
        Lobby,
        RoundSelect,
        Scoreboard,
        CoinLevel,
        SurviveLevel,
        RaceLevel,
        Credits,
        GameOver,
        Pause,
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
        maxRounds = 5;
        currentLevels = new List<string>(Levels);
    }

    public void StartSingleplayerGame()
    {
        PlayerInputManager.instance.JoinPlayer();
        GoToRoundsSelect();
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
        PlayerManager.Instance.ClearPlayers();
    }

    public void GoToCredits()
    {
        loader.LoadScene("Credits");
        currentGameState = GameStates.Credits;
    }

    public void GoToMainMenu()
    {
        loader.LoadScene("MainMenu");
        currentGameState = GameStates.MainMenu;
        PlayerManager.Instance.SwitchActionMaps();
        PlayerManager.Instance.ClearPlayers();
        currentLevels = new List<string>(Levels);
    }

    public void GoToRoundsSelect()
    {
        loader.LoadScene("RoundScene");
        currentGameState = GameStates.RoundSelect;
    }

    public void addMaxRounds(int value)
    {
        this.maxRounds += (value * 5);

        if (this.maxRounds < 5)
            this.maxRounds = 30;

        if (this.maxRounds >= 31)
            this.maxRounds = 5;

        FindFirstObjectByType<Rounds>().updateUI(this.maxRounds);
    }

    public void EndRoundCheck() //checks if round has already ended
    {
        if(roundStopped) //round has already been stopped
            return;

        roundStopped = true;

        roundsPlayed++;
        if (roundsPlayed >= maxRounds)
        {
            roundsPlayed = 0;
            GoToEndScene();
        }
        else
            GoToScoreboard();
    }

    public int getRoundsPlayed()
    {
        return roundsPlayed;
    }

    private void GoToScoreboard()
    {
        loader.LoadScene("ScoreBoard");
        currentGameState = GameStates.Scoreboard;
    }

    public void GoToEndScene()
    {
        loader.LoadScene("EndScene");
        currentGameState = GameStates.GameOver;
    }

    public string calculateNextScene()
    {
        string sceneToLoad = currentLevels[UnityEngine.Random.Range(0, currentLevels.Count)];
        currentLevels.Remove(sceneToLoad);

        if(currentLevels.Count == 0) //in case we run out of levels
            currentLevels = new List<string>(Levels);

        string roundType = "";
        Debug.Log("calculating scene: " + sceneToLoad);

        switch (sceneToLoad)
        {
            case "Collect1":
            case "Collect2":
            case "Collect3":
            case "Collect4":
                nextGameState = GameStates.CoinLevel;
                roundType = "Collect";
                break;
            case "Race1":
            case "Race2":
            case "Race3":
            case "Race4":
                nextGameState = GameStates.RaceLevel;
                roundType = "Race";
                break;
            case "Survive1":
            case "Survive2":
            case "Survive3":
            case "Survive4":
                nextGameState = GameStates.SurviveLevel;
                roundType = "Survive";
                break;      
        }
        hasFoundNextLevel = true;

        nextSceneToLoad = sceneToLoad;
        
        return roundType;
    }

    public void LoadLevelScene()
    {
        if (!hasFoundNextLevel) 
        {
            Debug.Log("BUG with level loading"); //should never happen
            calculateNextScene();
        }

        loader.LoadScene(nextSceneToLoad);
        PlayerManager.Instance.unFreezePlayers();
        roundStopped = false;

        currentGameState = nextGameState;
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void TogglePauseGame(int PlayerIndex = 0, string PlayerName = "Unknown")
    {
        // Only allow pausing in certain game states
        if (currentGameState != GameStates.Pause &&
            currentGameState != GameStates.CoinLevel &&
            currentGameState != GameStates.RaceLevel &&
            currentGameState != GameStates.SurviveLevel)
        {
            Debug.Log("Cannot toggle pause in the current game state: " + currentGameState);
            return;
        }

        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            currentGameState = GameStates.Pause;
            // Show pause menu UI
            if (InGameUIManager.Instance != null)
            {
                InGameUIManager.Instance.ShowPauseMenu(PlayerIndex, PlayerName);
            } 
        }
        else
        {
            Time.timeScale = 1f;
            // ASSUME: when we unpause, we are back to the same scene. 
            // nextGameState holds the correct state, and has not been updated yet
            currentGameState = nextGameState;
            // Hide pause menu UI
            if (InGameUIManager.Instance != null)
            {
                InGameUIManager.Instance.HidePauseMenu(PlayerIndex, PlayerName);
            }
        }
    }
}
