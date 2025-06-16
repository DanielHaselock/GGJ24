using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages the creation and display of the player scoreboard in the game.
/// Instantiates player score panels for each player, sets their placement (1st, 2nd, etc.),
/// assigns the player's sprite and material, and displays their current score.
/// Relies on the PlayerManager singleton for player data and score ordering.
/// </summary>
public class Scoreboard : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Scoreboard Settings")]
    [SerializeField] private float scoreTimer = 5f; // Time before transitioning to the next scene
    [SerializeField] private float roundNumbTimer = 5f; // Time for UI to show before playing next scene
    [SerializeField] private float roundTypeTimer = 0.5f; // Time for UI to show before playing next scene
    [SerializeField] private GameObject playerScorePanelPrefab;
    [SerializeField] private GameObject roundSignPrefab;
    [SerializeField] private TextMeshProUGUI roundNumbertext;
    [SerializeField] private GameObject levelPrompt;

    [SerializeField] private Sprite[] playerScoreBoards;
    void Start()
    {
        CreateScoreboard();
        StartCoroutine(TimerUntilNextScene());
    }

    void CreateScoreboard()
    {
        PlayerManager.Instance.UpdateScoreOrder();
        UpdatePlayerScorePanel();
    }

    private void UpdatePlayerScorePanel()
    {
        for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
        {
            var playerScorePanel = Instantiate(playerScorePanelPrefab, transform);
            //Put the Scoreboard based on PlayerIndex+1
            Transform playerBoardTransform = playerScorePanel.transform.Find("PlayerScoreBoardImage");
            GameObject scoreBoardImage = playerBoardTransform?.gameObject;
            if (scoreBoardImage != null)
            {
                Debug.Log("Player " + PlayerManager.Instance.players[i].PlayerIndex + 1);
                scoreBoardImage.GetComponent<Image>().sprite = playerScoreBoards[PlayerManager.Instance.players[i].PlayerIndex];
            }
            //Show the players name.
            Transform nameTransform = playerScorePanel.transform.Find("PlayerName");
            GameObject name = nameTransform?.gameObject;
            if (name != null)
            {
                Debug.Log("Name of Clown is: " + PlayerManager.Instance.players[i].PlayerName);
                name.GetComponent<TextMeshProUGUI>().text = PlayerManager.Instance.players[i].PlayerName.ToString();
            }
            else
            {
                Debug.Log("Name GameObject not found in prefab");
            }

            //Show the players score.
            Transform scoreTransform = playerScorePanel.transform.Find("Score");
            GameObject score = scoreTransform?.gameObject;
            if (score != null)
            {
                Debug.Log("Score: " + PlayerManager.Instance.players[i].score);
                score.GetComponent<TextMeshProUGUI>().text = "$"+PlayerManager.Instance.players[i].score.ToString();
            }
            else
            {
                Debug.LogError("Score GameObject not found in prefab.");
            }
        }
    }


    IEnumerator TimerUntilNextScene()
    {
        yield return new WaitForSeconds(scoreTimer);
        CloseScoreBoard();
        string roundType = GameManager.Instance.calculateNextScene();
        int roundNumb = GameManager.Instance.getRoundsPlayed() + 1;

        roundNumbertext.text = "ROUND " + roundNumb.ToString();

        if (GameManager.Instance.getRoundsPlayed() == GameManager.Instance.maxRounds - 1)
        {
            roundNumbertext.text = "FINAL ROUND!";
        }
        
        roundSignPrefab.GetComponent<Animator>().SetBool("show", true);

        yield return new WaitForSeconds(roundNumbTimer);
        
        roundSignPrefab.GetComponent<Animator>().SetBool("show", false);

        yield return new WaitForSeconds(0.75f);

        //play anims Moving in objects HERE
        if (roundType == "Collect")
        {
            levelPrompt.SetActive(true);
            levelPrompt.GetComponent<Animator>().SetTrigger("collect");
        }
        if (roundType == "Race")
        {
            levelPrompt.SetActive(true);
            levelPrompt.GetComponent<Animator>().SetTrigger("race");
        }
        if (roundType == "Survive")
        {
            levelPrompt.SetActive(true);
            levelPrompt.GetComponent<Animator>().SetTrigger("survive");
        }

        yield return new WaitForSeconds(roundTypeTimer);

        //play anims Moving out objects HERE!

        // Load the next scene here
        GameManager.Instance.LoadLevelScene();
    }

    public void CloseScoreBoard()
    {
        GameObject parent = null;
        parent = this.transform.parent != null ? this.transform.parent.gameObject : null;
        if (parent != null)
        {
            parent.GetComponent<Animator>().SetBool("show", false);
        }
        else
        {
            Debug.LogError("Scoreboard not found in the scene. Please ensure it is present.");
        }
    }
}
