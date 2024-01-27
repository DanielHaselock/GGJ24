using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{

    private LevelManager levelManager;
    private TimeManager timeManager;  


    enum GameState
    {
        MainMenu = 0,
        InGame = 1,
        Pause = 2,
        End = 3,
    }

    GameState state;

    [SerializeField] private string MainMenu = "MainMenu";

    [SerializeField] private GameObject ScoreCanvas;
    [SerializeField] private GameObject ScoreText;
    [SerializeField] private GameObject PauseCanvas;
    [SerializeField] public GameObject Player;


    [SerializeField] private InputAction pause;

    public float Score = 0;

    [SerializeField]
    private string EndScene;

    private bool Loaded = false;

    void Start()
    {

        levelManager = GetComponent<LevelManager>();
        timeManager = GetComponent<TimeManager>();

        if (CheckDontDestroy())
        {
            adddontdestroyonload();

            
            levelManager.SetPlayer(Player);
            pause.performed += _ => Pause();
        }

        timeManager.pPlayTime = false;
        state = GameState.MainMenu;
        levelManager.LoadNextLevel();
        MainMenu = SceneManager.GetActiveScene().name;
    }

    private void adddontdestroyonload()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(ScoreCanvas);
        DontDestroyOnLoad(PauseCanvas);
        DontDestroyOnLoad(Player);
        Loaded = true;
    }

    private bool CheckDontDestroy()
    {
        GameObject temp = null;
  
        temp = new GameObject();
        Object.DontDestroyOnLoad(temp);
        UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
        Object.DestroyImmediate(temp);
        temp = null;

        GameObject[] objects = dontDestroyOnLoad.GetRootGameObjects();
        return objects.Length == 0;
    }

    private void OnEnable()
    {
        pause.Enable();
    }

    private void OnDisable()
    {
        pause.Disable();
    }

    public void PlayGame()
    {
        state = GameState.InGame;
        levelManager.PlayNextLevel();
    }


    public void Pause()
    {
        if(state == GameState.Pause)
        {
            PauseCanvas.SetActive(false);
            state = GameState.InGame;
            Time.timeScale = 1;
        }
        else if (state == GameState.InGame)
        {
            PauseCanvas.SetActive(true);
            state = GameState.Pause;
            Time.timeScale = 0;
        }
    }

    public void ShowScore(float score, bool pShow)
    {
        ScoreText.GetComponent<TextMeshProUGUI>().SetText(score.ToString());
        ScoreCanvas.SetActive(pShow);
    }

    public void End()
    {
        state = GameState.End;
        timeManager.pPlayTime = false;
        levelManager.LoadSpecificScene(EndScene);
    }

    public void LevelSucceed(float pScore)
    {
        Score += pScore;
        LoadNextLevel();
    }

    public void LevelFail(float pScore)
    {
        Score += pScore;
        Player.SetActive(false);
       // ShowScore(Score, true);
        End();
    }

    public void LoadNextLevel()
    {
        Player.SetActive(false);
        ShowScore(Score, true);
        levelManager.LoadNextLevel();
    }

    public void PlayNextLevel()
    {
        ShowScore(Score, false);
        timeManager.TimePlayingCurrentLevel = levelManager.PlayNextLevel();
    }
}
