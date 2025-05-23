using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private GameObject playerScorePanelPrefab;
    void Start()
    {
        CreateScoreboard();
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

            Transform playerScorePlacementTransform = playerScorePanel.transform.Find("ScorePlacement");
            GameObject playerScorePlacement = playerScorePlacementTransform?.gameObject;
            if (playerScorePlacement != null)
            {
                if (PlayerManager.Instance.players.Count <= 1) playerScorePlacement.GetComponent<TextMeshProUGUI>().text = "";
                else
                {
                    switch (i)
                    {
                        case 0:
                            playerScorePlacement.GetComponent<TextMeshProUGUI>().text = "1st";
                            break;
                        case 1:
                            playerScorePlacement.GetComponent<TextMeshProUGUI>().text = "2nd";
                            break;
                        case 2:
                            playerScorePlacement.GetComponent<TextMeshProUGUI>().text = "3rd";
                            break;
                        case 3:
                            playerScorePlacement.GetComponent<TextMeshProUGUI>().text = "4th";
                            break;
                    }
                }
            }
            Transform playerSpriteTransform = playerScorePanel.transform.Find("PlayerSprite");
            GameObject playerSprite = playerSpriteTransform?.gameObject;
            if (playerSprite != null)
            {
                playerSprite.GetComponent<Image>().sprite = PlayerManager.Instance.players[i].GetComponent<SpriteRenderer>().sprite;
                playerSprite.GetComponent<Image>().material = PlayerManager.Instance.players[i].GetComponent<SpriteRenderer>().material;
                playerSprite.GetComponent<Image>().preserveAspect = true;
                playerSprite.GetComponent<Image>().SetNativeSize();
            }
            else
            {
                Debug.LogError("PlayerSprite GameObject not found in prefab.");
            }
            Transform scoreTransform = playerScorePanel.transform.Find("Score");
            GameObject score = scoreTransform?.gameObject;
            if (score != null)
            {
                Debug.Log("Score: " + PlayerManager.Instance.players[i].score);
                score.GetComponent<TextMeshProUGUI>().text = PlayerManager.Instance.players[i].score.ToString();
            }
            else
            {
                Debug.LogError("Score GameObject not found in prefab.");
            }
        }
    }
}
