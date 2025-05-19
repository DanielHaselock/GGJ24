using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
    //Manager that handles the Lobby UI in LobbyScene.

    public TextMeshProUGUI[] playerSlots;
    public Button startButton;

    void Start()
    {
        startButton.gameObject.SetActive(false);
        UpdateLobbyUI();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        PlayerManager.Instance.RegisterPlayer(playerInput);
        UpdateLobbyUI();
    }

    private void UpdateLobbyUI()
    {
        int playerCount = PlayerManager.Instance.GetPlayerCount();

        for (int i = 0; i < playerSlots.Length; i++)
        {
            if (i < playerCount)
                playerSlots[i].text = $"Player {i + 1}: Joined";
            else
                playerSlots[i].text = $"Player {i + 1}: Not Joined";
        }

        startButton.gameObject.SetActive(playerCount >= 2); // Minimum 2 players
    }

    public void StartGame()
    {
        //This will start the randomly generated levels.
    }
}


