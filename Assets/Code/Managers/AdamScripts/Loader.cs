using System.Collections;
using UnityEngine;
/// <summary>
///  MonoBehaviour script that manages scene loading.
/// - Ensures the right scene is loading using strings.
/// - Handles scene transitions.
/// </summary>
public class Loader : MonoBehaviour
{
    [SerializeField] private GameObject LeftCurtain;
    [SerializeField] private GameObject RightCurtain;

    [SerializeField] private float curtainClosedDelay = 2.0f; // Delay before opening curtains
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeCurtains();
        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene(string sceneName)
    {

        StartCoroutine(LoadSceneWithCurtains(sceneName));
    }

    IEnumerator LoadSceneWithCurtains(string sceneName)
    {
        LeftCurtain.GetComponent<Animator>().SetTrigger("close");
        RightCurtain.GetComponent<Animator>().SetTrigger("close");
        yield return new WaitForSeconds(curtainClosedDelay);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        while (!UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            yield return null; // Wait until the scene is loaded
        }
        InitializeCurtains();
    }

    public void InitializeCurtains()
    {
        LeftCurtain = GameObject.Find("LeftCurtain");
        RightCurtain = GameObject.Find("RightCurtain");
        if (LeftCurtain == null || RightCurtain == null)
        {
            Debug.LogError("Curtains not found in the scene. Please ensure they are present.");
        }
        else
        {
            LeftCurtain.GetComponent<Animator>().SetTrigger("open");
            RightCurtain.GetComponent<Animator>().SetTrigger("open");
        }
    }
}
