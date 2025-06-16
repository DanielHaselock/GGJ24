using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinData : MonoBehaviour
{
    private Animator m_animator;
    private bool m_collected;

    [SerializeField] private AudioClip collectSound;
    [SerializeField] private GameObject scoreGraphic;
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
        Instantiate(scoreGraphic, transform.position,transform.rotation);
        PlayerManager.Instance.players.Find(p => p.PlayerIndex == player.PlayerIndex).AddScore(scoreValue);
        if(GameManager.Instance.CurrentGameState == GameManager.GameStates.CoinLevel)
            FindFirstObjectByType<CoinLevelManager>().OnCollectCoin(player);
        StartCoroutine(FinishCollect());
    }

    IEnumerator FinishCollect()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !m_animator.IsInTransition(0));
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}