using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField] private float closedDelay = 0.3f; // Delay before closing curtains
    [SerializeField] private float openDelay = 2.0f; // Delay before opening curtains
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
        Debug.Log("Loading scene: " + sceneName);
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    IEnumerator LoadSceneCoroutine(string sceneName) //TODO Bug on loading the same scene
    {
        //If currently in the lobby scene, close the lobby board instead of the curtains.
        if (GameManager.Instance.CurrentGameState == GameManager.GameStates.Lobby)
        {
            // If the scene is the lobby, we don't want to close the curtains but to close the lobby board instead.
            GameObject lobbyBoard = GameObject.Find("LobbyBoard");

            // Close the lobby board instead of the curtains
            if (lobbyBoard != null)
            {
                lobbyBoard.GetComponent<Animator>().SetBool("show", false);
            }
            else
            {
                Debug.LogError("LobbyBoard not found in the scene. Please ensure it is present.");
            }
            yield return new WaitForSeconds(closedDelay);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            while (!UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                yield return null; // Wait until the scene is loaded
            }
            InitializeCurtains();
        }
        else
        {
            if (GameManager.Instance.CurrentGameState == GameManager.GameStates.RoundSelect)
            {
                CloseRoundSelect();
            }
            else if (GameManager.Instance.CurrentGameState == GameManager.GameStates.Scoreboard)
            {
                CloseScoreBoard();
            }
            yield return new WaitForSeconds(closedDelay);
            LeftCurtain.GetComponent<Animator>().SetTrigger("close");
            RightCurtain.GetComponent<Animator>().SetTrigger("close");
            yield return new WaitForSeconds(openDelay);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        }

        while (!UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            yield return null; // Wait until the scene is loaded
        }

        if (GameManager.Instance.CurrentGameState == GameManager.GameStates.Lobby)
        {
            GameManager.Instance.CurrentGameState = GameManager.GameStates.Lobby;
            InitializeLobbyboard();
        }
        else
        {
            InitializeCurtains();
        }
        if (GameManager.Instance.CurrentGameState == GameManager.GameStates.RoundSelect)
        {
            InitializeRoundSelect();
        }
        else if (GameManager.Instance.CurrentGameState == GameManager.GameStates.Scoreboard) InitializeScoreBoard();
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

    public void InitializeLobbyboard()
    {
        GameObject lobbyBoard = GameObject.Find("LobbyBoard");
        if (lobbyBoard != null)
        {
            lobbyBoard.GetComponent<Animator>().SetBool("show", true);
        }
        else
        {
            Debug.LogError("LobbyBoard not found in the scene. Please ensure it is present.");
        }
    }

    private void InitializeRoundSelect()
    {
        GameObject RoundOptions = GameObject.Find("RoundOptions");
        if (RoundOptions != null)
        {
            RoundOptions.GetComponent<Animator>().SetBool("show", true);
        }
        else
        {
            Debug.LogError("RoundOptions not found in the scene. Please ensure it is present.");
        }
    }

    private void InitializeScoreBoard()
    {
        GameObject scoreBoard = GameObject.Find("ScoreBoard");
        GameObject parent = null;
        parent = scoreBoard.transform.parent != null ? scoreBoard.transform.parent.gameObject : null;
        if (parent != null)
        {
            parent.GetComponent<Animator>().SetBool("show", true);
        }
        else
        {
            Debug.LogError("Scoreboard not found in the scene. Please ensure it is present.");
        }
    }

    private void CloseRoundSelect()
    {
        GameObject RoundOptions = GameObject.Find("RoundOptions");
        if (RoundOptions != null)
        {
            RoundOptions.GetComponent<Animator>().SetBool("show", false);
        }
        else
        {
            Debug.LogError("RoundOptions not found in the scene. Please ensure it is present.");
        }
    }

    private void CloseScoreBoard()
    {
        GameObject scoreBoard = GameObject.Find("ScoreBoard");
        GameObject parent = null;
        parent = scoreBoard.transform.parent != null ? scoreBoard.transform.parent.gameObject : null;
        if (parent != null)
        {
            parent.GetComponent<Animator>().SetBool("show", false);
        }
        else
        {
            Debug.LogError("Scoreboard not found in the scene. Please ensure it is present.");
        }
    }
}
