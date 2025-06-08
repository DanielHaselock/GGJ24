using UnityEngine;

public class BaseLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameStates.Level)
        {
            GameManager.Instance.CurrentGameState = GameManager.GameStates.Level;
            Debug.Log("Current Game State: " + GameManager.Instance.CurrentGameState);
        }

        for (int i = 0; i < PlayerManager.Instance.players.Count && i < spawnPoints.Length; i++)
        {
            Debug.Log($"Activating player {i} at spawn point {spawnPoints[i].name}");
            PlayerManager.Instance.players[i].gameObject.SetActive(true);
            PlayerManager.Instance.players[i].transform.position = spawnPoints[i].transform.position;
            PlayerManager.Instance.players[i].SetVisible(true);
        }
    }

    public Vector3 getSpawnPointForPlayer(GameObject player)
    {
        for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
        {
            if (PlayerManager.Instance.players[i].gameObject == player)
                return spawnPoints[i].transform.position;
        }

        return Vector3.zero;
    }
}
