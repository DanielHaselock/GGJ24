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
    private DifficultyManager difficultyManager;

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
    [SerializeField] private GameObject ScoreEndCanvas;
    [SerializeField] private GameObject ScoreText;
    [SerializeField] private GameObject PauseCanvas;
    [SerializeField] private GameObject ClockHUD;
    [SerializeField] public GameObject Player;


    [SerializeField] private InputAction pause;


    [HideInInspector]
    private float Score = 0;

    [SerializeField]
    private string EndScene;

    void Start()
    {
        if (CheckDontDestroy())
        {
            adddontdestroyonload();

            levelManager = GetComponent<LevelManager>();
            timeManager = GetComponent<TimeManager>();
            difficultyManager = GetComponent<DifficultyManager>();
            levelManager.SetPlayer(Player);
           // pause.performed += _ => Pause();

            Screen.SetResolution(1440, 1080, FullScreenMode.FullScreenWindow);
        }
        else
        {
            GameObject obj = GetDestroyonLoadGameobject("Manager");
            levelManager = obj.GetComponent<LevelManager>();
            timeManager = obj.GetComponent<TimeManager>();
            difficultyManager = obj.GetComponent<DifficultyManager>();
        }

        timeManager.pPlayTime = false;
        state = GameState.MainMenu;
        levelManager.LoadNextLevel();
        MainMenu = SceneManager.GetActiveScene().name;

        PlayerPrefs.DeleteKey("Score");
        Score = PlayerPrefs.GetFloat("Score");

        ClockHUD.SetActive(false);

        ShowScore(false);

        AudioManager.Instance.PlaySong("menu");
    }

    void Update()
    {
        if(Input.GetKey("escape") && (SceneManager.GetActiveScene().name == MainMenu)) {
            Application.Quit();
        }

        if(Input.GetKey("escape") && (SceneManager.GetActiveScene().name != MainMenu) && (SceneManager.GetActiveScene().name != EndScene)) {
            LevelFail();
        }
    }

    private void adddontdestroyonload()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(ScoreCanvas);
        DontDestroyOnLoad(ScoreEndCanvas);
        DontDestroyOnLoad(PauseCanvas);
        DontDestroyOnLoad(ClockHUD);
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
        return objects.Length <= 2;
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

    public void PlayGame(GameObject canvas)
    {
        canvas.SetActive(false);
        Score = 0; 
        difficultyManager.UpdateScore(Score, levelManager);
        state = GameState.InGame;
        ShowScore(true);
        SetGoalText();
        timeManager.SwitchTimeExternal(TimeState.Start);
        timeManager.pPlayTime = true;
        AudioManager.Instance.PlaySong("electro");
        //PlayNextLevel();
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

        ScoreText.GetComponent<TextMeshProUGUI>().SetText("Score " + PlayerPrefs.GetFloat("Score").ToString());

        ScoreCanvas.SetActive(true);
        GameObject animobject = GameObject.FindGameObjectWithTag("UIScoreImage");
        if (!animobject)
            return;

        Animator anim = animobject.GetComponent<Animator>();

        GameObject animobject2 = GameObject.FindGameObjectWithTag("UIScoreImage2");
        if (!animobject)
            return;

        Animator anim2 = animobject2.GetComponent<Animator>();

        if (pShow)
        {
            anim.SetTrigger("Show");
            anim2.SetTrigger("Show");

        }
        else
        {
            anim.SetTrigger("Hide");
            anim2.SetTrigger("Hide");
        }

    }

    public void SetGoalText()
    {
        GameObject goalText = GameObject.FindGameObjectWithTag("GoalText");
        var wincondition = levelManager.NextScene.wincondition;

        switch (wincondition)
        {
            case GameWinCondition.REACHCHECKPOINT:
                goalText.GetComponent<TextMeshProUGUI>().SetText("RUN CLOWN RUN");
                break;
            case GameWinCondition.SURVIVE:
                goalText.GetComponent<TextMeshProUGUI>().SetText("DON'T DIE");
                break;
            case GameWinCondition.COLLECTCOINS:
                goalText.GetComponent<TextMeshProUGUI>().SetText("GRAB THE CASH");
                break;
        }
    }

    public void ShowEndScore(bool pShow)
    {
       
        var ScoreTextEnd = GameObject.FindGameObjectWithTag("UIScoreEnd");

        ScoreTextEnd.GetComponent<TextMeshProUGUI>().SetText("Score " + PlayerPrefs.GetFloat("Score").ToString());

        GameObject leftCurtain = GameObject.FindGameObjectWithTag("UIScoreImageEnd1");
        GameObject rightCurtain = GameObject.FindGameObjectWithTag("UIScoreImageEnd2");
        if (!leftCurtain)
            return;

        Animator anim1 = leftCurtain.GetComponent<Animator>();
        Animator anim2 = rightCurtain.GetComponent<Animator>();

        if (pShow)
        {
            anim1.SetTrigger("Show");
            anim2.SetTrigger("Show");

        }
        else
        {
            anim1.SetTrigger("Hide");
            anim2.SetTrigger("Hide");
        }

    }

    public void End()
    {
        state = GameState.End;
        ClockHUD.SetActive(false);
        ShowEndScore(true);
        timeManager.SwitchTimeExternal(TimeState.End);
        levelManager.LoadSpecificScene(EndScene);
        AudioManager.Instance.PlaySong("lose");
    }

    public void ShowEndUI()
    {
        GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var gameObject in gameObjects)
        {
            if (gameObject.tag == "EndUI")
            {
                gameObject.SetActive(true);
            }

        }
        timeManager.pPlayTime = false;
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
        Player.GetComponent<PlayerCustomization>().Randomize();
        Player.SetActive(false);
        End();
    }

    public void LoadNextLevel()
    {
        Player.SetActive(false);
        difficultyManager.UpdateScore(Score, levelManager);
        ClockHUD.SetActive(false);
        levelManager.LoadNextLevel();

        ShowScore(true);
        SetGoalText();

        if (levelManager.NextScene.Name.Contains("Coins"))
            AudioManager.Instance.NextBeatSwitch = 2;
        else if (levelManager.NextScene.Name.Contains("Checkpoint"))
            AudioManager.Instance.NextBeatSwitch = 3;
        else
            AudioManager.Instance.NextBeatSwitch = 1;
    }

    public void PlayNextLevel()
    {
        ShowScore(false);
        ClockHUD.SetActive(true);
        levelManager.PlayNextLevel();
    }

    public void ResetUI()
    {
        PlayerPrefs.SetFloat("Score", 0);
        Score = 0;
        ShowEndScore(false);
    }
    public void ResetScore()
    {
        difficultyManager.ResetScore();
        levelManager.ResetSceneTimes();
        PlayerPrefs.SetFloat("Score", 0);
        PlayerPrefs.DeleteKey("Score");

    }

}
