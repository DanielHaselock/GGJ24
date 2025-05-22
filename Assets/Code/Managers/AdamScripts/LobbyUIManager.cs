using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

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



