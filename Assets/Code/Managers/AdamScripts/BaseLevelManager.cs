using UnityEngine;

public class BaseLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < PlayerManager.Instance.players.Count && i < spawnPoints.Length; i++)
        {
            PlayerManager.Instance.players[i].gameObject.SetActive(true);
            PlayerManager.Instance.players[i].transform.position = spawnPoints[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
