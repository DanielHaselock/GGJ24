using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// Manages the lobby UI in the game, including displaying player slots and controlling the visibility of the start button.
/// Updates the UI to reflect the current list of players and enables the start button when enough players have joined.
/// </summary>
public class LobbyUIManager : MonoBehaviour
{

    [Header("UI Elements")]
    // Assign in inspector
    [SerializeField] private TMP_Text[] playerSlots;
    [SerializeField] private Button startButton;

    private void Awake()
    {
        
    }

    private void Start()
    {
        Refresh();
        startButton.gameObject.SetActive(false);
    }
    /// <summary>
    /// Refreshes the lobby UI to display the current players and their status.
    /// Updates the text of each player slot to indicate whether a player has joined or is waiting.
    /// </summary>
    public void Refresh()
    {
        var players = PlayerManager.Instance?.players;

        if (players == null || playerSlots == null) return;

        for (int i = 0; i < playerSlots.Length; i++)
        {
            if (i < players.Count)
                playerSlots[i].text = $"Clown {i + 1}: Joined";
            else
                playerSlots[i].text = $"Clown {i + 1}: Waiting...";
        }

        startButton.gameObject.SetActive(players.Count >= 2);
    }

}



