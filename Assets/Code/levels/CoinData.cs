using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinData : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.GetComponent<CollectCoins>())
        {
            col.gameObject.GetComponent<CollectCoins>().AddCoin(this);
            Destroy(this.gameObject);
        }
    }
}