using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using Unity.VisualScripting;
using static TimeManager;

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


    [HideInInspector]
    private float Score = 0;

    [SerializeField]
    private string EndScene;

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

        
        Score = PlayerPrefs.GetFloat("Score");

        ShowScore(false);
    }

    private void adddontdestroyonload()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(ScoreCanvas);
        DontDestroyOnLoad(PauseCanvas);
        DontDestroyOnLoad(Player);
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

    public void ShowScore(bool pShow)
    {
        if (!ScoreCanvas)
            ScoreCanvas = GetDestroyonLoadGameobject("Score");

        if (!ScoreText)
            ScoreText = GameObject.FindGameObjectWithTag("ScoreText");

        ScoreText.GetComponent<TextMeshProUGUI>().SetText(PlayerPrefs.GetFloat("Score").ToString());

        ScoreCanvas.SetActive(true);
        GameObject animobject = GameObject.FindGameObjectWithTag("UIScoreImage");
        if (!animobject)
            return;

        Animator anim = animobject.GetComponent<Animator>();

        if (pShow)
        {
            anim.SetBool("ScreenHide", false);
            anim.SetBool("ScreenShow", true);
        }
        else
        {
            anim.SetBool("ScreenHide", true);
            anim.SetBool("ScreenShow", false);
        }

    }

    public void End()
    {
        state = GameState.End;
        timeManager.pPlayTime = false;
        levelManager.LoadSpecificScene(EndScene);
        ShowScore(true);
    }

    public void LevelSucceed(float pScore)
    {
        Score = PlayerPrefs.GetFloat("Score") + pScore;
        PlayerPrefs.SetFloat("Score", Score);
        timeManager.SwitchTimeExternal(TimeState.Score);
        LoadNextLevel();
    }

    public void LevelFail()
    {
        Player.SetActive(false);
        End();
    }

    public void LoadNextLevel()
    {
        Player.SetActive(false);
        ShowScore(true);
        levelManager.LoadNextLevel();
    }

    public void PlayNextLevel()
    {
        ShowScore(false);
        timeManager.TimePlayingCurrentLevel = levelManager.PlayNextLevel();
    }

    public void ResetUI()
    {
        PlayerPrefs.SetFloat("Score", 0);
        Score = 0;
        ShowScore(false);
    }
    public void ResetScore()
    {
        PlayerPrefs.SetFloat("Score", 0);
        PlayerPrefs.DeleteKey("Score");
    }

}
