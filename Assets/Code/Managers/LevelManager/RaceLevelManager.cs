using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RaceLevelManager : BaseLevelManager
{
    [SerializeField] private int[] scores = new int[4];

    private List<GameObject> players = new List<GameObject>();

    public void OnPlayerFinished(GameObject player)
    {
        if(!players.Find(p => p == player))
            players.Add(player);

        if(players.Count == PlayerManager.Instance.players.Count)
        {
            for(int i = 0; i < players.Count; ++i)
            {
                PlayerManager.Instance.players.Find(p => p.PlayerIndex == players[i].GetComponent<PlayerController>().PlayerIndex).AddScore(scores[i]);
            }

            GameManager.Instance.EndRoundCheck();
        }
    }

}
