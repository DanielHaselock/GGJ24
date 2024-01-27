using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectCoins : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private List<GameObject> Coins;

    [SerializeField] private GameObject LevelEditor;
    [SerializeField] private int CoinsCollected;
    void Start()
    {
        
    }

    public void Initialise(GameObject pLeveleditor)
    {
        Coins = GameObject.FindGameObjectsWithTag("Coin").ToList();
        LevelEditor = pLeveleditor;
    }
    
    public void AddCoin(GameObject Coin)
    {
        Coins.Remove(Coin);
        CoinsCollected++;
        if(Coins.Count == 0)
        {
            LevelEditor.GetComponent<CurrentLevelManager>().SetWinAndFinish();
        }
    }

}
