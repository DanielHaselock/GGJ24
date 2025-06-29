using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BouncyObject : GenericObstacle
{
    private Rigidbody2D m_rb;

    [SerializeField] private bool moving_object;
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

    protected void FixedUpdate()
    {
        if (moving_object == true) {
            m_animator.SetBool("is_moving", moving_object);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(BounceSound());

        if (m_animator == null) // I think the ball hits another ball that has just spawned -- it doesn't have an animator setup yet so it cannot play
            return;

        base.OnCollisionEnter2D(collision);

        Vector2 relativevVelocity = collision.relativeVelocity;

        if (collision.collider.tag.Equals("Player") || collision.collider.tag.Equals("Object") || collision.collider.tag.Equals("Hazard"))
            m_animator.SetTrigger("Bounce");
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
