using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public List<PlayerController> players = new List<PlayerController>();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GetComponent<PlayerInputManager>().onPlayerJoined += RegisterPlayer;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GetComponent<PlayerInputManager>().onPlayerJoined -= RegisterPlayer;
    }


    // Called when the scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LobbyScene") // Check if the loaded scene is the Lobby scene
        {
            // Enable player joining
            GetComponent<PlayerInputManager>().EnableJoining();
        }
        else
        {
            // Disable player joining in any other scene
            GetComponent<PlayerInputManager>().DisableJoining();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterPlayer(PlayerInput input)
    {
        var controller = input.GetComponent<PlayerController>();

        if (controller == null)
        {
            Debug.LogError("PlayerInput missing PlayerController!");
            return;
        }

        int index = players.Count;
        //controller.InitializePlayer(index, $"Player {index + 1}");
        players.Add(controller);

        DontDestroyOnLoad(controller.gameObject);

        controller.GetComponent<PlayerCustomization>().Randomize();

        if (SceneManager.GetActiveScene().name == "LobbyScene")
        {
            // Refresh the UI in the lobby scene
            LobbyUIManager.Instance?.Refresh();
        }
    }

    public int GetPlayerCount() => players.Count;
}


