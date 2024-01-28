using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinData : MonoBehaviour
{
    private Animator m_animator;
    private bool m_collected;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(!m_collected && col.gameObject.GetComponent<CollectCoins>())
        {
            m_collected = true;
            col.gameObject.GetComponent<CollectCoins>().AddCoin(this);
            m_animator.SetTrigger("Collect");
            AudioManager.Instance.Coin();
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}