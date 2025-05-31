using UnityEngine;

public class BaseLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManagerRemake.Instance.CurrentGameState != GameManagerRemake.GameStates.Level)
        {
            GameManagerRemake.Instance.CurrentGameState = GameManagerRemake.GameStates.Level;
            Debug.Log("Current Game State: " + GameManagerRemake.Instance.CurrentGameState);
        }

        for (int i = 0; i < PlayerManager.Instance.players.Count && i < spawnPoints.Length; i++)
        {
            Debug.Log($"Activating player {i} at spawn point {spawnPoints[i].name}");
            PlayerManager.Instance.players[i].gameObject.SetActive(true);
            PlayerManager.Instance.players[i].transform.position = spawnPoints[i].transform.position;
            PlayerManager.Instance.players[i].SetVisible(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
