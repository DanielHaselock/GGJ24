using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelInfo
{
    public string Name;
    public int LevelTime;
}

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private
    LevelInfo[] Scenes;

    [SerializeField]
    private GameObject loaderCanvas;

    private GameObject Player;

    public CurrentLevelManager CurrentLevelManager; //Updates At start of level

    private LevelInfo NextScene;

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

    public async void LoadNextLevel() //loads in background for score to be shown
    {
        GetNextScene();
        //loaderCanvas.SetActive(true);
        AsyncLoad = SceneManager.LoadSceneAsync(NextScene.Name);
        AsyncLoad.allowSceneActivation = false;
    }

    public void GetNextScene()
    {
        int a = Random.Range(0, Scenes.Length);
        if(CurrentScene == null || CurrentScene != Scenes[a])
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

}
