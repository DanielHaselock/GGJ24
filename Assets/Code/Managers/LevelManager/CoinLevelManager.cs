using System.ComponentModel;
using UnityEngine;

public class CoinLevelManager : BaseLevelManager
{
    [SerializeField] private GameObject[] coins;

    [ReadOnly(true)] private int coinsRemaining;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
        coinsRemaining = coins.Length;
        foreach (var coin in coins)
        {
            coin.SetActive(true);
        }
    }

    public void OnCollectCoin()
    {
        coinsRemaining--;
        Debug.Log($"Coins remaining: {coinsRemaining}");
        if (coinsRemaining <= 0)
        {
            Debug.Log("All coins collected!");
            GameManager.Instance.EndRoundCheck();
        }
    }
    


    // Update is called once per frame
    private void Update()
    {

    }
}
