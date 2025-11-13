using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceLevelManager : BaseLevelManager
{
    [SerializeField] private int[] scores = new int[4];

    private List<GameObject> players = new List<GameObject>();
    private bool hasAddedScores = false;

    protected override void Start()
    {
        base.Start();
        hasAddedScores = false;
    }

    public void OnPlayerFinished(GameObject player)
    {
        if (!players.Find(p => p == player))
            players.Add(player);
        else 
            return;

        if (players.Count == PlayerManager.Instance.players.Count)
        {
            addScores();
            OnRoundEndEarly();
        }
    }

    public override void OnRoundEnd()
    {
        addScores();
        deathZone.ToggleDeathZone(false);
        if(players.Count >= 1 && !audienceCheer)
        {
            audioSource.PlayOneShot(audienceCheer);
        }
        GameManager.Instance.EndRoundCheck();
    }

    private void addScores()
    {
        if (hasAddedScores)
            return;

        for (int i = 0; i < players.Count; ++i)
        {
            PlayerManager.Instance.players.Find(p => p.PlayerIndex == players[i].GetComponent<PlayerController>().PlayerIndex).AddScore(scores[i]);
        }

        hasAddedScores = true;
    }

}
