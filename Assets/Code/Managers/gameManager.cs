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

        ShowScore(Score, false);
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

    private GameObject GetDestroyonLoadGameobject(string tag)
    {
        GameObject temp = null;

        temp = new GameObject();
        Object.DontDestroyOnLoad(temp);
        UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
        Object.DestroyImmediate(temp);
        temp = null;

        GameObject[] objects = dontDestroyOnLoad.GetRootGameObjects();

        foreach (GameObject obj in objects)
        {
            if (obj.gameObject.tag == tag)
                return obj;
        }
        return null;
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
        Score = 0;
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
        if (!ScoreCanvas)
            ScoreCanvas = GetDestroyonLoadGameobject("Score");

        if(ScoreText)
            ScoreText.GetComponent<TextMeshProUGUI>().SetText(score.ToString());

        ScoreCanvas.SetActive(pShow);
    }

    public void End()
    {
        state = GameState.End;
        timeManager.pPlayTime = false;
        levelManager.LoadSpecificScene(EndScene);
        ShowScore(Score, true);
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

    public void ResetUI()
    {
        ShowScore(Score, false); //TODO FIX SCORE BUG
    }
}
