using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurviveLevelManager : BaseLevelManager
{
    [SerializeField] private int survivingScore;

    private List<GameObject> players = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        foreach (PlayerController player in PlayerManager.Instance.players)
        {
            player.gameObject.GetComponent<PlayerRespawn>().setRespawn(false);
        }
    }
    private void addPlayersScores()
    {
        for (int i = 0; i < PlayerManager.Instance.players.Count; ++i)
        {
            if (!players.Find(p => p.GetComponent<PlayerController>().PlayerIndex == PlayerManager.Instance.players[i].PlayerIndex))
            {
                PlayerManager.Instance.players[i].AddScore(survivingScore);
            }
        }
    }

    public override void OnDeath(PlayerRespawn player) 
    {
        GameObject playerobj = player.gameObject;

        if (!players.Find(p => p == playerobj))
            players.Add(playerobj);
        else return;

        if (players.Count == PlayerManager.Instance.players.Count) //all players died
            OnRoundEnd();
    }

    public override void OnRoundEnd()
    {
        deathZone.ToggleDeathZone(false);
        if(players.Count < PlayerManager.Instance.players.Count && !audienceCheered)
        {
            audioSource.PlayOneShot(audienceCheer);
        }
        foreach (PlayerController player in PlayerManager.Instance.players)
        {
            player.gameObject.GetComponent<PlayerRespawn>().setRespawn(true);
        }
        addPlayersScores();
        GameManager.Instance.EndRoundCheck();
    }
}
