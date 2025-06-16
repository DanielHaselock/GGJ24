using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceLevelManager : BaseLevelManager
{
    [SerializeField] private int[] scores = new int[4];

    private List<GameObject> players = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
    }

    public void OnPlayerFinished(GameObject player)
    {
        if (!players.Find(p => p == player))
            players.Add(player);
        else return;
        if (players.Count == PlayerManager.Instance.players.Count)
        {
            for (int i = 0; i < players.Count; ++i)
            {
                PlayerManager.Instance.players.Find(p => p.PlayerIndex == players[i].GetComponent<PlayerController>().PlayerIndex).AddScore(scores[i]);
            }

            this.OnRoundEndEarly();
        }
    }

}
