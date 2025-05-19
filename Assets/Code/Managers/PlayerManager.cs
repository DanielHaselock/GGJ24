using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public List<PlayerController> players = new List<PlayerController>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterPlayer(PlayerInput playerInput)
    {
        var controller = playerInput.GetComponent<PlayerController>();
        int index = players.Count;
        string name = $"Player {index + 1}";

        controller.InitializePlayer(index, name);
        players.Add(controller);
    }

    public int GetPlayerCount() => players.Count;
}


