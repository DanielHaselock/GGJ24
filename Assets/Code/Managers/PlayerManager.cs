using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
/// <summary>
/// Manages player instances, handles player joining, scene transitions, and player list updates.
/// Implements a singleton pattern to persist across scenes.
/// </summary>
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
    /// <summary>
    /// Registers a new player when they join the game.
    /// - Retrieves the PlayerController from the PlayerInput.
    /// - Adds the player to the players list and persists their GameObject across scenes.
    /// - Randomizes the player's customization.
    /// - If in the LobbyScene, refreshes the lobby UI to reflect the new player.
    /// </summary>
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
            LobbyUIManager lobbyUIManager = FindAnyObjectByType<LobbyUIManager>();
            if (lobbyUIManager != null)
            {
                lobbyUIManager.Refresh();
            }
        }
    }

    public void UpdateScoreOrder()
    {
        //Testing
        foreach (var player in players)
        {
            player.AddScore(0);
        }
        if (players.Count <= 1) return; // No need to sort if there's only one player
        players.Sort((a, b) => b.score.CompareTo(a.score)); // Sort in descending order
    }

    public int GetPlayerCount() => players.Count;
}


