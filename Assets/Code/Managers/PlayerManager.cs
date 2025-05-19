using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public List<PlayerController> players = new List<PlayerController>();

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
        controller.InitializePlayer(index, $"Player {index + 1}");
        players.Add(controller);

        LobbyUIManager.Instance?.Refresh(); // Safe call after UI is ready
    }

    public int GetPlayerCount() => players.Count;
}


