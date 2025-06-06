using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private List<string> names = new List<string> { "Adam", "Keven", "Hyhy", "Daniel" };

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
        if (GameManager.Instance.CurrentGameState == GameManager.GameStates.Lobby) // Check if the loaded scene is the Lobby scene
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

        if (input.GetDevice<Mouse>() != null)
        {
            Destroy(input.gameObject);
        }
        var controller = input.GetComponent<PlayerController>();

        //controller.SetVisible(false);

        if (controller == null)
        {
            Debug.LogError("PlayerInput missing PlayerController!");
            return;
        }
        string name = names[Random.Range(0, names.Count - 1)];
        names.Remove(name);
        int index = players.Count;
        players.Add(controller);
        DontDestroyOnLoad(controller.gameObject);

        controller.InitializePlayer(index, name);
        controller.GetComponent<PlayerCustomization>().Randomize();

        if (GameManager.Instance.CurrentGameState == GameManager.GameStates.Lobby)
        controller.Setup(input);

        if (SceneManager.GetActiveScene().name == "LobbyScene")
        {
            LobbyUIManager lobbyUIManager = FindAnyObjectByType<LobbyUIManager>();
            if (lobbyUIManager != null)
            {
                lobbyUIManager.Refresh();
            }
            else
            {
                Debug.LogWarning("LobbyUIManager not found in the scene. Cannot refresh lobby UI.");
            }
        }
    }


    public void RemovePlayer(PlayerController player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
            Destroy(player.gameObject);
            LobbyUIManager lobbyUIManager = FindAnyObjectByType<LobbyUIManager>();
            if (lobbyUIManager != null)
            {
                lobbyUIManager.Refresh();
            }
            else
            {
                Debug.LogWarning("LobbyUIManager not found in the scene. Cannot refresh lobby UI.");
            }
        }
        else
        {
            Debug.LogWarning("Player not found in the player list.");
        }
    }

    public void ClearPlayers()
    {
        foreach (var player in players) Destroy(player.gameObject);
        players.Clear();
    }

    public void SwitchActionMaps()
    {
        foreach (var player in players)
        {
            player.switchActionMap();
        }
    }
    public void UpdateScoreOrder()
    {
        if (players.Count <= 1) return; // No need to sort if there's only one player
        players.Sort((a, b) => b.score.CompareTo(a.score)); // Sort in descending order
    }

    public int GetPlayerCount() => players.Count;
}


