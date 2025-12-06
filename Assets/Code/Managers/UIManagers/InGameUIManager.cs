using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Image playerIndexDisplay;
    [SerializeField] private TMP_Text playerNameDisplay;
    [SerializeField] private Sprite[] playerIndicators;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this.gameObject);
        else Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pauseMenu == null)
        {
            Debug.LogWarning("Pause Menu is not assigned in the inspector.");
            pauseMenu = GameObject.Find("PauseMenu");
            if (pauseMenu == null)
            {
                Debug.LogError("Pause Menu GameObject not found in the scene.");
            }
        }

        if (playerIndexDisplay == null)
        {
            Debug.LogWarning("Player Index Display is not assigned in the inspector.");
            GameObject pidObj = GameObject.Find("PlayerIndexDisplay");
            if (pidObj != null)
            {
                playerIndexDisplay = pidObj.GetComponent<Image>();
            }
            if (playerIndexDisplay == null)
            {
                Debug.LogError("Player Index Display Image component not found in the scene.");
            }
        }

        if (playerIndicators == null || playerIndicators.Length == 0)
        {
            Debug.LogWarning("Player Indicators are not assigned in the inspector.");
        }

        pauseMenu.SetActive(false);
    }

    public void ShowPauseMenu(int PlayerIndex, string PlayerName)
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
            // Update player index display
            playerIndexDisplay.sprite = playerIndicators[PlayerIndex];
            // Update player name display
            playerNameDisplay.text = PlayerName;
        }
    }

    public void HidePauseMenu(int PlayerIndex, string PlayerName)
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    #region Button Callbacks
    public void OnResumeButtonPressed()
    {
        GameManager.Instance.TogglePauseGame();
    }

    public void OnQuitToMainMenuButtonPressed()
    {
        Time.timeScale = 1f; // Ensure time scale is reset
        GameManager.Instance.GoToCredits();
    }
    #endregion
}
