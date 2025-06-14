using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawning : MonoBehaviour
{
    [SerializeField] public GameObject Coin;
    [SerializeField] public GameObject MoneyBag;

    private void Awake()
    {
       int moneyBagChance = Random.Range(0, 10);

       if (moneyBagChance == 0)
       {
        Instantiate(MoneyBag, this.transform.position, Quaternion.identity);
       }
       else
       {
        Instantiate(Coin, this.transform.position, Quaternion.identity);
       }
       
       this.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
    }
}