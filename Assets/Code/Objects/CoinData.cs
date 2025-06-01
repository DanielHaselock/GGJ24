using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinData : MonoBehaviour
{
    private Animator m_animator;
    private bool m_collected;

    [SerializeField] private AudioClip collectSound;
    [SerializeField] private int scoreValue = 100;

    private AudioSource audioSource;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = collectSound;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (m_collected || col.gameObject.GetComponent<PlayerController>() == null)
            return;
        m_collected = true;
        m_animator.SetTrigger("Collect");
        Collect(col.gameObject.GetComponent<PlayerController>());
    }

    private void Collect(PlayerController player)
    {
        audioSource.PlayOneShot(collectSound);
        PlayerManager.Instance.players.Find(p => p.PlayerIndex == player.PlayerIndex).AddScore(scoreValue);
        FindFirstObjectByType<CoinLevelManager>().OnCollectCoin();
        StartCoroutine(FinishCollect());
    }

    IEnumerator FinishCollect()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}