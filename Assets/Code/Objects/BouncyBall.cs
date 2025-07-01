using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BouncyBall : GenericObstacle
{
    private Rigidbody2D m_rb;

    [SerializeField] private float m_bouncingforce = 0.5f;
    [SerializeField] private AudioClip m_bounceSound;
    [SerializeField] private float audio_pitch = 1f;

    private AudioSource m_audioSource;

    protected override void Start()
    {
        base.Start();
        m_rb = GetComponent<Rigidbody2D>();
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.clip = m_bounceSound;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(BounceSound());

        if (m_animator == null) // I think the ball hits another ball that has just spawned -- it doesn't have an animator setup yet so it cannot play
            return;

        base.OnCollisionEnter2D(collision);

        Vector2 relativevVelocity = collision.relativeVelocity;

        if (Mathf.Abs(Mathf.Acos(Vector2.Dot(relativevVelocity, Vector2.down))) < Mathf.PI / 4)
            m_animator.SetTrigger("BounceFloor");

        else
            m_animator.SetTrigger("BounceWall");

        if (collision.collider.tag.Equals("Player") || collision.collider.tag.Equals("Object"))
            m_animator.SetTrigger("Bounce");

        // Get average surface normal
        Vector2 averageNormal = Vector2.zero;
        foreach (var item in collision.contacts)
        {
            averageNormal += item.normal;
        }
        averageNormal /= collision.contacts.Length;

        // Bouncing direction
        m_rb.linearVelocity = (-relativevVelocity + 2 * Vector2.Dot(relativevVelocity, averageNormal) * averageNormal) * m_bouncingforce;
    }

    IEnumerator BounceSound()
    {
        if(m_audioSource == null)
        {
            yield return null;
        }

        if (m_audioSource != null && !m_audioSource.isPlaying)
        {
            m_audioSource.pitch = audio_pitch;
            m_audioSource.Play();
        }
        yield return new WaitForSeconds(m_audioSource.clip.length);
    }
}
