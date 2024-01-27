using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

    [SerializeField] private string MainMenu;

    [SerializeField] private GameObject ScoreCanvas;
    [SerializeField] private GameObject PauseCanvas;


    [SerializeField] private InputAction pause;

    void Start()
    {
        adddontdestroyonload();
        state = GameState.MainMenu;
        levelManager = GetComponent<LevelManager>();
        timeManager = GetComponent<TimeManager>();
        timeManager.enabled = false;
        levelManager.LoadNextLevel();
        MainMenu = SceneManager.GetActiveScene().name;
        pause.performed += _ => Pause();
    }

    private void adddontdestroyonload()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(ScoreCanvas);
        DontDestroyOnLoad(PauseCanvas);
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
        timeManager.enabled = true;
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
        ScoreCanvas.SetActive(pShow);
    }

    public void End()
    {

    }

    public void Quit()
    {
        state = GameState.MainMenu;
        timeManager.enabled = false;
        levelManager.LoadSpecificScene(MainMenu);
    }
}
