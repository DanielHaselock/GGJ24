using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager Instance { get; private set; }

    [Header("UI Elements")]
    public TMP_Text[] playerSlots; // Assign in inspector
    public Button startButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Refresh();
        startButton.onClick.AddListener(StartGame);
        startButton.gameObject.SetActive(false);
    }

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

    private void StartGame()
    {
        //SceneManager.LoadScene("GameScene"); // Change to your scene name
    }
}



