using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using static TimeManager;



public enum GameWinCondition
{
    REACHCHECKPOINT,
    COLLECTCOINS,
    SURVIVE
}

[System.Serializable]
public class LevelInfo
{
    public bool DebugNoPlayLevel = false;

    public int MinDifficultyForLevel = 0;

    public string Name;
    public int LevelTime;

    public int StartLevelTime;

    public bool pSpeedUpTime = false;
    public int pSpeedUpTimeAmount = 0;

    public GameWinCondition wincondition;
}

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private
    List<LevelInfo> Scenes;

    [SerializeField]
    private GameObject loaderCanvas;

    private GameObject Player;

    public CurrentLevelManager CurrentLevelManager; //Updates At start of level

    public LevelInfo NextScene;

    public LevelInfo CurrentScene;

    AsyncOperation AsyncLoad;

    

    void Start()
    {
//        DontDestroyOnLoad(loaderCanvas);
    }

    public void SetPlayer(GameObject pPlayer)
    {
        Player = pPlayer;
    }

    public void LoadNextLevel() //loads in background for score to be shown
    {
        GetNextScene();
        //loaderCanvas.SetActive(true);
        AsyncLoad = SceneManager.LoadSceneAsync(NextScene.Name);
        AsyncLoad.allowSceneActivation = false;
    }

    public void GetNextScene()
    {
        int a = Random.Range(0, Scenes.Count);
        if(CurrentScene == null || CurrentScene != Scenes[a] && Scenes[a].DebugNoPlayLevel == false && PlayerPrefs.GetInt("DifficultyLevel") >= Scenes[a].MinDifficultyForLevel)
        {
            NextScene = Scenes[a];
            return;
        }
        GetNextScene();
    }

    public int PlayNextLevel() //returns new time for timemanager
    {
        CurrentScene = NextScene;

        AsyncLoad.allowSceneActivation = true;

        GetComponent<TimeManager>().TimePlayingCurrentLevel = CurrentScene.LevelTime;
        //GetComponent<TimeManager>().ResetTime();
        GetComponent<TimeManager>().SwitchTimeExternal(TimeState.Playing);

        return NextScene.LevelTime;
    }

    public void LoadSpecificScene(string mainmenu)
    {
       // SceneManager.LoadScene(mainmenu);
        AsyncLoad = SceneManager.LoadSceneAsync(mainmenu);
        AsyncLoad.allowSceneActivation = true;
    }

    public void CheckLevelWin()
    {
        CurrentLevelManager.CheckLevelWin();
    }

    public void ChangeAllSceneTime()
    {
        foreach(LevelInfo scene in Scenes)
        {
            if(scene.pSpeedUpTime)
            {
                scene.LevelTime -= scene.pSpeedUpTimeAmount;
            }
        }
    }

    public void ChangeCurrentSceneTime(int pTimeTakeOff)
    {
        int index = Scenes.FindIndex(0, go => go == CurrentScene);
        Scenes[index].LevelTime -= pTimeTakeOff;
    }

    public void ResetSceneTimes()
    {
        foreach (LevelInfo scene in Scenes)
        {
            scene.LevelTime = scene.StartLevelTime;
        }
    }

}
