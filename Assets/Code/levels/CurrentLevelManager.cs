using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeManager;

public class CurrentLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    [SerializeField] private GameObject PlayerSpawn;

    [SerializeField] private GameObject Managers;

    public float Score = 10;
    private bool pWin = false;

    private void Start()
    {
        GetPlayer();
        SpawnPlayer();
        Managers.GetComponent<LevelManager>().CurrentLevelManager = this;
        Managers.GetComponent<TimeManager>().pPlayTime = true;
        Managers.GetComponent<TimeManager>().state = TimeState.Playing;
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
    }

    public void LevelSucceed()
    {
        Managers.GetComponent<GameManager>().LevelSucceed(Score);
    }

    public void LevelFail()
    {
        Managers.GetComponent<GameManager>().LevelFail(Score);
    }

    public void CheckLevelWin()
    {
        if (pWin)
            LevelSucceed();
        else 
            LevelFail();

       // pWin = false;
    }

}