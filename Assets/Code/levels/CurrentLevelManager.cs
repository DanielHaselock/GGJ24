using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    [SerializeField] private GameObject PlayerSpawn;

    private void Start()
    {
        GetPlayer();
        SpawnPlayer();
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


}
