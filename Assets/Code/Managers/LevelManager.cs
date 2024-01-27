using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static LevelManager Instance;

    [SerializeField]
    public string[] Scenes;

    [SerializeField]
    private GameObject loaderCanvas;

    private string NextScene;

    public string CurrentScene;

    AsyncOperation AsyncLoad;

    

    void Start()
    {
       
        DontDestroyOnLoad(loaderCanvas);
        //GetAvailableLevels
        CurrentScene = SceneManager.GetActiveScene().name;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void LoadNextLevel() //loads in background for score to be shown
    {
        GetNextScene();
        //loaderCanvas.SetActive(true);
        AsyncLoad = SceneManager.LoadSceneAsync(NextScene);
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

    public void PlayNextLevel()
    {
        CurrentScene = NextScene;

        AsyncLoad.allowSceneActivation = true;

        //loaderCanvas.SetActive(false);

        Debug.Log("LOADED");
    }

    public void LoadSpecificScene(string mainmenu)
    {
        SceneManager.LoadScene(mainmenu);
    }

}
