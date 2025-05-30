using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using Unity.Multiplayer.Center.Common;

/// <summary>
/// Manages the lobby UI in the game, including displaying player slots and controlling the visibility of the start button.
/// Updates the UI to reflect the current list of players and enables the start button when enough players have joined.
/// </summary>
public class LobbyUIManager : MonoBehaviour
{

    [Header("UI Elements")]
    // Assign in inspector
    [SerializeField] private GameObject[] playerSlots;
    [SerializeField] private GameObject[] buttonPromptSlots;
    [SerializeField] private Slider startBar;

    [SerializeField] private const float maxTimeToStart = 5f; // Maximum time to fill the start bar
    [SerializeField] private Slider cancelBar;
    private EventSystem eventSystem;

    private Coroutine fillStartCoroutine;

    private void Awake()
    {

    }

    private void Start()
    {
        Refresh();
        // Make sure GameManager is the actual runtime instance

        eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            Debug.LogError("EventSystem not found in the scene. Please add an EventSystem component.");
        }
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
            {
                playerSlots[i].GetComponent<Animator>().SetTrigger("join");
                buttonPromptSlots[i].GetComponent<Animator>().SetTrigger("player_" + (i + 1));
            }
        }
    }

    public void FillStartBar(PlayerController player)
    {
        if (PlayerManager.Instance.players.Find(p => p == player) == null)
        {
            Debug.LogWarning("Player not found in the player list.");
            return;
        }

        if (PlayerManager.Instance.players.Count < 2)
        {
            Debug.LogWarning("Not enough players to start the game.");
            return;
        }

        if (fillStartCoroutine == null)
        {
            fillStartCoroutine = StartCoroutine(FillBarOverTime());
        }
    }

    public IEnumerator FillBarOverTime()
    {

        while (startBar.value < 1f)
        {
            startBar.value += Time.deltaTime / maxTimeToStart;
            yield return null;
        }

        ResetStartBar();
        GameManagerRemake.Instance.StartMultiplayerGame();
    }

    public void CancelFillStartBar()
    {
        if (fillStartCoroutine != null)
        {
            StopCoroutine(fillStartCoroutine);
            fillStartCoroutine = null;
            ResetStartBar();
        }
    }



    public void ResetStartBar()
    {
        startBar.value = 0f;
    }

}



