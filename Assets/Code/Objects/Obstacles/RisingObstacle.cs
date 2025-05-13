using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingObstacle : GenericObstacle
{
    private Rigidbody2D m_rb;
    private BoxCollider2D collider;
    public ParticleSystem confetti;

    protected override void Start()
    {
        base.Start();
        m_rb = GetComponent<Rigidbody2D>();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.tag.Equals("Player"))
        {
            m_animator.SetTrigger("Pop");
            AudioManager.Instance.Pop();
            confetti.Play();
            m_rb.bodyType = RigidbodyType2D.Static;
            collider.enabled = false;
        }
    }

    public void Destroy()
    {
        if (confetti.isStopped) {
            Destroy(gameObject);
        }
    }
}
