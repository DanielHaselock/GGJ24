using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeManager;

public class CurrentLevelManager : MonoBehaviour
{
    [HideInInspector] private GameObject Player;

    [SerializeField] private GameObject PlayerSpawn;

    [HideInInspector] private GameObject Managers;

    public float Score = 10;

    [HideInInspector]
    public bool pWin = false;

    public enum GameWinCondition
    {
        REACHCHECKPOINT,
        COLLECTCOINS,
        SURVIVE
    }

    public GameWinCondition wincondition;


    private void Start()
    {
        GetPlayer();
        SpawnPlayer();
        Managers.GetComponent<LevelManager>().CurrentLevelManager = this;
        Managers.GetComponent<TimeManager>().pPlayTime = true;
        Managers.GetComponent<TimeManager>().state = TimeState.Playing;
        pWin = false;
    }

    private void GetPlayer()
    {
        GameObject temp = null;
        try
        {
            temp = new GameObject();
            Object.DontDestroyOnLoad(temp);
            UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
            Object.DestroyImmediate(temp);
            temp = null;

            GameObject[] objects =  dontDestroyOnLoad.GetRootGameObjects();

            foreach(GameObject obj in objects)
            {
                if(obj.tag == "Player")
                {
                    Player = obj;
                }
                if (obj.tag == "Manager")
                {
                    Managers = obj;
                }
            }

        }
        finally
        {
            if (temp != null)
                Object.DestroyImmediate(temp);
        }
    }

    public void SetPlayer(GameObject pPlayer)
    {
        Player = pPlayer;
    }
    public void SpawnPlayer()
    {
        Player.transform.position = PlayerSpawn.transform.position;
        Player.SetActive(true);

        if(wincondition == GameWinCondition.COLLECTCOINS)
        {
            Player.GetComponent<CollectCoins>().Initialise(gameObject);
        }
    }

    public void LevelSucceed()
    {
        Managers.GetComponent<GameManager>().LevelSucceed(Score);
        AudioManager.Instance.PlayCheer();
    }

    public void LevelFail()
    {
        Managers.GetComponent<GameManager>().LevelFail();
    }

    public void CheckLevelWin()
    {
        if (pWin || wincondition == GameWinCondition.SURVIVE)
        {
            pWin = false;
            LevelSucceed();
        }
        else 
            LevelFail();
    }

    public void SetWinAndFinish()
    {
        pWin = true;
        CheckLevelWin();
    }

}
