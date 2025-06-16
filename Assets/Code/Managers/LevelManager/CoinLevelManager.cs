using System.ComponentModel;
using UnityEngine;

public class CoinLevelManager : BaseLevelManager
{
    [ReadOnly(true)] private int coinsRemaining;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
        coinsRemaining = GameObject.FindGameObjectsWithTag("Coin").Length;
    }

    public void OnCollectCoin(PlayerController player)
    {
        coinsRemaining--;
        Debug.Log($"Coins remaining: {coinsRemaining}");
        if (coinsRemaining <= 0)
        {
            Debug.Log("All coins collected!");

            PlayerManager.Instance.checkGameStateAndPlayers();
            this.OnRoundEndEarly();
        }
    }
}
