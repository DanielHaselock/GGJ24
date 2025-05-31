using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
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

    private const float MAX_TIME_TO_START = 3f; // Maximum time to fill the start bar
    [SerializeField] private Slider cancelBar;
    private EventSystem eventSystem;

    private Coroutine fillStartCoroutine;

    private Coroutine fillCancelBarCoroutine;

    private Dictionary<PlayerController, int> playerSlotMap = new();


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
        if (players == null || playerSlots == null || buttonPromptSlots == null) return;

        int slotCount = playerSlots.Length;
        bool[] slotTaken = new bool[slotCount];

        // STEP 1: Validate and assign each player to a slot
        foreach (var player in players)
        {
            int assignedSlot = -1;

            // Check if player has a previously assigned slot
            if (playerSlotMap.TryGetValue(player, out int existingSlot))
            {
                if (existingSlot >= 0 && existingSlot < slotCount && !slotTaken[existingSlot])
                {
                    assignedSlot = existingSlot; // Keep their original slot
                }
            }

            // If no valid assigned slot, assign the first available one
            if (assignedSlot == -1)
            {
                for (int i = 0; i < slotCount; i++)
                {
                    if (!slotTaken[i])
                    {
                        assignedSlot = i;
                        break;
                    }
                }
            }

            // Update the slot map
            if (assignedSlot != -1)
            {
                playerSlotMap[player] = assignedSlot;
                slotTaken[assignedSlot] = true;

                var playerAnimator = playerSlots[assignedSlot].GetComponent<Animator>();
                var promptAnimator = buttonPromptSlots[assignedSlot].GetComponent<Animator>();

                if (playerAnimator != null)
                    playerAnimator.SetBool("hasJoined", true);

                if (promptAnimator != null)
                {
                    promptAnimator.SetBool("hasJoined", true);
                    promptAnimator.SetTrigger("player_" + (assignedSlot + 1)); // Optional
                }
            }
        }

        // STEP 2: Clear any unused slots
        for (int i = 0; i < slotCount; i++)
        {
            if (!slotTaken[i])
            {
                var playerAnimator = playerSlots[i].GetComponent<Animator>();
                var promptAnimator = buttonPromptSlots[i].GetComponent<Animator>();

                if (playerAnimator != null)
                    playerAnimator.SetBool("hasJoined", false);

                if (promptAnimator != null)
                    promptAnimator.SetBool("hasJoined", false);
            }
        }

        // STEP 3: Remove players that are no longer in the player list
        var disconnectedPlayers = new List<PlayerController>();
        foreach (var kvp in playerSlotMap)
        {
            if (!players.Contains(kvp.Key))
                disconnectedPlayers.Add(kvp.Key);
        }

        foreach (var player in disconnectedPlayers)
            playerSlotMap.Remove(player);
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
            fillStartCoroutine = StartCoroutine(FillStartBarOverTime());
        }
    }

    public IEnumerator FillStartBarOverTime()
    {

        while (startBar.value < 1f)
        {
            startBar.value += Time.deltaTime / MAX_TIME_TO_START;
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

    public void FillCancelBar(PlayerController player)
    {
        if (PlayerManager.Instance.players.Find(p => p == player) == null)
        {
            Debug.LogWarning("Player not found in the player list.");
            return;
        }

        if (fillCancelBarCoroutine == null)
        {
            fillCancelBarCoroutine = StartCoroutine(FillCancelBarOverTime());
        }
    }

    private IEnumerator FillCancelBarOverTime()
    {
        while (cancelBar.value < 1f)
        {
            cancelBar.value += Time.deltaTime / MAX_TIME_TO_START;
            yield return null;
        }

        ResetCancelBar();
        GameManagerRemake.Instance.GoToMainMenu();
    }

    public void CancelFillCancelBar()
    {
        if (fillCancelBarCoroutine != null)
        {
            StopCoroutine(fillCancelBarCoroutine);
            fillCancelBarCoroutine = null;
            ResetCancelBar();
        }
    }

    public void ResetCancelBar()
    {
        cancelBar.value = 0f;
    } 

}



