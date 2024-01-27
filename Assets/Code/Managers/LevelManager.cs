using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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

    public static LevelManager Instance;

    [SerializeField] private
    LevelInfo[] Scenes;

    [SerializeField]
    private GameObject loaderCanvas;

    private LevelInfo NextScene;

    public LevelInfo CurrentScene;

    AsyncOperation AsyncLoad;

    

    void Start()
    {
        DontDestroyOnLoad(loaderCanvas);
    }

    // Update is called once per frame
    void Update()
    {

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

        //loaderCanvas.SetActive(false);
    }

    public void LoadSpecificScene(string mainmenu)
    {
        SceneManager.LoadScene(mainmenu);
    }

}
